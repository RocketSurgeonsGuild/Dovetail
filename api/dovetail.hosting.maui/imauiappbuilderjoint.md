# IMauiAppBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Maui](/Dovetail/api/dovetail.hosting.maui.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IMauiAppBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MauiAppBuilder)](/Dovetail/api/dovetail.hosting.maui/imauiappbuilderjoint/methods.md#register-idovetailcontext-mauiappbuilder) `abstract` -- Register the MauiAppBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
