# DistributedApplicationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/v0.8.0/api/joinery.dovetail.aspire.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationAsyncJointDelegate.cs)
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

- [DistributedApplicationAsyncJointDelegate(object, nint)](/Dovetail/v0.8.0/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)](/Dovetail/v0.8.0/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.8.0/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
