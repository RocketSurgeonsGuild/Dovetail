# UnoApplicationBuilderJointDelegate Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno.md)
- Type: [UnoApplicationBuilderJointDelegate](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno/unoapplicationbuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the UnoApplicationBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iapplicationbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IApplicationBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IApplicationBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate UnoApplicationBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IApplicationBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate UnoApplicationBuilderJointDelegate
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
