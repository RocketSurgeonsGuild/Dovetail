# JointNameAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.4.1-beta.3`
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

- [JointNameAttribute(string)](/Dovetail/api/joinery.dovetail/jointnameattribute/constructors.md#constructor-string) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [Name](/Dovetail/api/joinery.dovetail/jointnameattribute/properties.md#name) : `string` `get` -- The name of the joint
