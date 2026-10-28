# HostApplicationAsyncJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Dovetail.Hosting](/Dovetail/v0.2.2/api/dovetail.hosting.md)
- Version: `0.2.2`
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

- [HostApplicationAsyncJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
