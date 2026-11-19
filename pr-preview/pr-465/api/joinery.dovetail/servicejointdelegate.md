# ServiceJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/obj/Release/net10.0/ServiceJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Service part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void ServiceJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)

## Constructors

- [ServiceJointDelegate(object, nint)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/servicejointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, AsyncCallback, object)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/servicejointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/servicejointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
