# HostApplicationHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.hosting.md)
- Type: [HostApplicationHostBuilderExtensions](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.hosting/hostapplicationhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying HostApplication parts to a `Dovetail.DovetailContextBuilder`.

<a id="configurehostapplication"></a>
<a id="configurehostapplication-dovetailcontextbuilder-hostapplicationjointdelegate-tbuilder-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHostApplication(DovetailContextBuilder, HostApplicationJointDelegate<TBuilder>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHostApplication(DovetailContextBuilder, HostApplicationJointDelegate<TBuilder>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class HostApplicationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHostApplication<TBuilder>(
        this DovetailContextBuilder container,
        HostApplicationJointDelegate<TBuilder> @delegate,
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
- `delegate` ([HostApplicationJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurehostapplication-dovetailcontextbuilder-hostapplicationasyncjointdelegate-tbuilder-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHostApplication(DovetailContextBuilder, HostApplicationAsyncJointDelegate<TBuilder>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHostApplication(DovetailContextBuilder, HostApplicationAsyncJointDelegate<TBuilder>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class HostApplicationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHostApplication<TBuilder>(
        this DovetailContextBuilder container,
        HostApplicationAsyncJointDelegate<TBuilder> @delegate,
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
- `delegate` ([HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
