# LiveJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/Attributes/LiveJointAttribute.cs)
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

- [LiveJointAttribute](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
