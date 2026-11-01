# ConfigurationAsyncJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.md)
- Type: [ConfigurationAsyncJointDelegate](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/configurationasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Configuration part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iconfigurationbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IConfigurationBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IConfigurationBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ConfigurationAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IConfigurationBuilder builder,
        CancellationToken cancellationToken,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
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
public delegate ConfigurationAsyncJointDelegate
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
