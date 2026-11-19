# DovetailSettings

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail/DovetailSettings.cs)

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

- [GetConfiguredDirectory(IConfiguration?)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailsettings/methods.md#getconfigureddirectory-iconfiguration) : `string?` `static` -- The full path of [DovetailSettings.ConfigurationDirectory](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailsettings/fields.md#configurationdirectory) when `configuration` sets it, or else the `Dovetail__ConfigurationDirectory` environment variable does (for hosts whose configuration doesn't read environment variables); otherwise `null`.
- [GetEntryPointFileDirectory](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailsettings/methods.md#getentrypointfiledirectory) : `string?` `static` -- The directory of the entry point file of a file-based app ( `dotnet run Build.cs`), where its configuration files sit, or `null` for any other app.

## Fields

- [ConfigurationDirectory](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailsettings/fields.md#configurationdirectory) : `string` `static` -- The configuration key naming the directory the configuration joints read their files from. Every host that reads configuration files from disk honors it. Without it, the joints read from the directory of the entry point file of a file-based app ( `dotnet run Build.cs`), and otherwise from the host's own file provider, which the host roots at its content root. A relative path is resolved against the current directory.
