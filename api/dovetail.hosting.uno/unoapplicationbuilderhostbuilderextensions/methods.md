# UnoApplicationBuilderHostBuilderExtensions Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/api/dovetail.hosting.uno.md)
- Type: [UnoApplicationBuilderHostBuilderExtensions](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying UnoApplicationBuilder parts to a `DovetailContextBuilder`.

<a id="configureunoapplicationbuilder"></a>
<a id="configureunoapplicationbuilder-dovetailcontextbuilder-unoapplicationbuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureUnoApplicationBuilder(DovetailContextBuilder, UnoApplicationBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureUnoApplicationBuilder(DovetailContextBuilder, UnoApplicationBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class UnoApplicationBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureUnoApplicationBuilder(
        this DovetailContextBuilder container,
        UnoApplicationBuilderJointDelegate @delegate,
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
- `delegate` ([UnoApplicationBuilderJointDelegate](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureunoapplicationbuilder-dovetailcontextbuilder-unoapplicationbuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureUnoApplicationBuilder(DovetailContextBuilder, UnoApplicationBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureUnoApplicationBuilder(DovetailContextBuilder, UnoApplicationBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class UnoApplicationBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureUnoApplicationBuilder(
        this DovetailContextBuilder container,
        UnoApplicationBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([UnoApplicationBuilderAsyncJointDelegate](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
