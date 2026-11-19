# OpenTelemetryJointExtensions Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions.md)
- Kind: `Methods`
- Members: `4`

Extension methods for applying LoggerProvider parts.

<a id="configureloggerprovider"></a>
<a id="configureloggerprovider-dovetailcontextbuilder-loggerproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureLoggerProvider(DovetailContextBuilder, LoggerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureLoggerProvider(DovetailContextBuilder, LoggerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.OpenTelemetry/Joints/OpenTelemetryJointExtensions.cs#L29-L30)

```csharp
public static class OpenTelemetryJointExtensions
{
    public static DovetailContextBuilder ConfigureLoggerProvider(
        this DovetailContextBuilder container,
        LoggerProviderJointDelegate @delegate,
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
- `delegate` ([LoggerProviderJointDelegate](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuremeterprovider"></a>
<a id="configuremeterprovider-dovetailcontextbuilder-meterproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureMeterProvider(DovetailContextBuilder, MeterProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureMeterProvider(DovetailContextBuilder, MeterProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.OpenTelemetry/Joints/OpenTelemetryJointExtensions.cs#L75-L76)

```csharp
public static class OpenTelemetryJointExtensions
{
    public static DovetailContextBuilder ConfigureMeterProvider(
        this DovetailContextBuilder container,
        MeterProviderJointDelegate @delegate,
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
- `delegate` ([MeterProviderJointDelegate](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureresourcebuilder"></a>
<a id="configureresourcebuilder-dovetailcontextbuilder-resourcebuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureResourceBuilder(DovetailContextBuilder, ResourceBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureResourceBuilder(DovetailContextBuilder, ResourceBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.OpenTelemetry/Joints/OpenTelemetryJointExtensions.cs#L45-L46)

```csharp
public static class OpenTelemetryJointExtensions
{
    public static DovetailContextBuilder ConfigureResourceBuilder(
        this DovetailContextBuilder container,
        ResourceBuilderJointDelegate @delegate,
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
- `delegate` ([ResourceBuilderJointDelegate](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuretracerprovider"></a>
<a id="configuretracerprovider-dovetailcontextbuilder-tracerproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureTracerProvider(DovetailContextBuilder, TracerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureTracerProvider(DovetailContextBuilder, TracerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.OpenTelemetry/Joints/OpenTelemetryJointExtensions.cs#L60-L61)

```csharp
public static class OpenTelemetryJointExtensions
{
    public static DovetailContextBuilder ConfigureTracerProvider(
        this DovetailContextBuilder container,
        TracerProviderJointDelegate @delegate,
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
- `delegate` ([TracerProviderJointDelegate](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
