# DistributedApplicationBuilderJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-254/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationBuilderJointDelegate](/Dovetail/pr-preview/pr-254/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the DistributedApplicationBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-idistributedapplicationbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IDistributedApplicationBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate DistributedApplicationBuilderJointDelegate
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
