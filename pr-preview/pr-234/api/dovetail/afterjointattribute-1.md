# AfterJointAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-234/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-234/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class AfterJointAttribute<T>
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailjoint.md) that this convention must run after.

## Constructors

- [AfterJointAttribute](/Dovetail/pr-preview/pr-234/api/dovetail/afterjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
