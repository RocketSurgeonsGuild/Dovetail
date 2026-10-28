# HostJointDelegate<THost> Methods

- Package: [Dovetail.Hosting](/Dovetail/api/dovetail.hosting.md)
- Type: [HostJointDelegate<THost>](/Dovetail/api/dovetail.hosting/hostjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Host part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-thost-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, THost, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, THost, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostJointDelegate<THost>
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

- `context` (`IDovetailContext`)
- `host` (`THost`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate HostJointDelegate<THost>
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
