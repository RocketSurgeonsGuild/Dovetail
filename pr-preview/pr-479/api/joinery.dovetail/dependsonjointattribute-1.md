# DependsOnJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/Attributes/DependsOnJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)

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

- `T` -- The [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md) that this convention must run after.

## Constructors

- [DependsOnJointAttribute](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dependsonjointattribute-1/constructors.md#constructor)

## See Also

- `Attribute`
