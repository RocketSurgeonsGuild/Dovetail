# UnitTestJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail/Attributes/UnitTestJointAttribute.cs)
- Inherits: `Attribute`
- Implements: `IHostBasedJoint`

Defines this convention as one that only runs during a unit test run

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class UnitTestJointAttribute
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IHostBasedJoint
{
    // ...
}
```

## Constructors

- [UnitTestJointAttribute](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/unittestjointattribute/constructors.md#constructor)

## See Also

- `Attribute`
