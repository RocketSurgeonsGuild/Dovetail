# DistributedApplicationTestingAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail.Aspire.Testing/DistributedApplicationTestingAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplicationTesting part.

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationTestingAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.Testing.IDistributedApplicationTestingBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationTestingAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
