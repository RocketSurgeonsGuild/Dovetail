# DryIocHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.dryioc.md)
- Type: [DryIocHostBuilderExtensions](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.dryioc/dryiochostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DryIoc parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredryioc"></a>
<a id="configuredryioc-dovetailcontextbuilder-dryiocjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DryIocHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDryIoc(
        this DovetailContextBuilder container,
        DryIocJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` (`DovetailContextBuilder`)
- `delegate` ([DryIocJointDelegate](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.dryioc/dryiocjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredryioc-dovetailcontextbuilder-dryiocasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DryIocHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDryIoc(
        this DovetailContextBuilder container,
        DryIocAsyncJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` (`DovetailContextBuilder`)
- `delegate` ([DryIocAsyncJointDelegate](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
