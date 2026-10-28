# WebAssemblyHostBuilderJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostBuilderJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the WebAssemblyHostBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-webassemblyhostbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate WebAssemblyHostBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        WebAssemblyHostBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate WebAssemblyHostBuilderJointDelegate
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
