# ConnectsJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail/Infrastructure/ConnectsJointAttribute.cs)
- Inherits: `Attribute`

A marker interface to indicate a type is a convention and has a name

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed class ConnectsJointAttribute<T>
    : System.Attribute
    where T : IDovetailJoint
{
    // ...
}
```

## Constructors

- [ConnectsJointAttribute](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/connectsjointattribute-1/constructors.md#constructor) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [JointName](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/connectsjointattribute-1/properties.md#jointname) : `string` `get` -- The name of the joint that this joint connects to
