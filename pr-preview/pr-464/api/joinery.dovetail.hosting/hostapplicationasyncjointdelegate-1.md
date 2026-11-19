# HostApplicationAsyncJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/HostApplicationAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the HostApplication part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask HostApplicationAsyncJointDelegate<TBuilder>(
    Joinery.Dovetail.IDovetailContext context,
    TBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostApplicationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
