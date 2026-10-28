# IUnoHostBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IUnoHostBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IHostBuilder, CancellationToken)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.uno/iunohostbuilderasyncjoint/methods.md#register-idovetailcontext-ihostbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the UnoHostBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
