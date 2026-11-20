# ILoggerProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail.OpenTelemetry/Joints/ILoggerProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public interface ILoggerProviderBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, LoggerProviderBuilder)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint/methods.md#register-idovetailcontext-loggerproviderbuilder) `abstract` -- Register the `Logs.LoggerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
