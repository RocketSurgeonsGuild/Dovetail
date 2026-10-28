# Dovetail.OpenTelemetry

- Package: `Dovetail.OpenTelemetry`
- Version: `0.2.3-pr.5`
- Target framework: `net10.0`
- NuGet: [Dovetail.OpenTelemetry](https://www.nuget.org/packages/Dovetail.OpenTelemetry)

## Namespaces

### Dovetail.Joints

- [IOpenTelemetryAsyncJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/iopentelemetryasyncjoint.md) -- `interface` -- Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/iopentelemetryjoint.md) -- `interface` -- Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`
- [OpenTelemetryAsyncJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/opentelemetryasyncjointdelegate.md) -- `delegate` -- Async delegate for the OpenTelemetry part.
- [OpenTelemetryHostBuilderExtensions](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/opentelemetryhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying OpenTelemetry parts to a `DovetailContextBuilder`.
- [OpenTelemetryJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/opentelemetryjointdelegate.md) -- `delegate` -- Delegate for the OpenTelemetry part.
- [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/dovetail.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying OpenTelemetry parts.

### Dovetail.OpenTelemetry

- [OpenTelemetryConnector](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/opentelemetryconnector.md) -- `class` -- A joint that registers OpenTelemetry services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

### Dovetail.OpenTelemetry.Joints

- [ILoggerProviderBuilderJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/iloggerproviderbuilderjoint.md) -- `interface` -- Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`
- [IMeterProviderBuilderJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/imeterproviderbuilderjoint.md) -- `interface` -- Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [IResourceBuilderJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/iresourcebuilderjoint.md) -- `interface` -- Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [ITracerProviderJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/itracerproviderjoint.md) -- `interface` -- Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`
- [LoggerProviderJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/loggerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Logs.LoggerProviderBuilder` part.
- [MeterProviderJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/meterproviderjointdelegate.md) -- `delegate` -- Delegate for the `Metrics.MeterProvider` part.
- [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/dovetail.opentelemetry.joints.opentelemetryjointextensions.md) -- `class` `static` -- Extension methods for applying LoggerProvider parts.
- [ResourceBuilderJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/resourcebuilderjointdelegate.md) -- `delegate` -- Delegate for the `Resources.ResourceBuilder` part.
- [TracerProviderJointDelegate](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/tracerproviderjointdelegate.md) -- `delegate` -- Delegate for the `Trace.TracerProvider` part.
