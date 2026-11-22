# DovetailCategory Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.md)
- Type: [DovetailCategory](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory.md)
- Kind: `Constructors`
- Members: `1`

The category of a given convention

<a id="constructor"></a>
<a id="constructor-string"></a>

## DovetailCategory(string)

- Name: `Constructor(string)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/DovetailCategory.cs#L14-L64)

The category of a given convention

```csharp
public sealed class DovetailCategory
{
    public DovetailCategory(
        string name)
    {
        // ...
    }
}
```

## Parameters

- `name` (`string`)

## Remarks

A set of categories given to [DovetailContextBuilder](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcontextbuilder.md) chooses which joints the context loads: the joints of those categories and the [DovetailCategory.Core](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory/fields.md#core) joints, which are always loaded. An empty set loads the [DovetailCategory.Application](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory/fields.md#application) joints and the [DovetailCategory.Core](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory/fields.md#core) joints.
