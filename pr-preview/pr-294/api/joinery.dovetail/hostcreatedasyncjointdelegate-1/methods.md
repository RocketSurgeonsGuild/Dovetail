# HostCreatedAsyncJointDelegate<THost> Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.md)
- Type: [HostCreatedAsyncJointDelegate<THost>](/Dovetail/pr-preview/pr-294/api/joinery.dovetail/hostcreatedasyncjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the HostCreated part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-thost-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, THost, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostCreatedAsyncJointDelegate<THost>
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-294/api/joinery.dovetail/idovetailcontext.md))
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
public delegate HostCreatedAsyncJointDelegate<THost>
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
