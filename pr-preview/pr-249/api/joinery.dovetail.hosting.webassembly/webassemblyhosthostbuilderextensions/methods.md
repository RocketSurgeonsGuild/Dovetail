# WebAssemblyHostHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostHostBuilderExtensions](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/webassemblyhosthostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying WebAssemblyHost parts to a `Dovetail.DovetailContextBuilder`.

<a id="configurewebassemblyhost"></a>
<a id="configurewebassemblyhost-dovetailcontextbuilder-webassemblyhostjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureWebAssemblyHost(DovetailContextBuilder, WebAssemblyHostJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureWebAssemblyHost(DovetailContextBuilder, WebAssemblyHostJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class WebAssemblyHostHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureWebAssemblyHost(
        this DovetailContextBuilder container,
        WebAssemblyHostJointDelegate @delegate,
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
- `delegate` ([WebAssemblyHostJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/webassemblyhostjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurewebassemblyhost-dovetailcontextbuilder-webassemblyhostasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureWebAssemblyHost(DovetailContextBuilder, WebAssemblyHostAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureWebAssemblyHost(DovetailContextBuilder, WebAssemblyHostAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class WebAssemblyHostHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureWebAssemblyHost(
        this DovetailContextBuilder container,
        WebAssemblyHostAsyncJointDelegate @delegate,
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
- `delegate` ([WebAssemblyHostAsyncJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
