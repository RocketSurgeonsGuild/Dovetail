# ILoggerProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.opentelemetry.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public interface ILoggerProviderBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, LoggerProviderBuilder)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint/methods.md#register-idovetailcontext-loggerproviderbuilder) `abstract` -- Register the `Logs.LoggerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
