# YamlConfigurationProvider

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.1-beta.9`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationProvider.cs)
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

- [YamlConfigurationProvider(YamlConfigurationSource)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider/constructors.md#constructor-yamlconfigurationsource) -- A YAML file based `Configuration.FileConfigurationProvider`.

## Methods

- [Load(Stream)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationprovider/methods.md#load-stream) -- Loads this provider's data from a stream.
