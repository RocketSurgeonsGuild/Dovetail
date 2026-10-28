# MauiAppJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.4-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the MauiApp part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void MauiAppJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiApp app)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `app` (`MauiApp`)

## Constructors

- [MauiAppJointDelegate(object, nint)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui/mauiappjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiApp, AsyncCallback, object)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui/mauiappjointdelegate/methods.md#begininvoke-idovetailcontext-mauiapp-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui/mauiappjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
