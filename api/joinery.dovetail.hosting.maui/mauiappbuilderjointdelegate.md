# MauiAppBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.4-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the MauiAppBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void MauiAppBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiAppBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)

## Constructors

- [MauiAppBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiAppBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-mauiappbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
