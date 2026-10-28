# HostAsyncJointDelegate<THost>

- Kind: `delegate`
- Package: [Dovetail.Hosting](/Dovetail/v0.2.2/api/dovetail.hosting.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Host part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask HostAsyncJointDelegate<THost>(
    Dovetail.IDovetailContext context,
    THost host,
    System.Threading.CancellationToken cancellationToken)
    where THost : IHost
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostAsyncJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.hosting/hostasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.hosting/hostasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.hosting/hostasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
