---
title: Configuration Files
description: How the Dovetail SDK renames, packs, copies, embeds and loads a library's appsettings files in JSON, YAML or TOML.
tags: [configuration, msbuild]
---

# Configuration Files

A library's configuration values ship in its own `appsettings` files, and the Dovetail SDK takes them to every application that
references it. Binding those values to types, and the JSON Schema editors use, come from
[Optionary](./configuration-with-optionary).

Defaults live in a normal `appsettings` file in the library. The SDK renames it, packs it, and loads it into the
host's configuration, where the options binding (Optionary's, or your own) picks it up:

```
MyLibrary/
├── appsettings.yaml
├── appsettings.Development.yaml
├── appsettings.local.yaml   # never packed, never committed
└── MyLibrary.csproj
```

- `appsettings.{ext}` becomes `MyLibrary.{ext}` (`appsettings.Production.json` becomes `MyLibrary.Production.json`), so several
  libraries can ship their own file into one output directory. The name is the project's assembly name (`AssemblyName`, which
  defaults to the project's name): the `ApplicationName` the runtime conventions look for.
- The files are packed under `contentFiles/any/any/configuration/`, copied to a same-solution host's output directory
  through a `ProjectReference`, and restored from the package otherwise. Either way they land at the root of the output
  directory, next to the application (the package flattens them out of `configuration/`).
- The generated joint loads, in ascending precedence, `MyLibrary.{ext}`, `MyLibrary.{Environment}.{ext}` and
  `MyLibrary.local.{ext}`. The `local` layer is never packed, and the SDK warns when no `.gitignore` at or above the
  project ignores `appsettings.local.*` (a `*.local.*` entry counts too).
- YAML has two extensions and one joint. It requires `MyLibrary.yaml` or `MyLibrary.yml`, whichever the library has, not both,
  and reads the environment and local layers with either extension, `.yaml` before `.yml` in each layer.
- A file-based app (`dotnet run Build.cs`) is named after its assembly, `Build`, without the `.cs`: its `appsettings.{ext}`
  files, next to `Build.cs`, are copied to its output directory as `Build.{ext}`, with or without the Dovetail SDK. At run time,
  though, the configuration joints read the files from next to `Build.cs` (the directory the .NET SDK names in the
  `EntryPointFileDirectoryPath` AppContext data), under their source names, so an edit takes effect on the next run without a
  rebuild. A host or pipeline whose configuration was given a base path or file provider of its own keeps it.
- Outside a file-based app, the joints read the files through the host's own file provider, the one its configuration
  uses for `appsettings.json`: the host roots it at its content root, unless `SetBasePath` or `SetFileProvider` pointed it
  somewhere else. `dotnet run` leaves the content root at the current directory, and Aspire starts a project in its project
  directory.
- A joint that doesn't find its base file (`MyLibrary.{ext}`) there reads its files from the application's output directory
  (`AppContext.BaseDirectory`), where the build copied them. That is where a library's files always are, so
  `dotnet run --project MyApp` from any directory, or from the application's project directory, finds every library's files.
