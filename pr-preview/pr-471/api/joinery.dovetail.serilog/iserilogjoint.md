# ISerilogJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.serilog.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-471-merge.Sha.bf40892310bcd6ceeb21a319fcdbc6a1dfabd05f/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/ISerilogJoint.cs)
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

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration)](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.serilog/iserilogjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration) `abstract` -- Register the Serilog part.

## See Also

- `Infrastructure.IDovetailJoint`
