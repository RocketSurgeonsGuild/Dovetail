# BeforeJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class BeforeJointAttribute<T>
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/idovetailjoint.md) that this convention must run before.

## Constructors

- [BeforeJointAttribute](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/beforejointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
