# YamlConfigurationProvider Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationProvider](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationProvider`.

<a id="load"></a>
<a id="load-stream"></a>

## Load(Stream)

- Name: `Load(Stream)`
- Modifiers: `override`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationProvider.cs#L20-L29)

Loads this provider's data from a stream.

```csharp
public class YamlConfigurationProvider
{
    public override void Load(
        Stream stream)
    {
        // ...
    }
}
```

## Parameters

- `stream` (`Stream`)
  The stream to read.

## Exceptions

- `FormatException` -- The YAML content in `stream` could not be parsed.
