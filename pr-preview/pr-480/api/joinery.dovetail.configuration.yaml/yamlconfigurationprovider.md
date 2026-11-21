# YamlConfigurationProvider

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.2-pr.5`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-480-merge.Sha.bbd00571859e9fa257a29d29502064fe14badc48/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationProvider.cs)
- Inherits: `FileConfigurationProvider`

A YAML file based `Configuration.FileConfigurationProvider`.

## Definition

```csharp
namespace Joinery.Dovetail.Configuration.Yaml;

public class YamlConfigurationProvider
    : Microsoft.Extensions.Configuration.FileConfigurationProvider
{
    // ...
}
```

## Remarks

The yaml configuration provider

## Constructors

- [YamlConfigurationProvider(YamlConfigurationSource)](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider/constructors.md#constructor-yamlconfigurationsource) -- A YAML file based `Configuration.FileConfigurationProvider`.

## Methods

- [Load(Stream)](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider/methods.md#load-stream) -- Loads this provider's data from a stream.
