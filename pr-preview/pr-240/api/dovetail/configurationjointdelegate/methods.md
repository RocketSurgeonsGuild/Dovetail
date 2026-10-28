# ConfigurationJointDelegate Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Type: [ConfigurationJointDelegate](/Dovetail/pr-preview/pr-240/api/dovetail/configurationjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Configuration part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iconfigurationbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IConfigurationBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IConfigurationBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ConfigurationJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IConfigurationBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate ConfigurationJointDelegate
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
