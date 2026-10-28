# DependsOnJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: [IDovetailDependency](/Dovetail/api/dovetail/idovetaildependency.md)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

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

- [DependsOnJointAttribute(Type)](/Dovetail/api/dovetail/dependsonjointattribute/constructors.md#constructor-type) -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

## See Also

- `Attribute`
