# ILoggingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.2.3/api/joinery.dovetail.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ILoggingJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ILoggingBuilder)](/Dovetail/v0.2.3/api/joinery.dovetail/iloggingjoint/methods.md#register-idovetailcontext-iloggingbuilder) `abstract` -- Register the Logging part.

## See Also

- [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)
