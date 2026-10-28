# IAutofacJoint

- Kind: `interface`
- Package: [Dovetail.Autofac](/Dovetail/pr-preview/pr-240/api/dovetail.autofac.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IAutofacJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ContainerBuilder)](/Dovetail/pr-preview/pr-240/api/dovetail.autofac/iautofacjoint/methods.md#register-idovetailcontext-containerbuilder) `abstract` -- Register the Autofac part.

## See Also

- `Infrastructure.IDovetailJoint`
