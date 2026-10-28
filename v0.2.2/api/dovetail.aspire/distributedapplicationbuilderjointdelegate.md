# DistributedApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Aspire](/Dovetail/v0.2.2/api/dovetail.aspire.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DistributedApplicationBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void DistributedApplicationBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    Aspire.Hosting.IDistributedApplicationBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)

## Constructors

- [DistributedApplicationBuilderJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.aspire/distributedapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
