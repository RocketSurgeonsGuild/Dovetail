# DovetailContextBuilderFactory

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-239/api/dovetail.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md).

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed delegate Dovetail.DovetailContextBuilder DovetailContextBuilderFactory(
    System.Collections.Generic.IDictionary<object, object>? properties,
    System.Collections.Generic.IEnumerable<Dovetail.DovetailCategory>? categories)
```

## Delegate Parameters

- `properties` (`IDictionary<object, object>`) `nullable` `optional` -- The properties to initialize the context builder with.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcategory.md)) `nullable` `optional` -- The categories to initialize the context builder with.

## Constructors

- [DovetailContextBuilderFactory(object, nint)](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilderfactory/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilderfactory/methods.md#begininvoke-idictionary-object-object-ienumerable-dovetailcategory-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilderfactory/methods.md#endinvoke-iasyncresult) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md) `virtual`
