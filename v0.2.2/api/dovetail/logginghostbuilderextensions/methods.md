# LoggingHostBuilderExtensions Methods

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [LoggingHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/logginghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Logging parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).

<a id="configurelogging"></a>
<a id="configurelogging-dovetailcontextbuilder-loggingjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md)

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

- `container` ([DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingJointDelegate](/Dovetail/v0.2.2/api/dovetail/loggingjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/v0.2.2/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/v0.2.2/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configurelogging-dovetailcontextbuilder-loggingasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLogging(DovetailContextBuilder, LoggingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md)

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

- `container` ([DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([LoggingAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail/loggingasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/v0.2.2/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/v0.2.2/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
