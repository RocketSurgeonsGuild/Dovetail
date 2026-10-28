# WebAssemblyHostBuilderAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the WebAssemblyHostBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-webassemblyhostbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate WebAssemblyHostBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        WebAssemblyHostBuilder builder,
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
- `builder` (`WebAssemblyHostBuilder`)
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
public delegate WebAssemblyHostBuilderAsyncJointDelegate
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
