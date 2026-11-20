# AfterJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail/Attributes/AfterJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class AfterJointAttribute
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IDovetailDependency
{
    // ...
}
```

## Remarks

The type to be used with the convention type

## Constructors

- [AfterJointAttribute(Type)](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/afterjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)

## See Also

- `Attribute`
