# DependsOnJointAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-245/api/dovetail.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-245/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-245/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class DependsOnJointAttribute<T>
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-245/api/dovetail/idovetailjoint.md) that this convention must run after.

## Constructors

- [DependsOnJointAttribute](/Dovetail/pr-preview/pr-245/api/dovetail/dependsonjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
