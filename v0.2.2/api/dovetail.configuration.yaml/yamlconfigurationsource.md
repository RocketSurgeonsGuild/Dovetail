# YamlConfigurationSource

- Kind: `class`
- Package: [Dovetail.Configuration.Yaml](/Dovetail/v0.2.2/api/dovetail.configuration.yaml.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Inherits: `FileConfigurationSource`

A YAML file based `Configuration.FileConfigurationSource`.

## Definition

```csharp
namespace Dovetail.Configuration.Yaml;

public class YamlConfigurationSource
    : Microsoft.Extensions.Configuration.FileConfigurationSource
{
    // ...
}
```

## Constructors

- [YamlConfigurationSource](/Dovetail/v0.2.2/api/dovetail.configuration.yaml/yamlconfigurationsource/constructors.md#constructor)

## Methods

- [Build(IConfigurationBuilder)](/Dovetail/v0.2.2/api/dovetail.configuration.yaml/yamlconfigurationsource/methods.md#build-iconfigurationbuilder) : `IConfigurationProvider` -- Builds the `Configuration.IConfigurationProvider` for this source.
