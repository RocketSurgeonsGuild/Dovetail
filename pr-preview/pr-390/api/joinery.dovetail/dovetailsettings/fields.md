# DovetailSettings Fields

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-390/api/joinery.dovetail.md)
- Type: [DovetailSettings](/Dovetail/pr-preview/pr-390/api/joinery.dovetail/dovetailsettings.md)
- Kind: `Fields`
- Members: `1`

Settings that change how Dovetail sets up a host, read from the host's own configuration (environment variables, command line, or anything added before Dovetail runs) rather than from Dovetail's configuration files.

<a id="configurationdirectory"></a>

## ConfigurationDirectory

- Name: `ConfigurationDirectory`
- Modifiers: `static` `const`
- Returns: `string`

The configuration key naming the directory the configuration joints read their files from. Every host that reads configuration files from disk honors it. Without it, the joints read from the directory of the entry point file of a file-based app ( `dotnet run Build.cs`), and otherwise from the host's own file provider, which the host roots at its content root. A relative path is resolved against the current directory.

```csharp
public const static string ConfigurationDirectory
```

## Remarks

As an environment variable it is `Dovetail__ConfigurationDirectory`; on the command line, `--Dovetail:ConfigurationDirectory=path`.

Configuration embedded in the application ( [DovetailEmbeddedConfiguration](/Dovetail/pr-preview/pr-390/api/joinery.dovetail/dovetailembeddedconfiguration.md), as MAUI and Uno apps have) still wins: the directory only applies where the joints read files from disk. A browser (WebAssembly) host has no file system and ignores it.
