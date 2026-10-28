# IMauiAppBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.4-beta.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IMauiAppBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiAppBuilder)](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappbuilderjoint/methods.md#register-idovetailcontext-mauiappbuilder) `abstract` -- Register the MauiAppBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
