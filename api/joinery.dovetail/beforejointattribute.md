# BeforeJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.2.3-beta.4`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/api/joinery.dovetail/idovetaildependency.md)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

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

- [BeforeJointAttribute(Type)](/Dovetail/api/joinery.dovetail/beforejointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

## See Also

- `Attribute`
