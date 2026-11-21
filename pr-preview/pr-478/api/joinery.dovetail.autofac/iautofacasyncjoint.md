# IAutofacAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-478-merge.Sha.3cefccc51138c5f35edba1dddeb09aae10973315/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacAsyncJoint.cs)
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

- [Register(IDovetailContext, ContainerBuilder, CancellationToken)](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.autofac/iautofacasyncjoint/methods.md#register-idovetailcontext-containerbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Autofac part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
