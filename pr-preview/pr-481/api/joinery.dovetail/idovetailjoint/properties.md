# IDovetailJoint Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.md)
- Type: [IDovetailJoint](/Dovetail/pr-preview/pr-481/api/joinery.dovetail/idovetailjoint.md)
- Kind: `Properties`
- Members: `1`

A marker interface to indicate a type is a joint

<a id="priority"></a>

## Priority

- Name: `Priority`
- Modifiers: `virtual` `get`
- Returns: `int`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-481-merge.Sha.155deb7a5941f87329ac1f5cf6b4b18b74b0ce52/src/Joinery.Dovetail/Infrastructure/IDovetailJoint.cs#L18)

The absolute priority of the convention. If not provided the default value be 0. Use a negative number to push conventions closer to the front. Use a positive number to push conventions closer to the back. Dovetails will still be ordered based on dependencies, so it is possible for a given convention to move front of a convention using int.MinValue.

```csharp
public virtual int Priority { get; }
```
