# IAutofacAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-beta.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.6690600a1d8dc3e5d18fb6e69136cd6b75569d71/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacAsyncJoint.cs)
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

- [Register(IDovetailContext, ContainerBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.autofac/iautofacasyncjoint/methods.md#register-idovetailcontext-containerbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Autofac part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
