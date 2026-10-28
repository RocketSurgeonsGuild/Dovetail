# WebAssemblyHostAsyncJointDelegate Methods

- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostAsyncJointDelegate](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the WebAssemblyHost part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-webassemblyhost-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, WebAssemblyHost, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, WebAssemblyHost, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate WebAssemblyHostAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        WebAssemblyHost host,
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
- `host` (`WebAssemblyHost`)
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
public delegate WebAssemblyHostAsyncJointDelegate
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
