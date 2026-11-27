# ILoggerProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail.OpenTelemetry/Joints/ILoggerProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, LoggerProviderBuilder)](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint/methods.md#register-idovetailcontext-loggerproviderbuilder) `abstract` -- Register the `Logs.LoggerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
