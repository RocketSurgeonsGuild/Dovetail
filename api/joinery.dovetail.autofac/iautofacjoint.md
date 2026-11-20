# IAutofacJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/api/joinery.dovetail.autofac.md)
- Version: `0.8.1-beta.9`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IAutofacJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ContainerBuilder)](/Dovetail/api/joinery.dovetail.autofac/iautofacjoint/methods.md#register-idovetailcontext-containerbuilder) `abstract` -- Register the Autofac part.

## See Also

- `Infrastructure.IDovetailJoint`
