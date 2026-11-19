# AfterJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/Attributes/AfterJointAttribute.cs)
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/v0.8.0/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailjoint.md)

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

- [AfterJointAttribute(Type)](/Dovetail/v0.8.0/api/joinery.dovetail/afterjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailjoint.md)

## See Also

- `Attribute`
