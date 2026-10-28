# WebAssemblyHostBuilderHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying WebAssemblyHostBuilder parts to a `DovetailContextBuilder`.

<a id="configurewebassemblyhostbuilder"></a>
<a id="configurewebassemblyhostbuilder-dovetailcontextbuilder-webassemblyhostbuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureWebAssemblyHostBuilder(DovetailContextBuilder, WebAssemblyHostBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureWebAssemblyHostBuilder(DovetailContextBuilder, WebAssemblyHostBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class WebAssemblyHostBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureWebAssemblyHostBuilder(
        this DovetailContextBuilder container,
        WebAssemblyHostBuilderJointDelegate @delegate,
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
- `delegate` ([WebAssemblyHostBuilderJointDelegate](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurewebassemblyhostbuilder-dovetailcontextbuilder-webassemblyhostbuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureWebAssemblyHostBuilder(DovetailContextBuilder, WebAssemblyHostBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureWebAssemblyHostBuilder(DovetailContextBuilder, WebAssemblyHostBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class WebAssemblyHostBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureWebAssemblyHostBuilder(
        this DovetailContextBuilder container,
        WebAssemblyHostBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([WebAssemblyHostBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
