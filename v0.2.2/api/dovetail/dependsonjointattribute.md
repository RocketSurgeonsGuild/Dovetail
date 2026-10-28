# DependsOnJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/v0.2.2/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class DependsOnJointAttribute
    : System.Attribute,
      Dovetail.Infrastructure.IDovetailDependency
{
    // ...
}
```

## Remarks

The type to be used with the convention type

## Constructors

- [DependsOnJointAttribute(Type)](/Dovetail/v0.2.2/api/dovetail/dependsonjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

## See Also

- `Attribute`
