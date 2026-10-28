# LiveJointAttribute

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Version: `0.2.2-pr.2`
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

- [LiveJointAttribute](/Dovetail/pr-preview/pr-236/api/dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
