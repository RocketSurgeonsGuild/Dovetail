# DependsOnJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail/Attributes/DependsOnJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class DependsOnJointAttribute<T>
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IDovetailDependency
    where T : IDovetailJoint
{
    // ...
}
```

## Type Parameters

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md) that this convention must run after.

## Constructors

- [DependsOnJointAttribute](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/dependsonjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
