# ITestingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing.md)
- Version: `0.8.2-pr.5`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail.Testing/obj/Release/net10.0/ITestingJoint.cs)
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

- [Register(IDovetailContext)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing/itestingjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Testing part.

## See Also

- `Infrastructure.IDovetailJoint`
