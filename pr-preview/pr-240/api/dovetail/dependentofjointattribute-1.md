# DependentOfJointAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-240/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class DependentOfJointAttribute<T>
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md) that this convention must run before.

## Constructors

- [DependentOfJointAttribute](/Dovetail/pr-preview/pr-240/api/dovetail/dependentofjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
