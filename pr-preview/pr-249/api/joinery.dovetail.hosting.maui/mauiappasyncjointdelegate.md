# MauiAppAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the MauiApp part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask MauiAppAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiApp app,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `app` (`MauiApp`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [MauiAppAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiappasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiApp, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiappasyncjointdelegate/methods.md#begininvoke-idovetailcontext-mauiapp-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiappasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
