# DovetailContextBuilderFactory Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.md)
- Type: [DovetailContextBuilderFactory](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/dovetailcontextbuilderfactory.md)
- Kind: `Methods`
- Members: `2`

A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="begininvoke"></a>
<a id="begininvoke-idictionary-object-object-ienumerable-dovetailcategory-asynccallback-object"></a>

## BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)

- Name: `BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate DovetailContextBuilderFactory
{
    public virtual IAsyncResult BeginInvoke(
        IDictionary<object, object>? properties,
        IEnumerable<DovetailCategory>? categories,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `properties` (`IDictionary<object, object>`)
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/dovetailcategory.md))
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/dovetailcontextbuilder.md)

```csharp
public delegate DovetailContextBuilderFactory
{
    public virtual DovetailContextBuilder EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
