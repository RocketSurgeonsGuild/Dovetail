# DistributedApplicationTestingAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingAsyncJointDelegate](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the DistributedApplicationTesting part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationTestingAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IDistributedApplicationTestingBuilder builder,
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
- `builder` (`IDistributedApplicationTestingBuilder`)
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
public delegate DistributedApplicationTestingAsyncJointDelegate
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
