# ISerilogJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.serilog.md)
- Version: `0.3.1-pr.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration)](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.serilog/iserilogjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration) `abstract` -- Register the Serilog part.

## See Also

- `Infrastructure.IDovetailJoint`
