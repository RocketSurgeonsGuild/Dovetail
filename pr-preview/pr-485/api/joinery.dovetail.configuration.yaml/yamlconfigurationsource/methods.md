# YamlConfigurationSource Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationSource](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationSource`.

<a id="build"></a>
<a id="build-iconfigurationbuilder"></a>

## Build(IConfigurationBuilder)

- Name: `Build(IConfigurationBuilder)`
- Modifiers: `override`
- Returns: `IConfigurationProvider`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs#L13-L14)

Builds the `Configuration.IConfigurationProvider` for this source.

```csharp
public class YamlConfigurationSource
{
    public override IConfigurationProvider Build(
        IConfigurationBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
  The `Configuration.IConfigurationBuilder`.

## Returns

`IConfigurationProvider` -- To be added.
