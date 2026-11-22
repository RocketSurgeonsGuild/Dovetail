# DovetailContextBuilderFactory

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.md)
- Version: `0.8.2-pr.10`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail/Infrastructure/DovetailContextBuilderFactory.cs)
- Inherits: `MulticastDelegate`

A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcontextbuilder.md).

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed delegate Joinery.Dovetail.DovetailContextBuilder DovetailContextBuilderFactory(
    System.Collections.Generic.IDictionary<object, object>? properties,
    System.Collections.Generic.IEnumerable<Joinery.Dovetail.DovetailCategory>? categories)
```

## Delegate Parameters

- `properties` (`IDictionary<object, object>`) `nullable` `optional` -- The properties to initialize the context builder with.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcategory.md)) `nullable` `optional` -- The categories of joints the context loads besides the `Core` joints, which are always loaded. `null` or an empty set loads the `Application` joints as well.

## Constructors

- [DovetailContextBuilderFactory(object, nint)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcontextbuilderfactory/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDictionary<object, object>, IEnumerable<DovetailCategory>, AsyncCallback, object)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#begininvoke-idictionary-object-object-ienumerable-dovetailcategory-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcontextbuilderfactory/methods.md#endinvoke-iasyncresult) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailcontextbuilder.md) `virtual`
