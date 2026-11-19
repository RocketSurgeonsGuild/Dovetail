# ServiceProviderFactoryAdapter

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.1-beta.1`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.a8ddd6e26ab68fe875ba7298722ca7d8e8d14c24/src/Joinery.Dovetail/Infrastructure/ServiceProviderFactoryAdapter.cs)
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

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)) -- The convention context
- `services` (`IServiceCollection`) -- The service collection
- `cancellationToken` (`CancellationToken`) -- The cancellation token

## Constructors

- [ServiceProviderFactoryAdapter(object, nint)](/Dovetail/api/joinery.dovetail/serviceproviderfactoryadapter/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/serviceproviderfactoryadapter/methods.md#begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/serviceproviderfactoryadapter/methods.md#endinvoke-iasyncresult) : `ValueTask<IServiceProviderFactory<object>>` `virtual`
