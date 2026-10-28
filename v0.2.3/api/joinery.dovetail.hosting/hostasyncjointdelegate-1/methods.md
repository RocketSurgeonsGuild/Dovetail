# HostAsyncJointDelegate<THost> Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.md)
- Type: [HostAsyncJointDelegate<THost>](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/hostasyncjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Host part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostAsyncJointDelegate<THost>
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        THost host,
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
- `host` (`THost`)
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
public delegate HostAsyncJointDelegate<THost>
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
