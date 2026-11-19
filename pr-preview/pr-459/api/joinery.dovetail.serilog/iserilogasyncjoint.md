# ISerilogAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.serilog.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/ISerilogAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ISerilogAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.serilog/iserilogasyncjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken) : `ValueTask` `abstract` -- Register the Serilog part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
