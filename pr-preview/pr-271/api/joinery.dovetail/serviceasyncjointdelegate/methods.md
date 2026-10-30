# ServiceAsyncJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.md)
- Type: [ServiceAsyncJointDelegate](/Dovetail/pr-preview/pr-271/api/joinery.dovetail/serviceasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Service part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ServiceAsyncJointDelegate
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-271/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`
- Returns: `ValueTask`

```csharp
public delegate ServiceAsyncJointDelegate
{
    public virtual ValueTask EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
