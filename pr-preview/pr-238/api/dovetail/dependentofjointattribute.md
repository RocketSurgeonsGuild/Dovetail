# DependentOfJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-238/api/dovetail.md)
- Version: `0.2.2-pr.3`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-238/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class DependentOfJointAttribute
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
{
    // ...
}
```

## Remarks

The type to be used with the convention type

## Constructors

- [DependentOfJointAttribute(Type)](/Dovetail/pr-preview/pr-238/api/dovetail/dependentofjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailjoint.md)

## See Also

- `Attribute`
