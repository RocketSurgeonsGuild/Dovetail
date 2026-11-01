# Joinery.Dovetail.OpenTelemetry

- Package: `Joinery.Dovetail.OpenTelemetry`
- Version: `0.4.2-beta.4`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.OpenTelemetry](https://www.nuget.org/packages/Joinery.Dovetail.OpenTelemetry)

## Namespaces

### Joinery.Dovetail.Joints

- [IOpenTelemetryAsyncJoint](/Dovetail/api/joinery.dovetail.opentelemetry/iopentelemetryasyncjoint.md) -- `interface` -- Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [IOpenTelemetryJoint](/Dovetail/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md) -- `interface` -- Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [OpenTelemetryAsyncJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate.md) -- `delegate` -- Async delegate for the OpenTelemetry part.
- [OpenTelemetryHostBuilderExtensions](/Dovetail/api/joinery.dovetail.opentelemetry/opentelemetryhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying OpenTelemetry parts to a `Dovetail.DovetailContextBuilder`.
- [OpenTelemetryJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate.md) -- `delegate` -- Delegate for the OpenTelemetry part.
- [OpenTelemetryJointExtensions](/Dovetail/api/joinery.dovetail.opentelemetry/joinery.dovetail.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying OpenTelemetry parts.

### Joinery.Dovetail.OpenTelemetry.Joints

- [ILoggerProviderBuilderJoint](/Dovetail/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint.md) -- `interface` -- Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`
- [IMeterProviderBuilderJoint](/Dovetail/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint.md) -- `interface` -- Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [IResourceBuilderJoint](/Dovetail/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint.md) -- `interface` -- Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [ITracerProviderJoint](/Dovetail/api/joinery.dovetail.opentelemetry/itracerproviderjoint.md) -- `interface` -- Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [LoggerProviderJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Logs.LoggerProviderBuilder` part.
- [MeterProviderJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate.md) -- `delegate` -- Delegate for the `Metrics.MeterProvider` part.
- [OpenTelemetryJointExtensions](/Dovetail/api/joinery.dovetail.opentelemetry/joinery.dovetail.opentelemetry.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying LoggerProvider parts.
- [ResourceBuilderJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate.md) -- `delegate` -- Delegate for the `Resources.ResourceBuilder` part.
- [TracerProviderJointDelegate](/Dovetail/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Trace.TracerProvider` part.
