# DryIocHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc.md)
- Type: [DryIocHostBuilderExtensions](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc/dryiochostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DryIoc parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredryioc"></a>
<a id="configuredryioc-dovetailcontextbuilder-dryiocjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs)

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
- `delegate` ([DryIocJointDelegate](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc/dryiocjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredryioc-dovetailcontextbuilder-dryiocasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs)

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
- `delegate` ([DryIocAsyncJointDelegate](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
