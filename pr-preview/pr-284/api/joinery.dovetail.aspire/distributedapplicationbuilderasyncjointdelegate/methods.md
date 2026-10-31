# DistributedApplicationBuilderAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the DistributedApplicationBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-idistributedapplicationbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IDistributedApplicationBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IDistributedApplicationBuilder builder,
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
- `builder` (`IDistributedApplicationBuilder`)
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
public delegate DistributedApplicationBuilderAsyncJointDelegate
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
