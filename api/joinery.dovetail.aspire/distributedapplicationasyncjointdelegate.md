# DistributedApplicationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationAsyncJointDelegate.cs)
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

- [DistributedApplicationAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
