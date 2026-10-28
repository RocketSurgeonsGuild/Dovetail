# DistributedApplicationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplication part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.DistributedApplication host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
