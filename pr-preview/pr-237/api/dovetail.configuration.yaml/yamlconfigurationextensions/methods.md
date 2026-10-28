# YamlConfigurationExtensions Methods

- Package: [Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-237/api/dovetail.configuration.yaml.md)
- Type: [YamlConfigurationExtensions](/Dovetail/pr-preview/pr-237/api/dovetail.configuration.yaml/yamlconfigurationextensions.md)
- Kind: `Methods`
- Members: `6`

Extension methods for adding YAML configuration sources to an `Configuration.IConfigurationBuilder`.

<a id="addyamlfile"></a>
<a id="addyamlfile-iconfigurationbuilder-string"></a>

## AddYamlFile(IConfigurationBuilder, string)

- Name: `AddYamlFile(IConfigurationBuilder, string)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds the YAML configuration provider at `path` to `builder`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlFile(
        this IConfigurationBuilder builder,
        string path)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `path` (`string`)
  Path relative to the base path stored in `IConfigurationBuilder.Properties` of `builder`.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
- `ArgumentException` -- `path` is `null` or empty.

<a id="addyamlfile-iconfigurationbuilder-string-bool"></a>

## AddYamlFile(IConfigurationBuilder, string, bool)

- Name: `AddYamlFile(IConfigurationBuilder, string, bool)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds the YAML configuration provider at `path` to `builder`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlFile(
        this IConfigurationBuilder builder,
        string path,
        bool optional)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `path` (`string`)
  Path relative to the base path stored in `IConfigurationBuilder.Properties` of `builder`.
- `optional` (`bool`)
  Whether the file is optional.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
- `ArgumentException` -- `path` is `null` or empty.

<a id="addyamlfile-iconfigurationbuilder-string-bool-bool"></a>

## AddYamlFile(IConfigurationBuilder, string, bool, bool)

- Name: `AddYamlFile(IConfigurationBuilder, string, bool, bool)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds the YAML configuration provider at `path` to `builder`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlFile(
        this IConfigurationBuilder builder,
        string path,
        bool optional,
        bool reloadOnChange)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `path` (`string`)
  Path relative to the base path stored in `IConfigurationBuilder.Properties` of `builder`.
- `optional` (`bool`)
  Whether the file is optional.
- `reloadOnChange` (`bool`)
  Whether the configuration should be reloaded if the file changes.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
- `ArgumentException` -- `path` is `null` or empty.

<a id="addyamlfile-iconfigurationbuilder-ifileprovider-string-bool-bool"></a>

## AddYamlFile(IConfigurationBuilder, IFileProvider?, string, bool, bool)

- Name: `AddYamlFile(IConfigurationBuilder, IFileProvider?, string, bool, bool)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds a YAML configuration source to `builder`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlFile(
        this IConfigurationBuilder builder,
        IFileProvider? provider,
        string path,
        bool optional,
        bool reloadOnChange)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `provider` (`IFileProvider?`)
  The `FileProviders.IFileProvider` to use to access the file.
- `path` (`string`)
  Path relative to the base path stored in `IConfigurationBuilder.Properties` of `builder`.
- `optional` (`bool`)
  Whether the file is optional.
- `reloadOnChange` (`bool`)
  Whether the configuration should be reloaded if the file changes.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
- `ArgumentException` -- `path` is `null` or empty.

<a id="addyamlfile-iconfigurationbuilder-action-yamlconfigurationsource"></a>

## AddYamlFile(IConfigurationBuilder, Action<YamlConfigurationSource>)

- Name: `AddYamlFile(IConfigurationBuilder, Action<YamlConfigurationSource>)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds a YAML configuration source to `builder`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlFile(
        this IConfigurationBuilder builder,
        Action<YamlConfigurationSource> configureSource)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `configureSource` (`Action<YamlConfigurationSource>`)
  Configures the source.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

<a id="addyamlstream"></a>
<a id="addyamlstream-iconfigurationbuilder-stream"></a>

## AddYamlStream(IConfigurationBuilder, Stream)

- Name: `AddYamlStream(IConfigurationBuilder, Stream)`
- Modifiers: `extension`
- Returns: `IConfigurationBuilder`

Adds a YAML configuration source to `builder` that reads from a `IO.Stream`.

```csharp
public static class YamlConfigurationExtensions
{
    public static IConfigurationBuilder AddYamlStream(
        this IConfigurationBuilder builder,
        Stream stream)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder` to add to.
- `stream` (`Stream`)
  The `IO.Stream` to read the yaml configuration data from.

## Returns

`IConfigurationBuilder` -- The `Configuration.IConfigurationBuilder`.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
- `FormatException` -- The YAML content in `stream` could not be parsed.
