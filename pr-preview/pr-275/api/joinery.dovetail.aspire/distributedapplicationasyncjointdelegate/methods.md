# DistributedApplicationAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-275/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationAsyncJointDelegate](/Dovetail/pr-preview/pr-275/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the DistributedApplication part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-distributedapplication-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, DistributedApplication, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        DistributedApplication host,
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
- `host` (`DistributedApplication`)
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
public delegate DistributedApplicationAsyncJointDelegate
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
