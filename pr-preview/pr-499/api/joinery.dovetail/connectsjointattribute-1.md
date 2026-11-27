# ConnectsJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail/Infrastructure/ConnectsJointAttribute.cs)
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

- [ConnectsJointAttribute](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/connectsjointattribute-1/constructors.md#constructor) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [JointName](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/connectsjointattribute-1/properties.md#jointname) : `string` `get` -- The name of the joint that this joint connects to
