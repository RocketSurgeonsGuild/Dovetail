# YamlConfigurationSource

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.2-beta.21`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.f0c56e1dcadc9219196cd48facce90770bb6bd05/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs)
- Inherits: `FileConfigurationSource`

A YAML file based `Configuration.FileConfigurationSource`.

## Definition

```csharp
namespace Joinery.Dovetail.Configuration.Yaml;

public class YamlConfigurationSource
    : Microsoft.Extensions.Configuration.FileConfigurationSource
{
    // ...
}
```

## Constructors

- [YamlConfigurationSource](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/constructors.md#constructor)

## Methods

- [Build(IConfigurationBuilder)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/methods.md#build-iconfigurationbuilder) : `IConfigurationProvider` -- Builds the `Configuration.IConfigurationProvider` for this source.
