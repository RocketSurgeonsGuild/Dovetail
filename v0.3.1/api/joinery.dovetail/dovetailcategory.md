# DovetailCategory

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/v0.3.1/api/joinery.dovetail.md)
- Version: `0.3.1`
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

This is used to load limited sets of conventions based on categories provided.

## Constructors

- [DovetailCategory(string)](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/constructors.md#constructor-string) -- The category of a given convention

## Properties

- [ValueComparer](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/properties.md#valuecomparer) : [IEqualityComparer<DovetailCategory>](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory.md) `static` `get` -- The value comparer for this category

## Methods

- [Equals(object?)](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [GetHashCode](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Implicit(DovetailCategory)](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/methods.md#op-implicit-dovetailcategory) : `string` `static` -- Implicitly convert to a string
- [op_Implicit(string)](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/methods.md#op-implicit-string) : [DovetailCategory](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory.md) `static` -- Implicitly convert from a string
- [ToString](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/methods.md#tostring) : `string` -- Returns a string that represents the current object.

## Fields

- [Application](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/fields.md#application) : `string` `static` -- This convention is loaded for any application that might be starting
- [Core](/Dovetail/v0.3.1/api/joinery.dovetail/dovetailcategory/fields.md#core) : `string` `static` -- This convention is to load for any infrastructure bits (serializer, logging, etc)
