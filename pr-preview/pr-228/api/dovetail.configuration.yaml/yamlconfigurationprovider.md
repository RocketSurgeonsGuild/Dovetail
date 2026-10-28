# YamlConfigurationProvider

- Kind: `class`
- Package: [Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-228/api/dovetail.configuration.yaml.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Inherits: `FileConfigurationProvider`

A YAML file based `Configuration.FileConfigurationProvider`.

## Definition

```csharp
namespace Dovetail.Configuration.Yaml;

public class YamlConfigurationProvider
    : Microsoft.Extensions.Configuration.FileConfigurationProvider
{
    // ...
}
```

## Remarks

The yaml configuration provider

## Constructors

- [YamlConfigurationProvider(YamlConfigurationSource)](/Dovetail/pr-preview/pr-228/api/dovetail.configuration.yaml/yamlconfigurationprovider/constructors.md#constructor-yamlconfigurationsource) -- A YAML file based `Configuration.FileConfigurationProvider`.

## Methods

- [Load(Stream)](/Dovetail/pr-preview/pr-228/api/dovetail.configuration.yaml/yamlconfigurationprovider/methods.md#load-stream) -- Loads this provider's data from a stream.
