# DistributedApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.7.2-beta.16`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DistributedApplicationBuilderJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplicationBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void DistributedApplicationBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Aspire.Hosting.IDistributedApplicationBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)

## Constructors

- [DistributedApplicationBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
