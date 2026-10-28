# AfterJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class AfterJointAttribute
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
{
    // ...
}
```

## Remarks

The type to be used with the convention type

## Constructors

- [AfterJointAttribute(Type)](/Dovetail/api/dovetail/afterjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

## See Also

- `Attribute`
