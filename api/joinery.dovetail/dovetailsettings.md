# DovetailSettings

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.23`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail/DovetailSettings.cs)

Settings that change how Dovetail sets up a host, read from the host's own configuration (environment variables, command line, or anything added before Dovetail runs) rather than from Dovetail's configuration files.

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailSettings
{
    // ...
}
```

## Methods

- [GetConfiguredDirectory(IConfiguration?)](/Dovetail/api/joinery.dovetail/dovetailsettings/methods.md#getconfigureddirectory-iconfiguration) : `string?` `static` -- The full path of [DovetailSettings.ConfigurationDirectory](/Dovetail/api/joinery.dovetail/dovetailsettings/fields.md#configurationdirectory) when `configuration` sets it, or else the `Dovetail__ConfigurationDirectory` environment variable does (for hosts whose configuration doesn't read environment variables); otherwise `null`.
- [GetEntryPointFileDirectory](/Dovetail/api/joinery.dovetail/dovetailsettings/methods.md#getentrypointfiledirectory) : `string?` `static` -- The directory of the entry point file of a file-based app ( `dotnet run Build.cs`), where its configuration files sit, or `null` for any other app.

## Fields

- [ConfigurationDirectory](/Dovetail/api/joinery.dovetail/dovetailsettings/fields.md#configurationdirectory) : `string` `static` -- The configuration key naming the directory the configuration joints read their files from. Every host that reads configuration files from disk honors it. Without it, the joints read from the directory of the entry point file of a file-based app ( `dotnet run Build.cs`), and otherwise from the host's own file provider, which the host roots at its content root. A relative path is resolved against the current directory. Wherever they look, a project whose base file isn't there has its files read from the application's output directory, where the build copied them.
