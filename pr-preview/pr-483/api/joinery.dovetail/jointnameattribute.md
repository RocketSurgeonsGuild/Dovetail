# JointNameAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/Infrastructure/JointNameAttribute.cs)
- Inherits: `Attribute`

A marker interface to indicate a type is a convention and has a name

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed class JointNameAttribute
    : System.Attribute
{
    // ...
}
```

## Constructors

- [JointNameAttribute(string)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/jointnameattribute/constructors.md#constructor-string) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [Name](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/jointnameattribute/properties.md#name) : `string` `get` -- The name of the joint
