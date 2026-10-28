# HostApplicationJointDelegate<TBuilder> Methods

- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.md)
- Type: [HostApplicationJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-244/api/dovetail.hosting/hostapplicationjointdelegate-1.md)
- Kind: `Methods`
- Members: `2`

Delegate for the HostApplication part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-tbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate HostApplicationJointDelegate<TBuilder>
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        TBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate HostApplicationJointDelegate<TBuilder>
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
