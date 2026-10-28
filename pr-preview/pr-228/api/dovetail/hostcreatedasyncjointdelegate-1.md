# HostCreatedAsyncJointDelegate<THost>

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the HostCreated part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask HostCreatedAsyncJointDelegate<THost>(
    Dovetail.IDovetailContext context,
    THost host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailcontext.md))
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostCreatedAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-228/api/dovetail/hostcreatedasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-228/api/dovetail/hostcreatedasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-228/api/dovetail/hostcreatedasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
