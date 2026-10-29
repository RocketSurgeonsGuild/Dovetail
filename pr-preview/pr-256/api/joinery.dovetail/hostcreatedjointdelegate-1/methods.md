# HostCreatedJointDelegate<THost> Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-256/api/joinery.dovetail.md)
- Type: [HostCreatedJointDelegate<THost>](/Dovetail/pr-preview/pr-256/api/joinery.dovetail/hostcreatedjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Delegate for the HostCreated part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-thost-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, THost, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, THost, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostCreatedJointDelegate<THost>
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        THost host,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-256/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate HostCreatedJointDelegate<THost>
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
