# IUnoApplicationBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IUnoApplicationBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IApplicationBuilder, CancellationToken)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderasyncjoint/methods.md#register-idovetailcontext-iapplicationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the UnoApplicationBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
