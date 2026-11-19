# ILoggerProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-466/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-466-merge.Sha.8c32fc2383e0ecb5c3325a81237d52253f24a3ca/src/Joinery.Dovetail.OpenTelemetry/Joints/ILoggerProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-466/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, LoggerProviderBuilder)](/Dovetail/pr-preview/pr-466/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint/methods.md#register-idovetailcontext-loggerproviderbuilder) `abstract` -- Register the `Logs.LoggerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
