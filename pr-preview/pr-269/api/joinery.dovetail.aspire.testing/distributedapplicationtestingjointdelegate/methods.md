# DistributedApplicationTestingJointDelegate Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingJointDelegate](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the DistributedApplicationTesting part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-idistributedapplicationtestingbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IDistributedApplicationTestingBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DistributedApplicationTestingJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IDistributedApplicationTestingBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate DistributedApplicationTestingJointDelegate
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
