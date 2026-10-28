# DovetailContextBuilderFactory

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.md)
- Version: `0.2.4-pr.6`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcontextbuilder.md).

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed delegate Joinery.Dovetail.DovetailContextBuilder DovetailContextBuilderFactory(
    System.Collections.Generic.IDictionary<object, object>? properties,
    System.Collections.Generic.IEnumerable<Joinery.Dovetail.DovetailCategory>? categories)
```

## Delegate Parameters

- `properties` (`IDictionary<object, object>`) `nullable` `optional` -- The properties to initialize the context builder with.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcategory.md)) `nullable` `optional` -- The categories to initialize the context builder with.

## Constructors

- [DovetailContextBuilderFactory(object, nint)](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcontextbuilderfactory/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#begininvoke-idictionary-object-object-ienumerable-dovetailcategory-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#endinvoke-iasyncresult) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-248/api/joinery.dovetail/dovetailcontextbuilder.md) `virtual`
