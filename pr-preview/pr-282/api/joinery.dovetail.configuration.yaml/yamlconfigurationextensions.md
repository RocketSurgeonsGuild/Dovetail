# YamlConfigurationExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.3.1-pr.17`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`

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

- [AddYamlFile(IConfigurationBuilder, string)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool, bool)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, IFileProvider?, string, bool, bool)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-ifileprovider-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlFile(IConfigurationBuilder, Action<YamlConfigurationSource>)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-action-yamlconfigurationsource) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlStream(IConfigurationBuilder, Stream)](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlstream-iconfigurationbuilder-stream) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder` that reads from a `IO.Stream`.
