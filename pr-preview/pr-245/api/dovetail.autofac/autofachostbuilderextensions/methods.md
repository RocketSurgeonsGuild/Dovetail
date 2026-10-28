# AutofacHostBuilderExtensions Methods

- Package: [Dovetail.Autofac](/Dovetail/pr-preview/pr-245/api/dovetail.autofac.md)
- Type: [AutofacHostBuilderExtensions](/Dovetail/pr-preview/pr-245/api/dovetail.autofac/autofachostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Autofac parts to a `DovetailContextBuilder`.

<a id="configureautofac"></a>
<a id="configureautofac-dovetailcontextbuilder-autofacjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureAutofac(DovetailContextBuilder, AutofacJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureAutofac(DovetailContextBuilder, AutofacJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class AutofacHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureAutofac(
        this DovetailContextBuilder container,
        AutofacJointDelegate @delegate,
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
- `delegate` ([AutofacJointDelegate](/Dovetail/pr-preview/pr-245/api/dovetail.autofac/autofacjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureautofac-dovetailcontextbuilder-autofacasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureAutofac(DovetailContextBuilder, AutofacAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureAutofac(DovetailContextBuilder, AutofacAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class AutofacHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureAutofac(
        this DovetailContextBuilder container,
        AutofacAsyncJointDelegate @delegate,
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
- `delegate` ([AutofacAsyncJointDelegate](/Dovetail/pr-preview/pr-245/api/dovetail.autofac/autofacasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
