# DependentOfJointAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

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

- `T` -- The [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md) that this convention must run before.

## Constructors

- [DependentOfJointAttribute](/Dovetail/api/dovetail/dependentofjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
