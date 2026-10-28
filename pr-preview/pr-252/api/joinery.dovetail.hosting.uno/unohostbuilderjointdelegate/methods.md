# UnoHostBuilderJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno.md)
- Type: [UnoHostBuilderJointDelegate](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the UnoHostBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-ihostbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IHostBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IHostBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate UnoHostBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IHostBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate UnoHostBuilderJointDelegate
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
