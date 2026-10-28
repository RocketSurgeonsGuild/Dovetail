# IMauiAppJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IMauiAppJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiApp)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui/imauiappjoint/methods.md#register-idovetailcontext-mauiapp) `abstract` -- Register the MauiApp part.

## See Also

- `Infrastructure.IDovetailJoint`
