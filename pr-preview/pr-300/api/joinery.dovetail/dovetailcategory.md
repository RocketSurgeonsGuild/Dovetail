# DovetailCategory

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Version: `0.4.2-pr.11`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

The category of a given convention

## Definition

```csharp
namespace Joinery.Dovetail;

public sealed class DovetailCategory
{
    // ...
}
```

## Remarks

A set of categories given to [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) chooses which joints the context loads: the joints of those categories and the [DovetailCategory.Core](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/fields.md#core) joints, which are always loaded. An empty set loads the [DovetailCategory.Application](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/fields.md#application) joints and the [DovetailCategory.Core](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/fields.md#core) joints.

## Constructors

- [DovetailCategory(string)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/constructors.md#constructor-string) -- The category of a given convention

## Properties

- [ValueComparer](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/properties.md#valuecomparer) : [IEqualityComparer<DovetailCategory>](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory.md) `static` `get` -- The value comparer for this category

## Methods

- [Equals(object?)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [GetHashCode](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Implicit(DovetailCategory)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/methods.md#op-implicit-dovetailcategory) : `string` `static` -- Implicitly convert to a string
- [op_Implicit(string)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/methods.md#op-implicit-string) : [DovetailCategory](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory.md) `static` -- Implicitly convert from a string
- [ToString](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/methods.md#tostring) : `string` -- Returns a string that represents the current object.

## Fields

- [Application](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/fields.md#application) : `string` `static` -- This convention is loaded for any application that might be starting
- [Core](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory/fields.md#core) : `string` `static` -- This convention is to load for any infrastructure bits (serializer, logging, etc)
