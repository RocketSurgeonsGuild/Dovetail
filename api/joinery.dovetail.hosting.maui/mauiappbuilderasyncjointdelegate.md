# MauiAppBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/api/joinery.dovetail.hosting.maui.md)
- Version: `0.2.4-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the MauiAppBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask MauiAppBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiAppBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [MauiAppBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiAppBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-mauiappbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
