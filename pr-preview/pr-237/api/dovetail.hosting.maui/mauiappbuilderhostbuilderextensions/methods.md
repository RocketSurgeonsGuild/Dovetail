# MauiAppBuilderHostBuilderExtensions Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui.md)
- Type: [MauiAppBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui/mauiappbuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying MauiAppBuilder parts to a `DovetailContextBuilder`.

<a id="configuremauiappbuilder"></a>
<a id="configuremauiappbuilder-dovetailcontextbuilder-mauiappbuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureMauiAppBuilder(DovetailContextBuilder, MauiAppBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureMauiAppBuilder(DovetailContextBuilder, MauiAppBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class MauiAppBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureMauiAppBuilder(
        this DovetailContextBuilder container,
        MauiAppBuilderJointDelegate @delegate,
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
- `delegate` ([MauiAppBuilderJointDelegate](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui/mauiappbuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuremauiappbuilder-dovetailcontextbuilder-mauiappbuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureMauiAppBuilder(DovetailContextBuilder, MauiAppBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureMauiAppBuilder(DovetailContextBuilder, MauiAppBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class MauiAppBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureMauiAppBuilder(
        this DovetailContextBuilder container,
        MauiAppBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([MauiAppBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui/mauiappbuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
