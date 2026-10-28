# ServiceProviderFactoryAdapter

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

A factory that provides a service provider factory from the convention context

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed delegate System.Threading.Tasks.ValueTask<Microsoft.Extensions.DependencyInjection.IServiceProviderFactory<object>> ServiceProviderFactoryAdapter(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-236/api/dovetail/idovetailcontext.md)) -- The convention context
- `services` (`IServiceCollection`) -- The service collection
- `cancellationToken` (`CancellationToken`) -- The cancellation token

## Constructors

- [ServiceProviderFactoryAdapter(object, nint)](/Dovetail/pr-preview/pr-236/api/dovetail/serviceproviderfactoryadapter/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-236/api/dovetail/serviceproviderfactoryadapter/methods.md#begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-236/api/dovetail/serviceproviderfactoryadapter/methods.md#endinvoke-iasyncresult) : `ValueTask<IServiceProviderFactory<object>>` `virtual`
