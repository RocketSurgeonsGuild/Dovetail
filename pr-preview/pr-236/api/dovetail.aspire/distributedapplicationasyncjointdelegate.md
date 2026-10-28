# DistributedApplicationAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-236/api/dovetail.aspire.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplication part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.DistributedApplication host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-236/api/dovetail.aspire/distributedapplicationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-236/api/dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-236/api/dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
