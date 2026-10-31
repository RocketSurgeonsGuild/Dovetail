# TestingAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.testing.md)
- Type: [TestingAsyncJointDelegate](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.testing/testingasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Testing part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate TestingAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
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
public delegate TestingAsyncJointDelegate
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
