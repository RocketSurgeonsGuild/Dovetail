# YamlConfigurationSource Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationSource](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationSource`.

<a id="build"></a>
<a id="build-iconfigurationbuilder"></a>

## Build(IConfigurationBuilder)

- Name: `Build(IConfigurationBuilder)`
- Modifiers: `override`
- Returns: `IConfigurationProvider`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs#L13-L14)

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
