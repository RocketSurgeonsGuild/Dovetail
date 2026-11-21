# LiveJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/Attributes/LiveJointAttribute.cs)
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

- [LiveJointAttribute](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
