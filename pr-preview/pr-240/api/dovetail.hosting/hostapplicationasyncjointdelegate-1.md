# HostApplicationAsyncJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-240/api/dovetail.hosting.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the HostApplication part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask HostApplicationAsyncJointDelegate<TBuilder>(
    Dovetail.IDovetailContext context,
    TBuilder builder,
    System.Threading.CancellationToken cancellationToken)
    where TBuilder : IHostApplicationBuilder
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostApplicationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-240/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
