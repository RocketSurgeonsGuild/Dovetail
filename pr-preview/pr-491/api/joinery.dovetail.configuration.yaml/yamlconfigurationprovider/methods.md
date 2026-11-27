# YamlConfigurationProvider Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationProvider](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationProvider`.

<a id="load"></a>
<a id="load-stream"></a>

## Load(Stream)

- Name: `Load(Stream)`
- Modifiers: `override`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationProvider.cs#L20-L29)

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
