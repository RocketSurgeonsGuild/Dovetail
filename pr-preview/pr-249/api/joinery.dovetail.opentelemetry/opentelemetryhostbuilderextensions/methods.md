# OpenTelemetryHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryHostBuilderExtensions](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.opentelemetry/opentelemetryhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying OpenTelemetry parts to a `Dovetail.DovetailContextBuilder`.

<a id="configureopentelemetry"></a>
<a id="configureopentelemetry-dovetailcontextbuilder-opentelemetryjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureOpenTelemetry(DovetailContextBuilder, OpenTelemetryJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureOpenTelemetry(DovetailContextBuilder, OpenTelemetryJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class OpenTelemetryHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureOpenTelemetry(
        this DovetailContextBuilder container,
        OpenTelemetryJointDelegate @delegate,
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

- `container` (`DovetailContextBuilder`)
- `delegate` ([OpenTelemetryJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureopentelemetry-dovetailcontextbuilder-opentelemetryasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureOpenTelemetry(DovetailContextBuilder, OpenTelemetryAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureOpenTelemetry(DovetailContextBuilder, OpenTelemetryAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class OpenTelemetryHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureOpenTelemetry(
        this DovetailContextBuilder container,
        OpenTelemetryAsyncJointDelegate @delegate,
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

- `container` (`DovetailContextBuilder`)
- `delegate` ([OpenTelemetryAsyncJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