- A library ships its `appsettings.{ext}` files as `MyLibrary.{ext}`, so its joint looks for that name only: an
  `appsettings.{ext}` where the host reads files is the application's. The application's own joint also looks for its
  source names (`appsettings.{ext}`), which it finds in its project directory, so an edit there takes effect on the next
  `dotnet run` without a rebuild. The application is the project the host is named after, its `ApplicationName` (the
  entry assembly's name unless the host was given another).
- To read the files from another directory, set `Dovetail:ConfigurationDirectory` (`DovetailSettings.ConfigurationDirectory`)
  in the host's configuration, for example `Dovetail__ConfigurationDirectory=./config` or
  `--Dovetail:ConfigurationDirectory=./config`. A relative path is resolved against the current directory. It wins over
  everything above for the files it holds; a project whose base file it doesn't hold reads its files from the output
  directory, so the directory need only hold the files you override. The `Dovetail__ConfigurationDirectory` environment
  variable is read directly too, for hosts whose configuration doesn't read environment variables.
- Every host that reads the files from disk (the generic host, ASP.NET Core, ModularPipelines, Uno) picks the
  directory in the same order: `Dovetail:ConfigurationDirectory`, then the directory of a file-based app's entry point file,
  then the host's own file provider, and each joint falls back to the output directory. Configuration embedded in the application (MAUI and Uno apps) still wins over all of
  them: a MAUI app always reads its embedded copies, and a browser (WebAssembly) host fetches its files over HTTP.

### Naming the source files

A library's configuration files can be written under either of two names, and both end up in the same place:

| Source file                                            | Copied, packed and embedded as                     | Included when              |
| ------------------------------------------------------ | -------------------------------------------------- | -------------------------- |
| `appsettings.{ext}`, `appsettings.{Environment}.{ext}` | `MyLibrary.{ext}`, `MyLibrary.{Environment}.{ext}` | `appsettings.{ext}` exists |
| `MyLibrary.{ext}`, `MyLibrary.{Environment}.{ext}`     | the same name                                      | `MyLibrary.{ext}` exists   |
| `appsettings.local.{ext}` or `MyLibrary.local.{ext}`   | `MyLibrary.local.{ext}`, never packed              | as above                   |

`MyLibrary` is the assembly name, as above: in a `lib.csproj` whose `AssemblyName` is `Contoso.Library`, `Contoso.Library.yaml`
is configuration and `lib.yaml` is an ordinary file, because nothing would load a `lib.yaml`. A file-based app's `Build.yaml`, next
to `Build.cs`, is included the same way. Files named after the assembly are treated exactly like the `appsettings` ones: copied
to the output directory, packed under `contentFiles/any/any/configuration/`, embedded, loaded by the generated joint, and added to a
file-based app's items. `MyLibrary.schema.json` and a file-based app's `Build.run.json` (its launch profiles) are not
configuration and are left alone. `appsettings.local.*` doesn't ignore `MyLibrary.local.{ext}`, so when one exists the SDK
warns, naming the file, until a `.gitignore` has `MyLibrary.local.*` or `*.local.*`, which covers both.

Each pair of names that would land on the same file, such as `appsettings.yaml` and `MyLibrary.yaml`, or
`appsettings.Development.yaml` and `MyLibrary.Development.yaml`, fails the build with
`DT0019`; keep one of them. Different extensions are different files, so `appsettings.yaml` and `MyLibrary.yml`
coexist, as do `appsettings.yaml` and `MyLibrary.Development.yaml` (which is only included next to a `MyLibrary.yaml`).

### Opting in

- JSON, YAML and TOML are supported. Opt in with `DovetailEnableConfiguration`; YAML is the default format, and
  `DovetailEnableJsonConfiguration` or `DovetailEnableTomlConfiguration` chooses another. Hosts with no file system
  (MAUI, Uno) read copies embedded in the assembly: `DovetailEmbedConfiguration`.

| Property                          | Default                            | Purpose                                                                                             |
| --------------------------------- | ---------------------------------- | --------------------------------------------------------------------------------------------------- |
| `DovetailEnableConfiguration`     | `false`                            | Turns the pipeline on. Existing consumers see no change until it or a format flag is set.           |
| `DovetailEnableYamlConfiguration` | `true` when no other format is set | Adds the YAML provider. Setting it to `true` also enables the pipeline.                             |
| `DovetailEnableJsonConfiguration` | `false`                            | Adds the JSON provider. Setting it to `true` also enables the pipeline, replacing the YAML default. |
| `DovetailEnableTomlConfiguration` | `false`                            | Adds the TOML provider. Setting it to `true` also enables the pipeline, replacing the YAML default. |
| `DovetailPackConfiguration`       | `true`, `false` if not packable    | Packs the library's configuration files into the package.                                           |

## Diagnostics

| Id       | Severity | Meaning                                                                                                                                          |
| -------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| `DT0019` | Error    | A project has both `appsettings{suffix}.{ext}` and `{AssemblyName}{suffix}.{ext}`, which land on the same file. Reported by the build; keep one. |
