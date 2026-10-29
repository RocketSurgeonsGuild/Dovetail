# IAutofacAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.autofac.md)
- Version: `0.3.1-pr.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IAutofacAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ContainerBuilder, CancellationToken)](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.autofac/iautofacasyncjoint/methods.md#register-idovetailcontext-containerbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Autofac part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
