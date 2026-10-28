# DryIocJointDelegate Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-251/api/joinery.dovetail.dryioc.md)
- Type: [DryIocJointDelegate](/Dovetail/pr-preview/pr-251/api/joinery.dovetail.dryioc/dryiocjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the DryIoc part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-icontainer-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DryIocJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IContainer container,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate DryIocJointDelegate
{
    public virtual void EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
