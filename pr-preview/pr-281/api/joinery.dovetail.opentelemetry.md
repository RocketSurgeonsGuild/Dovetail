# Joinery.Dovetail.OpenTelemetry

- Package: `Joinery.Dovetail.OpenTelemetry`
- Version: `0.3.1-pr.15`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.OpenTelemetry](https://www.nuget.org/packages/Joinery.Dovetail.OpenTelemetry)

## Namespaces

### Joinery.Dovetail.Joints

- [IOpenTelemetryAsyncJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/iopentelemetryasyncjoint.md) -- `interface` -- Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md) -- `interface` -- Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [OpenTelemetryAsyncJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate.md) -- `delegate` -- Async delegate for the OpenTelemetry part.
- [OpenTelemetryHostBuilderExtensions](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/opentelemetryhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying OpenTelemetry parts to a `Dovetail.DovetailContextBuilder`.
- [OpenTelemetryJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate.md) -- `delegate` -- Delegate for the OpenTelemetry part.
- [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/joinery.dovetail.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying OpenTelemetry parts.

### Joinery.Dovetail.OpenTelemetry.Joints

- [ILoggerProviderBuilderJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint.md) -- `interface` -- Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`
- [IMeterProviderBuilderJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint.md) -- `interface` -- Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [IResourceBuilderJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint.md) -- `interface` -- Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [ITracerProviderJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/itracerproviderjoint.md) -- `interface` -- Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [LoggerProviderJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Logs.LoggerProviderBuilder` part.
- [MeterProviderJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate.md) -- `delegate` -- Delegate for the `Metrics.MeterProvider` part.
- [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying LoggerProvider parts.
- [ResourceBuilderJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate.md) -- `delegate` -- Delegate for the `Resources.ResourceBuilder` part.
- [TracerProviderJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Trace.TracerProvider` part.
