# UnitTestJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: `IHostBasedJoint`

Defines this convention as one that only runs during a unit test run

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class UnitTestJointAttribute
    : System.Attribute,
      Dovetail.Infrastructure.IHostBasedJoint
{
    // ...
}
```

## Constructors

- [UnitTestJointAttribute](/Dovetail/pr-preview/pr-235/api/dovetail/unittestjointattribute/constructors.md#constructor)

## See Also

- `Attribute`
