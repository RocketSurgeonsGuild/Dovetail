# ServiceProviderFactoryAdapter

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/Infrastructure/ServiceProviderFactoryAdapter.cs)
- Inherits: `MulticastDelegate`

A factory that provides a service provider factory from the convention context

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed delegate System.Threading.Tasks.ValueTask<Microsoft.Extensions.DependencyInjection.IServiceProviderFactory<object>> ServiceProviderFactoryAdapter(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext.md)) -- The convention context
- `services` (`IServiceCollection`) -- The service collection
- `cancellationToken` (`CancellationToken`) -- The cancellation token

## Constructors

- [ServiceProviderFactoryAdapter(object, nint)](/Dovetail/v0.8.0/api/joinery.dovetail/serviceproviderfactoryadapter/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)](/Dovetail/v0.8.0/api/joinery.dovetail/serviceproviderfactoryadapter/methods.md#begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.8.0/api/joinery.dovetail/serviceproviderfactoryadapter/methods.md#endinvoke-iasyncresult) : `ValueTask<IServiceProviderFactory<object>>` `virtual`
