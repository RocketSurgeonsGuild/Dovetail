# ISerilogAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.serilog.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/ISerilogAsyncJoint.cs)
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

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.serilog/iserilogasyncjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken) : `ValueTask` `abstract` -- Register the Serilog part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
