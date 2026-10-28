# DistributedApplicationTestingJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire.Testing](/Dovetail/v0.2.2/api/dovetail.aspire.testing.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplicationTesting part.

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public sealed delegate void DistributedApplicationTestingJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.Testing.IDistributedApplicationTestingBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)

## Constructors

- [DistributedApplicationTestingJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.aspire.testing/distributedapplicationtestingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
