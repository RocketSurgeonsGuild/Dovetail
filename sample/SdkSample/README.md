# Joinery.Dovetail.Sdk sample

Consumes Dovetail purely as an MSBuild SDK from packed NuGet packages, the way an external project
would. No project here references Dovetail directly or imports anything from `src/`. The only
wiring is one line in [`Directory.Build.props`](Directory.Build.props):

```xml
<Project Sdk="Joinery.Dovetail.Sdk">
```

with the SDK version pinned in [`global.json`](global.json) under `msbuild-sdks`.

## What it exercises

| Project                   | Shape                   | The SDK should...                                                                                                                                               |
| ------------------------- | ----------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `SdkSample.Greetings`     | plain class library     | inject `Joinery.Dovetail`, export its `[DovetailExport]` joints, rename `appsettings.yaml` to `SdkSample.Greetings.yaml` and generate a YAML configuration joint for it |
| `SdkSample.Notifications` | plain class library     | the same, for a section with every common member type (see below)                                                                                               |
| `SdkSample.Storage`       | plain class library     | the same, for immutable options that Dovetail creates through generated code (see below)                                                                        |
| `SdkSample.Diagnostics`   | plain class library     | the same, for options of types the library doesn't own (see below)                                                                                              |
| `SdkSample.Worker`        | `OutputType=Exe`        | detect a `HostApplicationBuilder` host, inject `Joinery.Dovetail.Hosting`, generate `ConfigureDovetail()`                                                               |
| `SdkSample.Web`           | `Microsoft.NET.Sdk.Web` | detect a `WebApplicationBuilder` host, inject `Joinery.Dovetail.Hosting.Web`, generate `ConfigureDovetail()`                                                            |

`DovetailEnableConfiguration=true` (set in `Directory.Build.props`) additionally injects
`Joinery.Dovetail.Configuration.Yaml` into every project. YAML is the default format; setting
`DovetailEnableJsonConfiguration` or `DovetailEnableTomlConfiguration` instead opts into that format.

## What each library shows

Every library is a plain class library in the style of `SdkSample.Greetings`: options declared with
`[DovetailConfiguration]`, XML docs on every member, defaults in its own `appsettings.yaml` (renamed to
`<Project>.yaml`), and an exported joint that registers a small service reading those options through
`IOptions<T>`. Dovetail's generated binder fills the options and its generated joint loads the YAML file, so the
joints never register the options themselves.

| Library                   | Section(s)                       | Shows                                                                                                                                                                                                                                                                                                               |
| ------------------------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `SdkSample.Greetings`     | `Greetings`, `Farewell`          | the basics: a class with `[Description]` and `[Range]`, and a positional record with a `<param>` description. `IGreeter`                                                                                                                                                                                            |
| `SdkSample.Notifications` | `Notifications`                  | a plain enum and a `[Flags]` enum, `TimeSpan`, `Uri`, `List<string>`, a `Dictionary<string, ChannelOptions>` of documented nested objects with `[Range]`, `[Description]`, `[EmailAddress]` and an `[Obsolete("Use Sender instead.")]` property. `INotifier`                                                        |
| `SdkSample.Storage`       | `Storage`, `Archive`             | the generated factory path: a positional record with constructor defaults, and a class with a `required` member and `init`-only properties. `[RegularExpression]`, a `string[]`, dash-case (`Storage`) and underscore-case (`Archive`) keys through `KeyStyle`, and `[ConfigurationKeyName("keep_days")]`. `IStore` |
| `SdkSample.Diagnostics`   | `Diagnostics`, `Host`, `Console` | types the library doesn't own, through `[assembly: DovetailConfiguration<HostOptions>("Host")]` and `[assembly: DovetailConfiguration<ConsoleLifetimeOptions>("Console")]`, whose descriptions in the schema come from the XML docs shipped in `Microsoft.Extensions.Hosting`. `IDiagnosticsReport`                 |

`SdkSample.Storage` shows two key styles. `StorageOptions` declares `KeyStyle = ConfigurationKeyStyle.Dash`, so its section
is `storage` and its keys `root`, `max-file-size-mb` and `compress`. `ArchiveOptions` declares
`KeyStyle = ConfigurationKeyStyle.Underscore`: section `archive`, keys `name`, `allowed_extensions` and `keep_days`. The
binder reads those keys and the schema suggests them. The explicit `[ConfigurationKeyName("keep_days")]` wins over the
`retention_days` the style would give. The other libraries keep the declared PascalCase keys.

Both hosts reference all four libraries. Each host's merged `obj/appsettings.schema.json` combines the sections of
every one of them (plus SchemaStore's `Logging`, `Kestrel` and the rest of the stock settings).
[`.vscode/settings.json`](.vscode/settings.json) maps those schemas to the `appsettings*` files for JSON, YAML and
TOML.

### Layered configuration

Each library ships its defaults in its own file. A host's own `appsettings.yaml` (renamed by the SDK to
`SdkSample.Worker.yaml` and `SdkSample.Web.yaml`) is loaded after them, so it overrides what it sets and leaves
the rest alone:

