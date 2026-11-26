# IDovetailJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-493/api/joinery.dovetail.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-493-merge.Sha.6497bf3ce2e5a41f8b0e13cad9b2c876f4918e36/src/Joinery.Dovetail/Infrastructure/IDovetailJoint.cs)

A marker interface to indicate a type is a joint

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public interface IDovetailJoint
{
    // ...
}
```

## Properties

- [Priority](/Dovetail/pr-preview/pr-493/api/joinery.dovetail/idovetailjoint/properties.md#priority) : `int` `virtual` `get` -- The absolute priority of the convention. If not provided the default value be 0. Use a negative number to push conventions closer to the front. Use a positive number to push conventions closer to the back. Dovetails will still be ordered based on dependencies, so it is possible for a given convention to move front of a convention using int.MinValue.
