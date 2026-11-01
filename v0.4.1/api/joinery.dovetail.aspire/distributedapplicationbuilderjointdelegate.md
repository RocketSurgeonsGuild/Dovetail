# DistributedApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/v0.4.1/api/joinery.dovetail.aspire.md)
- Version: `0.4.1`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [DistributedApplicationBuilderJointDelegate(object, nint)](/Dovetail/v0.4.1/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, AsyncCallback, object)](/Dovetail/v0.4.1/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-idistributedapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.4.1/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
