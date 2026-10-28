# IDryIocAsyncJoint

- Kind: `interface`
- Package: [Dovetail.DryIoc](/Dovetail/api/dovetail.dryioc.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDryIocAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IContainer, CancellationToken)](/Dovetail/api/dovetail.dryioc/idryiocasyncjoint/methods.md#register-idovetailcontext-icontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the DryIoc part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
