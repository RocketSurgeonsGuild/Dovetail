# YamlConfigurationSource

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationSource.cs)
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

- [YamlConfigurationSource](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/constructors.md#constructor)

## Methods

- [Build(IConfigurationBuilder)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.configuration.yaml/yamlconfigurationsource/methods.md#build-iconfigurationbuilder) : `IConfigurationProvider` -- Builds the `Configuration.IConfigurationProvider` for this source.
