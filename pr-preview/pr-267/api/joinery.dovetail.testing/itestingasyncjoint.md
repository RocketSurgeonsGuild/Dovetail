# ITestingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.testing.md)
- Version: `0.3.1-pr.6`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Testing. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public interface ITestingAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.testing/itestingasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Testing part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
