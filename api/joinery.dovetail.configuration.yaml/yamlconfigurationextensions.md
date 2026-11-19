# YamlConfigurationExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.Configuration.Yaml](/Dovetail/api/joinery.dovetail.configuration.yaml.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Configuration.Yaml`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.Configuration.Yaml/YamlConfigurationExtensions.cs)

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

- [AddYamlFile(IConfigurationBuilder, string)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, string, bool, bool)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds the YAML configuration provider at `path` to `builder`.
- [AddYamlFile(IConfigurationBuilder, IFileProvider?, string, bool, bool)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-ifileprovider-string-bool-bool) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlFile(IConfigurationBuilder, Action<YamlConfigurationSource>)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlfile-iconfigurationbuilder-action-yamlconfigurationsource) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder`.
- [AddYamlStream(IConfigurationBuilder, Stream)](/Dovetail/api/joinery.dovetail.configuration.yaml/yamlconfigurationextensions/methods.md#addyamlstream-iconfigurationbuilder-stream) : `IConfigurationBuilder` `extension` -- Adds a YAML configuration source to `builder` that reads from a `IO.Stream`.
