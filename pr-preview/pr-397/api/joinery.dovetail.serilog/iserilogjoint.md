# ISerilogJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.serilog.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/ISerilogJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ISerilogJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.serilog/iserilogjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration) `abstract` -- Register the Serilog part.

## See Also

- `Infrastructure.IDovetailJoint`
