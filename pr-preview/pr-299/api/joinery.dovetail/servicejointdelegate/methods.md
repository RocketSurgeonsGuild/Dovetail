# ServiceJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.md)
- Type: [ServiceJointDelegate](/Dovetail/pr-preview/pr-299/api/joinery.dovetail/servicejointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Service part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iservicecollection-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IServiceCollection, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IServiceCollection, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ServiceJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IServiceCollection services,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-299/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate ServiceJointDelegate
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
