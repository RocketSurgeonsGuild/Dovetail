# DistributedApplicationJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire.md)
- Version: `0.3.2-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplication part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void DistributedApplicationJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.DistributedApplication host)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)

## Constructors

- [DistributedApplicationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, AsyncCallback, object)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
