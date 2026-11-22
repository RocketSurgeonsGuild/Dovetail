# DistributedApplicationBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-beta.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationBuilderAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplicationBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.IDistributedApplicationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
