# IMauiAppBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.3-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IMauiAppBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiAppBuilder, CancellationToken)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.maui/imauiappbuilderasyncjoint/methods.md#register-idovetailcontext-mauiappbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the MauiAppBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
