# HostHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.md)
- Type: [HostHostBuilderExtensions](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting/hosthostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Host parts to a `Dovetail.DovetailContextBuilder`.

<a id="configurehost"></a>
<a id="configurehost-dovetailcontextbuilder-hostjointdelegate-thost-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHost(DovetailContextBuilder, HostJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHost(DovetailContextBuilder, HostJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class HostHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHost<THost>(
        this DovetailContextBuilder container,
        HostJointDelegate<THost> @delegate,
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
- `delegate` ([HostJointDelegate<THost>](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting/hostjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurehost-dovetailcontextbuilder-hostasyncjointdelegate-thost-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureHost(DovetailContextBuilder, HostAsyncJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureHost(DovetailContextBuilder, HostAsyncJointDelegate<THost>, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class HostHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureHost<THost>(
        this DovetailContextBuilder container,
        HostAsyncJointDelegate<THost> @delegate,
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
- `delegate` ([HostAsyncJointDelegate<THost>](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting/hostasyncjointdelegate-1.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
