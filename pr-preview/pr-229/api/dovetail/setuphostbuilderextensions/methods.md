# SetupHostBuilderExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-229/api/dovetail.md)
- Type: [SetupHostBuilderExtensions](/Dovetail/pr-preview/pr-229/api/dovetail/setuphostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Setup parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcontextbuilder.md).

<a id="configuresetup"></a>
<a id="configuresetup-dovetailcontextbuilder-setupjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureSetup(DovetailContextBuilder, SetupJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureSetup(DovetailContextBuilder, SetupJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcontextbuilder.md)

```csharp
public static class SetupHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureSetup(
        this DovetailContextBuilder container,
        SetupJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([SetupJointDelegate](/Dovetail/pr-preview/pr-229/api/dovetail/setupjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configuresetup-dovetailcontextbuilder-setupasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureSetup(DovetailContextBuilder, SetupAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureSetup(DovetailContextBuilder, SetupAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcontextbuilder.md)

```csharp
public static class SetupHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureSetup(
        this DovetailContextBuilder container,
        SetupAsyncJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([SetupAsyncJointDelegate](/Dovetail/pr-preview/pr-229/api/dovetail/setupasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-229/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
