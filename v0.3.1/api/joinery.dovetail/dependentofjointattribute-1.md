# DependentOfJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/v0.3.1/api/joinery.dovetail.md)
- Version: `0.3.1`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/v0.3.1/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/v0.3.1/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class DependentOfJointAttribute<T>
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/v0.3.1/api/joinery.dovetail/idovetailjoint.md) that this convention must run before.

## Constructors

- [DependentOfJointAttribute](/Dovetail/v0.3.1/api/joinery.dovetail/dependentofjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
