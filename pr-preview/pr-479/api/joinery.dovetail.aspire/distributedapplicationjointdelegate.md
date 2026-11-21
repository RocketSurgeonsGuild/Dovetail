# DistributedApplicationJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationJointDelegate.cs)
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

- [DistributedApplicationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, DistributedApplication, AsyncCallback, object)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/methods.md#begininvoke-idovetailcontext-distributedapplication-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire/distributedapplicationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
