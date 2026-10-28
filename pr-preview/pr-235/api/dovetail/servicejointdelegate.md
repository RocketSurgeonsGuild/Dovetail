# ServiceJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Service part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void ServiceJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)

## Constructors

- [ServiceJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail/servicejointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail/servicejointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail/servicejointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
