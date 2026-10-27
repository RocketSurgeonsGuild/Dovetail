# IUnoHostBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.uno.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IUnoHostBuilderAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IHostBuilder, CancellationToken)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.uno/iunohostbuilderasyncjoint/methods.md#register-idovetailcontext-ihostbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the UnoHostBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
