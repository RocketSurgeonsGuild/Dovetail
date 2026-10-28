# JointNameAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `Attribute`

A marker interface to indicate a type is a convention and has a name

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed class JointNameAttribute
    : System.Attribute
{
    // ...
}
```

## Constructors

- [JointNameAttribute(string)](/Dovetail/v0.2.2/api/dovetail/jointnameattribute/constructors.md#constructor-string) -- A marker interface to indicate a type is a convention and has a name

## Properties

- [Name](/Dovetail/v0.2.2/api/dovetail/jointnameattribute/properties.md#name) : `string` `get` -- The name of the joint
