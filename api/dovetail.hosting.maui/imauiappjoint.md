# IMauiAppJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Maui](/Dovetail/api/dovetail.hosting.maui.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IMauiAppJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiApp)](/Dovetail/api/dovetail.hosting.maui/imauiappjoint/methods.md#register-idovetailcontext-mauiapp) `abstract` -- Register the MauiApp part.

## See Also

- `Infrastructure.IDovetailJoint`
