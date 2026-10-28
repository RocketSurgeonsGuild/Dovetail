# DistributedApplicationTestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplicationTesting part.

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public sealed delegate void DistributedApplicationTestingJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.Testing.IDistributedApplicationTestingBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)

## Constructors

- [DistributedApplicationTestingJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
