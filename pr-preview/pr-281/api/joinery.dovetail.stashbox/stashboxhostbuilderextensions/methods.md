# StashboxHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.stashbox.md)
- Type: [StashboxHostBuilderExtensions](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.stashbox/stashboxhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Stashbox parts to a `Dovetail.DovetailContextBuilder`.

<a id="configurestashbox"></a>
<a id="configurestashbox-dovetailcontextbuilder-stashboxjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureStashbox(DovetailContextBuilder, StashboxJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureStashbox(DovetailContextBuilder, StashboxJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class StashboxHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureStashbox(
        this DovetailContextBuilder container,
        StashboxJointDelegate @delegate,
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
- `delegate` ([StashboxJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.stashbox/stashboxjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurestashbox-dovetailcontextbuilder-stashboxasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureStashbox(DovetailContextBuilder, StashboxAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureStashbox(DovetailContextBuilder, StashboxAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class StashboxHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureStashbox(
        this DovetailContextBuilder container,
        StashboxAsyncJointDelegate @delegate,
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
- `delegate` ([StashboxAsyncJointDelegate](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
