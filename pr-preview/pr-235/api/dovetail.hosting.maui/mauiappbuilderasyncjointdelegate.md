# MauiAppBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the MauiAppBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask MauiAppBuilderAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Maui.Hosting.MauiAppBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [MauiAppBuilderAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MauiAppBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-mauiappbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui/mauiappbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
