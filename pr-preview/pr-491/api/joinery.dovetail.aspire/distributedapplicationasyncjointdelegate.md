# DistributedApplicationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationAsyncJointDelegate.cs)
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

- [DistributedApplicationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
