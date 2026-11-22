# LoggingHostBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [LoggingHostBuilderExtensions](/Dovetail/api/joinery.dovetail/logginghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Logging parts to a [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="configurelogging"></a>
<a id="configurelogging-dovetailcontextbuilder-loggingjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs)

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

- `container` ([DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingJointDelegate](/Dovetail/api/joinery.dovetail/loggingjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configurelogging-dovetailcontextbuilder-loggingasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs)

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

- `container` ([DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingAsyncJointDelegate](/Dovetail/api/joinery.dovetail/loggingasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
