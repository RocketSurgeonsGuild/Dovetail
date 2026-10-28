# LiveJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`
- Implements: `IHostBasedJoint`

Defines this convention as one that only runs during live usage to avoid unit tests

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class LiveJointAttribute
    : System.Attribute,
      Dovetail.Infrastructure.IHostBasedJoint
{
    // ...
}
```

## Constructors

- [LiveJointAttribute](/Dovetail/api/dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
