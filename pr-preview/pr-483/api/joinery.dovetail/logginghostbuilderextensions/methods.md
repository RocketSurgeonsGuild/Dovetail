# LoggingHostBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.md)
- Type: [LoggingHostBuilderExtensions](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/logginghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Logging parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="configurelogging"></a>
<a id="configurelogging-dovetailcontextbuilder-loggingjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs)

```csharp
public static class LoggingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureLogging(
        this DovetailContextBuilder container,
        LoggingJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingJointDelegate](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/loggingjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configurelogging-dovetailcontextbuilder-loggingasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs)

```csharp
public static class LoggingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureLogging(
        this DovetailContextBuilder container,
        LoggingAsyncJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingAsyncJointDelegate](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/loggingasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
