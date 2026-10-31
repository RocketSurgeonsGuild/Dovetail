# AutofacAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.autofac.md)
- Type: [AutofacAsyncJointDelegate](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.autofac/autofacasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Autofac part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-containerbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, ContainerBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, ContainerBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate AutofacAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        ContainerBuilder builder,
        CancellationToken cancellationToken,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)
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
public delegate AutofacAsyncJointDelegate
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
