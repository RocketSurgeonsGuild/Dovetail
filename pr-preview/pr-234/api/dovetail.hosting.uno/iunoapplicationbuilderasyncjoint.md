# IUnoApplicationBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IUnoApplicationBuilderAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IApplicationBuilder, CancellationToken)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno/iunoapplicationbuilderasyncjoint/methods.md#register-idovetailcontext-iapplicationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the UnoApplicationBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
