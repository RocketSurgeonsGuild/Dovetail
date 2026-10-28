# IAutofacAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Autofac](/Dovetail/api/dovetail.autofac.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IAutofacAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ContainerBuilder, CancellationToken)](/Dovetail/api/dovetail.autofac/iautofacasyncjoint/methods.md#register-idovetailcontext-containerbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Autofac part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
