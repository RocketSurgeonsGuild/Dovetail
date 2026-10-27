# UnoHostBuilderAsyncJointDelegate Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.uno.md)
- Type: [UnoHostBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.uno/unohostbuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the UnoHostBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-ihostbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IHostBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IHostBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate UnoHostBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IHostBuilder builder,
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
- `builder` (`IHostBuilder`)
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
public delegate UnoHostBuilderAsyncJointDelegate
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
