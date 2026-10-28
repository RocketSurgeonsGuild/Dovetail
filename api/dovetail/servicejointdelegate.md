# ServiceJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.1`
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

- `context` ([IDovetailContext](/Dovetail/api/dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)

## Constructors

- [ServiceJointDelegate(object, nint)](/Dovetail/api/dovetail/servicejointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, AsyncCallback, object)](/Dovetail/api/dovetail/servicejointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail/servicejointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
