# ITestingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Testing](/Dovetail/api/joinery.dovetail.testing.md)
- Version: `0.8.2-beta.24`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c89aa479a31cf66e20128be06e87a71eacecc74f/src/Joinery.Dovetail.Testing/obj/Release/net10.0/ITestingAsyncJoint.cs)
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

- [Register(IDovetailContext, CancellationToken)](/Dovetail/api/joinery.dovetail.testing/itestingasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Testing part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
