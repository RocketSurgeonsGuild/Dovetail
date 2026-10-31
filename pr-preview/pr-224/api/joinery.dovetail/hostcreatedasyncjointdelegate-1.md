# HostCreatedAsyncJointDelegate<THost>

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.md)
- Version: `0.3.2-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [HostCreatedAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/hostcreatedasyncjointdelegate-1/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
