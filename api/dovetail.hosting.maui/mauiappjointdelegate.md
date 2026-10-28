# MauiAppJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Maui](/Dovetail/api/dovetail.hosting.maui.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the MauiApp part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void MauiAppJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiApp app)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `app` (`MauiApp`)

## Constructors

- [MauiAppJointDelegate(object, nint)](/Dovetail/api/dovetail.hosting.maui/mauiappjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiApp, AsyncCallback, object)](/Dovetail/api/dovetail.hosting.maui/mauiappjointdelegate/methods.md#begininvoke-idovetailcontext-mauiapp-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.hosting.maui/mauiappjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
