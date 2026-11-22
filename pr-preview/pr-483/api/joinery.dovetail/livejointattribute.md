# LiveJointAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/Attributes/LiveJointAttribute.cs)
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

- [LiveJointAttribute](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/livejointattribute/constructors.md#constructor)

## See Also

- `Attribute`
