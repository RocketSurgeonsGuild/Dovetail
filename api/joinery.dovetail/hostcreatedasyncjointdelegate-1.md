# HostCreatedAsyncJointDelegate<THost>

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/obj/Release/net10.0/HostCreatedAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the HostCreated part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask HostCreatedAsyncJointDelegate<THost>(
    Joinery.Dovetail.IDovetailContext context,
    THost host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostCreatedAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
