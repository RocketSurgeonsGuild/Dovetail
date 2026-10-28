# ITestingJoint

- Kind: `interface`
- Package: [Dovetail.Testing](/Dovetail/v0.2.2/api/dovetail.testing.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Testing. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public interface ITestingJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext)](/Dovetail/v0.2.2/api/dovetail.testing/itestingjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Testing part.

## See Also

- `Infrastructure.IDovetailJoint`
