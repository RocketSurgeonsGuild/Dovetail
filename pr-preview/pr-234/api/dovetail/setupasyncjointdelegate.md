# SetupAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-234/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Setup part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask SetupAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [SetupAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-234/api/dovetail/setupasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-234/api/dovetail/setupasyncjointdelegate/methods.md#begininvoke-idovetailcontext-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-234/api/dovetail/setupasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
