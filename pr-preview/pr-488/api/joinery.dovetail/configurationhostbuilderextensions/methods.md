# ConfigurationHostBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.md)
- Type: [ConfigurationHostBuilderExtensions](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/configurationhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Configuration parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="configureconfiguration"></a>
<a id="configureconfiguration-dovetailcontextbuilder-configurationjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.b923ce22aa77b414a24bbc6f5b0f138765cba600/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Configuration.g.cs)

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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationJointDelegate](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/configurationjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configureconfiguration-dovetailcontextbuilder-configurationasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureConfiguration(DovetailContextBuilder, ConfigurationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.b923ce22aa77b414a24bbc6f5b0f138765cba600/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Configuration.g.cs)

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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ConfigurationAsyncJointDelegate](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/configurationasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
