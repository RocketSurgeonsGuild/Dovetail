# DistributedApplicationTestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.Aspire.Testing/DistributedApplicationTestingJointDelegate.cs)
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

- [DistributedApplicationTestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
