# ServiceProviderFactoryAdapter Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.md)
- Type: [ServiceProviderFactoryAdapter](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/serviceproviderfactoryadapter.md)
- Kind: `Methods`
- Members: `2`

A factory that provides a service provider factory from the convention context

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ServiceProviderFactoryAdapter
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IServiceCollection services,
        CancellationToken cancellationToken,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`
- Returns: `ValueTask<IServiceProviderFactory<object>>`

```csharp
public delegate ServiceProviderFactoryAdapter
{
    public virtual ValueTask<IServiceProviderFactory<object>> EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
