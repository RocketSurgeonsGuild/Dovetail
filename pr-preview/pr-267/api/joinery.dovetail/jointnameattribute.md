# JointNameAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.md)
- Version: `0.3.1-pr.6`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
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

- [JointNameAttribute(string)](/Dovetail/pr-preview/pr-267/api/joinery.dovetail/jointnameattribute/constructors.md#constructor-string) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [Name](/Dovetail/pr-preview/pr-267/api/joinery.dovetail/jointnameattribute/properties.md#name) : `string` `get` -- The name of the joint
