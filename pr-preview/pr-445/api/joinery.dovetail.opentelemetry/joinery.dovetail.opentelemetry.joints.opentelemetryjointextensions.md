# OpenTelemetryJointExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail.OpenTelemetry/Joints/OpenTelemetryJointExtensions.cs)

Extension methods for applying LoggerProvider parts.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public static class OpenTelemetryJointExtensions
{
    // ...
}
```

## Methods

- [ConfigureLoggerProvider(DovetailContextBuilder, LoggerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions/methods.md#configureloggerprovider-dovetailcontextbuilder-loggerproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string) : `DovetailContextBuilder` `extension`
- [ConfigureMeterProvider(DovetailContextBuilder, MeterProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions/methods.md#configuremeterprovider-dovetailcontextbuilder-meterproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string) : `DovetailContextBuilder` `extension`
- [ConfigureResourceBuilder(DovetailContextBuilder, ResourceBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions/methods.md#configureresourcebuilder-dovetailcontextbuilder-resourcebuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string) : `DovetailContextBuilder` `extension`
- [ConfigureTracerProvider(DovetailContextBuilder, TracerProviderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions/methods.md#configuretracerprovider-dovetailcontextbuilder-tracerproviderjointdelegate-int-dovetailcategory-dovetailhosttype-string) : `DovetailContextBuilder` `extension`
