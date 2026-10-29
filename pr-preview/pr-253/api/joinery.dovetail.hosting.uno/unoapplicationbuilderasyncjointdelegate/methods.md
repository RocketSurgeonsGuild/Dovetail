# UnoApplicationBuilderAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno.md)
- Type: [UnoApplicationBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the UnoApplicationBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iapplicationbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IApplicationBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IApplicationBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate UnoApplicationBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IApplicationBuilder builder,
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
- `builder` (`IApplicationBuilder`)
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
public delegate UnoApplicationBuilderAsyncJointDelegate
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
