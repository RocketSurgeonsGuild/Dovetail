# ITestingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.testing.md)
- Version: `0.7.2-pr.13`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-447-merge.Sha.07e64d25a30d87fc48248bbdc4924e0013be133c/src/Joinery.Dovetail.Testing/obj/Release/net10.0/ITestingJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Testing. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public interface ITestingJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext)](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.testing/itestingjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Testing part.

## See Also

- `Infrastructure.IDovetailJoint`
