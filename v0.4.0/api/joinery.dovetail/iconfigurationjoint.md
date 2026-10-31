# IConfigurationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IConfigurationJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IConfigurationBuilder)](/Dovetail/v0.4.0/api/joinery.dovetail/iconfigurationjoint/methods.md#register-idovetailcontext-iconfigurationbuilder) `abstract` -- Register the Configuration part.

## See Also

- [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)
