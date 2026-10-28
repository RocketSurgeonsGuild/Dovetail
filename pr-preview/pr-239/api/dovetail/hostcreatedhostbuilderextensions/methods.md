# HostCreatedHostBuilderExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-239/api/dovetail.md)
- Type: [HostCreatedHostBuilderExtensions](/Dovetail/pr-preview/pr-239/api/dovetail/hostcreatedhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying HostCreated parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md).

<a id="configurehostcreated"></a>
<a id="configurehostcreated-dovetailcontextbuilder-hostcreatedjointdelegate-thost-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHostCreated(DovetailContextBuilder, HostCreatedJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHostCreated(DovetailContextBuilder, HostCreatedJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md)

```csharp
public static class HostCreatedHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHostCreated<THost>(
        this DovetailContextBuilder container,
        HostCreatedJointDelegate<THost> @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([HostCreatedJointDelegate<THost>](/Dovetail/pr-preview/pr-239/api/dovetail/hostcreatedjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`

<a id="configurehostcreated-dovetailcontextbuilder-hostcreatedasyncjointdelegate-thost-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHostCreated(DovetailContextBuilder, HostCreatedAsyncJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHostCreated(DovetailContextBuilder, HostCreatedAsyncJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md)

```csharp
public static class HostCreatedHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHostCreated<THost>(
        this DovetailContextBuilder container,
        HostCreatedAsyncJointDelegate<THost> @delegate,
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

- `container` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md))
- `delegate` ([HostCreatedAsyncJointDelegate<THost>](/Dovetail/pr-preview/pr-239/api/dovetail/hostcreatedasyncjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` ([DovetailCategory?](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcategory.md)) `optional`
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailhosttype.md)) `optional`
- `expression` (`string?`) `optional`