| Host               | Overrides                                                                                                           |
| ------------------ | ------------------------------------------------------------------------------------------------------------------- |
| `SdkSample.Worker` | `Greetings:Repeat: 2`, `Notifications:MinimumLevel: Info`, `Notifications:Sender`, `Host:ShutdownTimeout: 00:00:05` |
| `SdkSample.Web`    | `Farewell:Exclamations: 3`, `storage:max-file-size-mb: 128`, `Console:SuppressStatusMessages: true`                 |

## Running it

From this directory:

```sh
dotnet msbuild build/pack.proj   # pack src/** as 0.0.0-sample into artifacts/nuget-sample
dotnet run --project SdkSample.Worker
dotnet run --project SdkSample.Web    # then GET / and GET /config
```

or from anywhere in the repo: `mise run sample:sdk`.

Expected worker output, one line (the greeting is said twice here) or two per library:

```text
Hello from SdkSample.Greetings.yaml, SdkSample.Worker! Hello from SdkSample.Greetings.yaml, SdkSample.Worker!
Goodbye from SdkSample.Greetings.yaml, SdkSample.Worker!
Sent 'Worker started' as Info from worker@example.com via Email (60/min) + Push (30/min) to ops@example.com (timeout 20s).
Store 'sdk-sample' in ./data: files up to 64 MB, kept 30 days, accepts .txt .json .csv.
Diagnostics 'sdk-sample': level Information, sampling 100%, host shutdown timeout 5s, console status messages on.
```

The Web app answers `GET /` with the greeting and `GET /config` with the resolved options of every library as JSON
(enums as names). With the Worker's overrides absent and the Web ones applied you see, among other things:

```json
{
    "farewell": { "message": "Goodbye from SdkSample.Greetings.yaml", "exclamations": 3 },
    "notifications": { "minimumLevel": "Warning", "defaultChannels": "Email, Push", "timeout": "00:00:20" },
    "storage": { "root": "./data", "maxFileSizeMb": 128, "compress": false },
    "host": { "shutdownTimeout": "00:00:15" },
    "console": { "suppressStatusMessages": true }
}
```

In the Development environment the Web app also serves the merged schema at
`/.well-known/dovetail/configuration-schema`:

```sh
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://127.0.0.1:5198 dotnet run --project SdkSample.Web
curl http://127.0.0.1:5198/config
curl http://127.0.0.1:5198/.well-known/dovetail/configuration-schema
```

## Try the schema

1. Build a host once so the schema exists: `dotnet build SdkSample.Worker` writes
   `SdkSample.Worker/obj/appsettings.schema.json` (`SdkSample.Web/obj/...` for the web app).
2. Open `SdkSample.Notifications/appsettings.yaml` in VS Code with the Red Hat YAML extension. The settings in
   [`.vscode/settings.json`](.vscode/settings.json) attach the Worker's schema to it. Every library's file, and
   each host's, gets completion for all sections.
3. Try some edits:
    - Misspell a key under `Notifications`, for example `Recipent:`. The strict schema flags it as an unknown key and
      lists the known ones. Keys are case-insensitive, so `recipients:` is fine.
    - Write `Timeout: soon` or `Enabled: maybe`. A wrong type or format is flagged.
    - Set `RateLimitPerMinute: 0` under a channel, or `storage: max-file-size-mb: 5000`. Both are outside their `[Range]`.
    - Add `From: ops@example.com` under `Notifications`. The obsolete property is marked deprecated, with the
      message "Use Sender instead."
    - In `archive`, set `name: Not Valid`. It doesn't match the `[RegularExpression]`.
    - Write `MaxFileSizeMb` under `storage`, or `AllowedExtensions` under `archive`. `Storage` is declared in dash case and
      `Archive` in underscore case, so only `max-file-size-mb` and `allowed_extensions` are known; the PascalCase spellings
      are flagged as unknown keys (they would not bind either). The other libraries keep their declared PascalCase keys.
    - Hover over `Host:ShutdownTimeout` or `Console:SuppressStatusMessages`. The text is Microsoft's own XML documentation
      for `HostOptions` and `ConsoleLifetimeOptions`, not something written in this sample.
4. The same schema covers `appsettings*.json` and `appsettings*.toml` files placed in a host project.

Rerun the pack step after changing anything under `src/`. It also clears the Dovetail entries from
this sample's own package cache (`.nuget/`, see [`NuGet.config`](NuGet.config)), since the version
never changes.

## Why the extra files

- `global.json` pins `Joinery.Dovetail.Sdk`. MSBuild SDKs resolved from NuGet need an explicit version.
- `NuGet.config` clears the repository's feeds and maps `Joinery.Dovetail*` to the local feed.
- `Directory.Packages.props` and `Directory.Build.targets` are intentionally empty so MSBuild stops
  here instead of picking up the repository's central package management and build targets.
- `.gitignore` ignores `appsettings.local.*`. With configuration enabled, the SDK warns when no
  `.gitignore` at or above a project ignores those developer-only overrides.
