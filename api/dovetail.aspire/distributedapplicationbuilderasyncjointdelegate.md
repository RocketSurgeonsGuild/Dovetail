# DistributedApplicationBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire](/Dovetail/api/dovetail.aspire.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplicationBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationBuilderAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.IDistributedApplicationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
