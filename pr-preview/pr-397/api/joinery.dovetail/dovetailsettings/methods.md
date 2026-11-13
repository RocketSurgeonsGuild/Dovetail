# DovetailSettings Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Type: [DovetailSettings](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/dovetailsettings.md)
- Kind: `Methods`
- Members: `2`

Settings that change how Dovetail sets up a host, read from the host's own configuration (environment variables, command line, or anything added before Dovetail runs) rather than from Dovetail's configuration files.

<a id="getconfigureddirectory"></a>
<a id="getconfigureddirectory-iconfiguration"></a>

## GetConfiguredDirectory(IConfiguration?)

- Name: `GetConfiguredDirectory(IConfiguration?)`
- Modifiers: `static` `nullable`
- Returns: `string?`

The full path of [DovetailSettings.ConfigurationDirectory](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/dovetailsettings/fields.md#configurationdirectory) when `configuration` sets it, or else the `Dovetail__ConfigurationDirectory` environment variable does (for hosts whose configuration doesn't read environment variables); otherwise `null`.

```csharp
public static class DovetailSettings
{
    public static string? GetConfiguredDirectory(
        IConfiguration? configuration)
    {
        // ...
    }
}
```

## Parameters

- `configuration` (`IConfiguration?`)
  The host's configuration.

## Returns

`string?` -- The full path of the directory, or `null`.

<a id="getentrypointfiledirectory"></a>

## GetEntryPointFileDirectory

- Name: `GetEntryPointFileDirectory`
- Modifiers: `static` `nullable`
- Returns: `string?`

The directory of the entry point file of a file-based app ( `dotnet run Build.cs`), where its configuration files sit, or `null` for any other app.

```csharp
public static class DovetailSettings
{
    public static string? GetEntryPointFileDirectory()
    {
        // ...
    }
}
```

## Returns

`string?` -- The directory, or `null`.

## Remarks

The .NET SDK writes the directory into the runtimeconfig.json, so a published or copied file-based app still names the source directory of the machine it was built on; where that doesn't exist, this is `null`.
