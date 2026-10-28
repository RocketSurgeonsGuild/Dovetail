# IMauiAppAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.3-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IMauiAppAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiApp, CancellationToken)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.maui/imauiappasyncjoint/methods.md#register-idovetailcontext-mauiapp-cancellationtoken) : `ValueTask` `abstract` -- Register the MauiApp part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
