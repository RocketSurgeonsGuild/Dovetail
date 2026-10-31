# HostApplicationAsyncJointDelegate<TBuilder> Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-291/api/joinery.dovetail.hosting.md)
- Type: [HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-291/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the HostApplication part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-tbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, TBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, TBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostApplicationAsyncJointDelegate<TBuilder>
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        TBuilder builder,
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
- `builder` (`TBuilder`)
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
public delegate HostApplicationAsyncJointDelegate<TBuilder>
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
