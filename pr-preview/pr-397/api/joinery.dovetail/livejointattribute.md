# LiveJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/Attributes/LiveJointAttribute.cs)
- Inherits: `Attribute`
- Implements: `IHostBasedJoint`

Defines this convention as one that only runs during live usage to avoid unit tests

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class LiveJointAttribute
    : System.Attribute,
      Joinery.Dovetail.Infrastructure.IHostBasedJoint
{
    // ...
}
```

## Constructors

- [LiveJointAttribute](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
