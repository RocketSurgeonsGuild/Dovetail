# IMauiAppBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui.md)
- Version: `0.2.3-pr.3`
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

- [Register(IDovetailContext, MauiAppBuilder)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui/imauiappbuilderjoint/methods.md#register-idovetailcontext-mauiappbuilder) `abstract` -- Register the MauiAppBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
