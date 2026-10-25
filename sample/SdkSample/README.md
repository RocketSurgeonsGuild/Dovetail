# Dovetail.Sdk sample

Consumes Dovetail purely as an MSBuild SDK from packed NuGet packages, the way an external project
would. No project here references Dovetail directly or imports anything from `src/`. The only
wiring is one line in [`Directory.Build.props`](Directory.Build.props):

```xml
<Project Sdk="Dovetail.Sdk">
```

with the SDK version pinned in [`global.json`](global.json) under `msbuild-sdks`.

## What it exercises

| Project               | Shape                   | The SDK should...                                                                                                                                               |
| --------------------- | ----------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `SdkSample.Greetings` | plain class library     | inject `Dovetail`, export its `[DovetailExport]` joints, rename `appsettings.yaml` to `SdkSample.Greetings.yaml` and generate a YAML configuration joint for it |
| `SdkSample.Worker`    | `OutputType=Exe`        | detect a `HostApplicationBuilder` host, inject `Dovetail.Hosting`, generate `ConfigureDovetail()`                                                               |
| `SdkSample.Web`       | `Microsoft.NET.Sdk.Web` | detect a `WebApplicationBuilder` host, inject `Dovetail.Hosting.Web`, generate `ConfigureDovetail()`                                                            |

`DovetailEnableConfiguration=true` (set in `Directory.Build.props`) additionally injects
`Dovetail.Configuration.Yaml` into every project. YAML is the default format; setting
`DovetailEnableJsonConfiguration` or `DovetailEnableTomlConfiguration` instead opts into that format.

Both hosts resolve `IGreeter`, which is registered by the library's exported joint. It reads
`GreetingsOptions` (a class, section `Greetings`) and `FarewellOptions` (a positional record, section
`Farewell`) through `IOptions<T>`; both are declared with `[DovetailConfiguration]`, bound from the library's
YAML file by Dovetail's generated binder, and described in each host's merged
`obj/appsettings.schema.json`. [`.vscode/settings.json`](.vscode/settings.json) maps those schemas to the
`appsettings*` files for JSON, YAML and TOML; build a host once, then open
`SdkSample.Greetings/appsettings.yaml` in VS Code to try completion.

## Running it

From this directory:

```sh
dotnet msbuild build/pack.proj   # pack src/** as 0.0.0-sample into artifacts/nuget-sample
dotnet run --project SdkSample.Worker
dotnet run --project SdkSample.Web    # then GET /
```

or from anywhere in the repo: `mise run sample:sdk`.

Expected worker output:

```text
Hello from SdkSample.Greetings.yaml, SdkSample.Worker!
Goodbye from SdkSample.Greetings.yaml, SdkSample.Worker!
```

Rerun the pack step after changing anything under `src/`. It also clears the Dovetail entries from
this sample's own package cache (`.nuget/`, see [`NuGet.config`](NuGet.config)), since the version
never changes.

## Why the extra files

- `global.json` pins `Dovetail.Sdk`. MSBuild SDKs resolved from NuGet need an explicit version.
- `NuGet.config` clears the repository's feeds and maps `Dovetail*` to the local feed.
- `Directory.Packages.props` and `Directory.Build.targets` are intentionally empty so MSBuild stops
  here instead of picking up the repository's central package management and build targets.
- `.gitignore` ignores `appsettings.local.*`. With configuration enabled, the SDK warns when no
  `.gitignore` at or above a project ignores those developer-only overrides.
