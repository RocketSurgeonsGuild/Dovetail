# ConfigurationHostBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Type: [ConfigurationHostBuilderExtensions](/Dovetail/v0.4.0/api/joinery.dovetail/configurationhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Configuration parts to a [DovetailContextBuilder](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="configureconfiguration"></a>
<a id="configureconfiguration-dovetailcontextbuilder-configurationjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcontextbuilder.md)

```csharp
public static class ConfigurationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureConfiguration(
        this DovetailContextBuilder container,
        ConfigurationJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` ([DovetailContextBuilder](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationJointDelegate](/Dovetail/v0.4.0/api/joinery.dovetail/configurationjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configureconfiguration-dovetailcontextbuilder-configurationasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcontextbuilder.md)

```csharp
public static class ConfigurationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureConfiguration(
        this DovetailContextBuilder container,
        ConfigurationAsyncJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` ([DovetailContextBuilder](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationAsyncJointDelegate](/Dovetail/v0.4.0/api/joinery.dovetail/configurationasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
