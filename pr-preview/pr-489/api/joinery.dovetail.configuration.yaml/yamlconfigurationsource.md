# YamlConfigurationSource

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs)
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

- [YamlConfigurationSource](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/constructors.md#constructor)

## Methods

- [Build(IConfigurationBuilder)](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/methods.md#build-iconfigurationbuilder) : `IConfigurationProvider` -- Builds the `Configuration.IConfigurationProvider` for this source.
