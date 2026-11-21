# AutofacHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.autofac.md)
- Type: [AutofacHostBuilderExtensions](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.autofac/autofachostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Autofac parts to a `Dovetail.DovetailContextBuilder`.

<a id="configureautofac"></a>
<a id="configureautofac-dovetailcontextbuilder-autofacjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureAutofac(DovetailContextBuilder, AutofacJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureAutofac(DovetailContextBuilder, AutofacJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-481-merge.Sha.155deb7a5941f87329ac1f5cf6b4b18b74b0ce52/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/DovetailJoint.Autofac.g.cs)

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
- `delegate` ([AutofacJointDelegate](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.autofac/autofacjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureautofac-dovetailcontextbuilder-autofacasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureAutofac(DovetailContextBuilder, AutofacAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureAutofac(DovetailContextBuilder, AutofacAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-481-merge.Sha.155deb7a5941f87329ac1f5cf6b4b18b74b0ce52/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/DovetailJoint.Autofac.g.cs)

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
- `delegate` ([AutofacAsyncJointDelegate](/Dovetail/pr-preview/pr-481/api/joinery.dovetail.autofac/autofacasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
