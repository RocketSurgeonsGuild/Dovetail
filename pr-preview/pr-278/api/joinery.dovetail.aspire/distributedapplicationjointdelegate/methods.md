# DistributedApplicationJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationJointDelegate](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.aspire/distributedapplicationjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the DistributedApplication part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-distributedapplication-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, DistributedApplication, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, DistributedApplication, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        DistributedApplication host,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate DistributedApplicationJointDelegate
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
