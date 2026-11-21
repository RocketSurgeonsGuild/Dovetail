# YamlConfigurationExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-481-merge.Sha.155deb7a5941f87329ac1f5cf6b4b18b74b0ce52/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationExtensions.cs)

Extension methods for adding YAML configuration sources to an `Configuration.IConfigurationBuilder`.

## Definition

```csharp
namespace Joinery.Dovetail.Configuration.Yaml;

public static class YamlConfigurationExtensions
{
    // ...
}
```

## Methods

- [AddYamlFile(IConfigurationBuilder, string)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool, bool)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, IFileProvider?, string, bool, bool)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-ifileprovider-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlFile(IConfigurationBuilder, Action<YamlConfigurationSource>)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-action-yamlconfigurationsource) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlStream(IConfigurationBuilder, Stream)](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlstream-iconfigurationbuilder-stream) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder` that reads from a `IO.Stream`.
