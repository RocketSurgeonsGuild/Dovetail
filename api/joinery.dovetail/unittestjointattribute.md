# UnitTestJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.7.2-beta.15`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Attributes/UnitTestJointAttribute.cs)
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

- [UnitTestJointAttribute](/Dovetail/api/joinery.dovetail/unittestjointattribute/constructors.md#constructor)

## See Also

- `Attribute`
