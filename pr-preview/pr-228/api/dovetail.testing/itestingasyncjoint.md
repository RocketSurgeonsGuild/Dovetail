# ITestingAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Testing](/Dovetail/pr-preview/pr-228/api/dovetail.testing.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Testing. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public interface ITestingAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-228/api/dovetail.testing/itestingasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Testing part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
