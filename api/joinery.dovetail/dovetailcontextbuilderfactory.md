# DovetailContextBuilderFactory

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.2.4-beta.14`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md).

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed delegate Joinery.Dovetail.DovetailContextBuilder DovetailContextBuilderFactory(
    System.Collections.Generic.IDictionary<object, object>? properties,
    System.Collections.Generic.IEnumerable<Joinery.Dovetail.DovetailCategory>? categories)
```

## Delegate Parameters

- `properties` (`IDictionary<object, object>`) `nullable` `optional` -- The properties to initialize the context builder with.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/api/joinery.dovetail/dovetailcategory.md)) `nullable` `optional` -- The categories to initialize the context builder with.

## Constructors

- [DovetailContextBuilderFactory(object, nint)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilderfactory/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#begininvoke-idictionary-object-object-ienumerable-dovetailcategory-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#endinvoke-iasyncresult) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `virtual`
