# ILoggingJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface ILoggingJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ILoggingBuilder)](/Dovetail/api/dovetail/iloggingjoint/methods.md#register-idovetailcontext-iloggingbuilder) `abstract` -- Register the Logging part.

## See Also

- [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)
