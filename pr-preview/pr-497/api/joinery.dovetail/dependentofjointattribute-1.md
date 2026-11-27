# DependentOfJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail/Attributes/DependentOfJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetailjoint.md)

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

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetailjoint.md) that this convention must run before.

## Constructors

- [DependentOfJointAttribute](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/dependentofjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
