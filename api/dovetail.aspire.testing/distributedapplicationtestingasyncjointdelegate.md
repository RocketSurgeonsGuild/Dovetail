# DistributedApplicationTestingAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire.Testing](/Dovetail/api/dovetail.aspire.testing.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the DistributedApplicationTesting part.

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DistributedApplicationTestingAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.Testing.IDistributedApplicationTestingBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DistributedApplicationTestingAsyncJointDelegate(object, nint)](/Dovetail/api/dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
