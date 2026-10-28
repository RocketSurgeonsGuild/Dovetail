# DistributedApplicationJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-235/api/dovetail.aspire.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplication part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void DistributedApplicationJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.DistributedApplication host)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)

## Constructors

- [DistributedApplicationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.aspire/distributedapplicationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.aspire/distributedapplicationjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.aspire/distributedapplicationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
