# MauiAppBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the MauiAppBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void MauiAppBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiAppBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)

## Constructors

- [MauiAppBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui/mauiappbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiAppBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui/mauiappbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-mauiappbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.maui/mauiappbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
