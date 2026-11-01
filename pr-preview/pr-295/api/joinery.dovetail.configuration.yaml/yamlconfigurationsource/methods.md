# YamlConfigurationSource Methods

- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.configuration.yaml.md)
- Type: [YamlConfigurationSource](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource.md)
- Kind: `Methods`
- Members: `1`

A YAML file based `Configuration.FileConfigurationSource`.

<a id="build"></a>
<a id="build-iconfigurationbuilder"></a>

## Build(IConfigurationBuilder)

- Name: `Build(IConfigurationBuilder)`
- Modifiers: `override`
- Returns: `IConfigurationProvider`

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
