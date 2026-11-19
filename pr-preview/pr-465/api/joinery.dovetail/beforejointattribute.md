# BeforeJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/Attributes/BeforeJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class BeforeJointAttribute
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IDovetailDependency
{
    // ...
}
```

## Remarks

The type to be used with the convention type

## Constructors

- [BeforeJointAttribute(Type)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/beforejointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

## See Also

- `Attribute`
