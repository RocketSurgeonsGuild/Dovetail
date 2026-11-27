# YamlConfigurationProvider Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-495/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationProvider](/Dovetail/pr-preview/pr-495/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationProvider`.

<a id="load"></a>
<a id="load-stream"></a>

## Load(Stream)

- Name: `Load(Stream)`
- Modifiers: `override`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-495-merge.Sha.fd3d23685f218ca4cae24c50782b49b292f37ef0/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationProvider.cs#L20-L29)

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
