# IAutofacAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-beta.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacAsyncJoint.cs)
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
