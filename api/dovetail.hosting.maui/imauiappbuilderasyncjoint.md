# IMauiAppBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Maui](/Dovetail/api/dovetail.hosting.maui.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IMauiAppBuilderAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiAppBuilder, CancellationToken)](/Dovetail/api/dovetail.hosting.maui/imauiappbuilderasyncjoint/methods.md#register-idovetailcontext-mauiappbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the MauiAppBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
