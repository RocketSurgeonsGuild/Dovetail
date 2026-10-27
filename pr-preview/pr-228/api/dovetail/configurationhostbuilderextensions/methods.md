# ConfigurationHostBuilderExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Type: [ConfigurationHostBuilderExtensions](/Dovetail/pr-preview/pr-228/api/dovetail/configurationhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Configuration parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontextbuilder.md).

<a id="configureconfiguration"></a>
<a id="configureconfiguration-dovetailcontextbuilder-configurationjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontextbuilder.md)

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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationJointDelegate](/Dovetail/pr-preview/pr-228/api/dovetail/configurationjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configureconfiguration-dovetailcontextbuilder-configurationasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontextbuilder.md)

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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationAsyncJointDelegate](/Dovetail/pr-preview/pr-228/api/dovetail/configurationasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
