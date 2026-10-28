# IUnoHostBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Uno](/Dovetail/api/dovetail.hosting.uno.md)
- Version: `0.2.3-beta.1`
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

- [Register(IDovetailContext, IHostBuilder, CancellationToken)](/Dovetail/api/dovetail.hosting.uno/iunohostbuilderasyncjoint/methods.md#register-idovetailcontext-ihostbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the UnoHostBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
