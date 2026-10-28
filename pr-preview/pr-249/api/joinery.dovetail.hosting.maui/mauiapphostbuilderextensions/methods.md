# MauiAppHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui.md)
- Type: [MauiAppHostBuilderExtensions](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiapphostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying MauiApp parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuremauiapp"></a>
<a id="configuremauiapp-dovetailcontextbuilder-mauiappjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureMauiApp(DovetailContextBuilder, MauiAppJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureMauiApp(DovetailContextBuilder, MauiAppJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class MauiAppHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureMauiApp(
        this DovetailContextBuilder container,
        MauiAppJointDelegate @delegate,
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
- `delegate` ([MauiAppJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiappjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuremauiapp-dovetailcontextbuilder-mauiappasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureMauiApp(DovetailContextBuilder, MauiAppAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureMauiApp(DovetailContextBuilder, MauiAppAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class MauiAppHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureMauiApp(
        this DovetailContextBuilder container,
        MauiAppAsyncJointDelegate @delegate,
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
- `delegate` ([MauiAppAsyncJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/mauiappasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
