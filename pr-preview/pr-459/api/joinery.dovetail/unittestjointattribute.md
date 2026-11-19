# UnitTestJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.md)
- Version: `0.7.2-pr.11`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.a1990fe6695c5fa9e9c003a3a333f8ff361aa602/src/Joinery.Dovetail/Attributes/UnitTestJointAttribute.cs)
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

- [UnitTestJointAttribute](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/unittestjointattribute/constructors.md#constructor)

## See Also

- `Attribute`
