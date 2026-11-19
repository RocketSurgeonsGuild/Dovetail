# IDovetailJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/Infrastructure/IDovetailJoint.cs)

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

- [Priority](/Dovetail/api/joinery.dovetail/idovetailjoint/properties.md#priority) : `int` `virtual` `get` -- The absolute priority of the convention. If not provided the default value be 0. Use a negative number to push conventions closer to the front. Use a positive number to push conventions closer to the back. Dovetails will still be ordered based on dependencies, so it is possible for a given convention to move front of a convention using int.MinValue.
