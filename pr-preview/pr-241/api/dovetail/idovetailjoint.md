# IDovetailJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-241/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

A marker interface to indicate a type is a joint

## Definition

```csharp
namespace Dovetail.Infrastructure;

public interface IDovetailJoint
{
    // ...
}
```

## Properties

- [Priority](/Dovetail/pr-preview/pr-241/api/dovetail/idovetailjoint/properties.md#priority) : `int` `virtual` `get` -- The absolute priority of the convention. If not provided the default value be 0. Use a negative number to push conventions closer to the front. Use a positive number to push conventions closer to the back. Dovetails will still be ordered based on dependencies, so it is possible for a given convention to move front of a convention using int.MinValue.
