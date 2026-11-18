# JointNameAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/Infrastructure/JointNameAttribute.cs)
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

- [JointNameAttribute(string)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/jointnameattribute/constructors.md#constructor-string) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [Name](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/jointnameattribute/properties.md#name) : `string` `get` -- The name of the joint
