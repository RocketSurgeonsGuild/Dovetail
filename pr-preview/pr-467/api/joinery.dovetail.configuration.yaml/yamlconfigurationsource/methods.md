# YamlConfigurationSource Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-467/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationSource](/Dovetail/pr-preview/pr-467/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationSource`.

<a id="build"></a>
<a id="build-iconfigurationbuilder"></a>

## Build(IConfigurationBuilder)

- Name: `Build(IConfigurationBuilder)`
- Modifiers: `override`
- Returns: `IConfigurationProvider`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-467-merge.Sha.d99fa32788fc6ba98c72987996ae5cba72e8fc58/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs#L13-L14)

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
