# DryIocAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/v0.4.1/api/joinery.dovetail.dryioc.md)
- Type: [DryIocAsyncJointDelegate](/Dovetail/v0.4.1/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the DryIoc part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-icontainer-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DryIocAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IContainer container,
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
- `container` (`IContainer`)
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
public delegate DryIocAsyncJointDelegate
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
