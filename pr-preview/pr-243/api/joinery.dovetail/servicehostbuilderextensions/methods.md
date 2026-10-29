# ServiceHostBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-243/api/joinery.dovetail.md)
- Type: [ServiceHostBuilderExtensions](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/servicehostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Service parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="configureservices"></a>
<a id="configureservices-dovetailcontextbuilder-servicejointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureServices(DovetailContextBuilder, ServiceJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureServices(DovetailContextBuilder, ServiceJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcontextbuilder.md)

```csharp
public static class ServiceHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureServices(
        this DovetailContextBuilder container,
        ServiceJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ServiceJointDelegate](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/servicejointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configureservices-dovetailcontextbuilder-serviceasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureServices(DovetailContextBuilder, ServiceAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureServices(DovetailContextBuilder, ServiceAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcontextbuilder.md)

```csharp
public static class ServiceHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureServices(
        this DovetailContextBuilder container,
        ServiceAsyncJointDelegate @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcontextbuilder.md))
- `delegate` ([ServiceAsyncJointDelegate](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/serviceasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
