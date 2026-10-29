# TestingJointDelegate Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-260/api/joinery.dovetail.testing.md)
- Type: [TestingJointDelegate](/Dovetail/pr-preview/pr-260/api/joinery.dovetail.testing/testingjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Testing part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate TestingJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate TestingJointDelegate
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
