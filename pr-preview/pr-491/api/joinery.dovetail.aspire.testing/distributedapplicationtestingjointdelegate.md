# DistributedApplicationTestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.Aspire.Testing/DistributedApplicationTestingJointDelegate.cs)
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

- [DistributedApplicationTestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
