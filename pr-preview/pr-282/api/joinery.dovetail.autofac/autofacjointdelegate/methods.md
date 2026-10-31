# AutofacJointDelegate Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.autofac.md)
- Type: [AutofacJointDelegate](/Dovetail/pr-preview/pr-282/api/joinery.dovetail.autofac/autofacjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Autofac part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-containerbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, ContainerBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, ContainerBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate AutofacJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        ContainerBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate AutofacJointDelegate
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
