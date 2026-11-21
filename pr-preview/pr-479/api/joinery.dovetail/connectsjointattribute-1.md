# ConnectsJointAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/Infrastructure/ConnectsJointAttribute.cs)
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

- [ConnectsJointAttribute](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/connectsjointattribute-1/constructors.md#constructor) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [JointName](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/connectsjointattribute-1/properties.md#jointname) : `string` `get` -- The name of the joint that this joint connects to
