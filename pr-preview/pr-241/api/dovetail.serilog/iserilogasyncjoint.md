# ISerilogAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Serilog](/Dovetail/pr-preview/pr-241/api/dovetail.serilog.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface ISerilogAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken)](/Dovetail/pr-preview/pr-241/api/dovetail.serilog/iserilogasyncjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken) : `ValueTask` `abstract` -- Register the Serilog part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
