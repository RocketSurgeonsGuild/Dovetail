# IMauiAppJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.maui.md)
- Version: `0.2.1-pr.19`
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

- [Register(IDovetailContext, MauiApp)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.maui/imauiappjoint/methods.md#register-idovetailcontext-mauiapp) `abstract` -- Register the MauiApp part.

## See Also

- `Infrastructure.IDovetailJoint`
