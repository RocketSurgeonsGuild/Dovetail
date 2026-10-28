# WebAssemblyHostJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostJointDelegate](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.webassembly/webassemblyhostjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the WebAssemblyHost part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-webassemblyhost-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, WebAssemblyHost, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, WebAssemblyHost, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate WebAssemblyHostJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        WebAssemblyHost host,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate WebAssemblyHostJointDelegate
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
