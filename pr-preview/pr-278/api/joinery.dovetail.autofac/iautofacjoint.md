# IAutofacJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.autofac.md)
- Version: `0.3.1-pr.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [Register(IDovetailContext, ContainerBuilder)](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.autofac/iautofacjoint/methods.md#register-idovetailcontext-containerbuilder) `abstract` -- Register the Autofac part.

## See Also

- `Infrastructure.IDovetailJoint`
