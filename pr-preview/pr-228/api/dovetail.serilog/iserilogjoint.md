# ISerilogJoint

- Kind: `interface`
- Package: [Dovetail.Serilog](/Dovetail/pr-preview/pr-228/api/dovetail.serilog.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface ISerilogJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection, LoggerConfiguration)](/Dovetail/pr-preview/pr-228/api/dovetail.serilog/iserilogjoint/methods.md#register-idovetailcontext-iservicecollection-loggerconfiguration) `abstract` -- Register the Serilog part.

## See Also

- `Infrastructure.IDovetailJoint`
