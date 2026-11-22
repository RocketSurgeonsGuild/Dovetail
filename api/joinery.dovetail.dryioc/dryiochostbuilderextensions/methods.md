# DryIocHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/api/joinery.dovetail.dryioc.md)
- Type: [DryIocHostBuilderExtensions](/Dovetail/api/joinery.dovetail.dryioc/dryiochostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DryIoc parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredryioc"></a>
<a id="configuredryioc-dovetailcontextbuilder-dryiocjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs)

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
- `delegate` ([DryIocJointDelegate](/Dovetail/api/joinery.dovetail.dryioc/dryiocjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredryioc-dovetailcontextbuilder-dryiocasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDryIoc(DovetailContextBuilder, DryIocAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs)

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
- `delegate` ([DryIocAsyncJointDelegate](/Dovetail/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
