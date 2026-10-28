# ConnectsJointAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `Attribute`

A marker interface to indicate a type is a convention and has a name

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed class ConnectsJointAttribute<T>
    : System.Attribute
    where T : IDovetailJoint
{
    // ...
}
```

## Constructors

- [ConnectsJointAttribute](/Dovetail/pr-preview/pr-236/api/dovetail/connectsjointattribute-1/constructors.md#constructor) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [JointName](/Dovetail/pr-preview/pr-236/api/dovetail/connectsjointattribute-1/properties.md#jointname) : `string` `get` -- The name of the joint that this joint connects to
