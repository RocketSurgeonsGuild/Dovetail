# UnoHostBuilderHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Type: [UnoHostBuilderHostBuilderExtensions](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying UnoHostBuilder parts to a `Dovetail.DovetailContextBuilder`.

<a id="configureunohostbuilder"></a>
<a id="configureunohostbuilder-dovetailcontextbuilder-unohostbuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureUnoHostBuilder(DovetailContextBuilder, UnoHostBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureUnoHostBuilder(DovetailContextBuilder, UnoHostBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class UnoHostBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureUnoHostBuilder(
        this DovetailContextBuilder container,
        UnoHostBuilderJointDelegate @delegate,
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
- `delegate` ([UnoHostBuilderJointDelegate](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureunohostbuilder-dovetailcontextbuilder-unohostbuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureUnoHostBuilder(DovetailContextBuilder, UnoHostBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureUnoHostBuilder(DovetailContextBuilder, UnoHostBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class UnoHostBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureUnoHostBuilder(
        this DovetailContextBuilder container,
        UnoHostBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([UnoHostBuilderAsyncJointDelegate](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
