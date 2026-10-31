# DovetailExceptionPolicyDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-290/api/joinery.dovetail.md)
- Type: [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-290/api/joinery.dovetail/dovetailexceptionpolicydelegate.md)
- Kind: `Methods`
- Members: `2`

A policy delegate that can be used to determine if a given exception should be ignored or not

<a id="begininvoke"></a>
<a id="begininvoke-exception-asynccallback-object"></a>

## BeginInvoke(Exception, AsyncCallback, object)

- Name: `BeginInvoke(Exception, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DovetailExceptionPolicyDelegate
{
    public virtual IAsyncResult BeginInvoke(
        Exception exception,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `exception` (`Exception`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`
- Returns: `bool`

```csharp
public delegate DovetailExceptionPolicyDelegate
{
    public virtual bool EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
