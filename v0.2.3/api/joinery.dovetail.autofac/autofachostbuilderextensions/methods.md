# AutofacHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/v0.2.3/api/joinery.dovetail.autofac.md)
- Type: [AutofacHostBuilderExtensions](/Dovetail/v0.2.3/api/joinery.dovetail.autofac/autofachostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Autofac parts to a `Dovetail.DovetailContextBuilder`.

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
- `delegate` ([AutofacJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.autofac/autofacjointdelegate.md))
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
- `delegate` ([AutofacAsyncJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.autofac/autofacasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
