# DistributedApplicationHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationHostBuilderExtensions](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.aspire/distributedapplicationhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DistributedApplication parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredistributedapplication"></a>
<a id="configuredistributedapplication-dovetailcontextbuilder-distributedapplicationjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplication(DovetailContextBuilder, DistributedApplicationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplication(DovetailContextBuilder, DistributedApplicationJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DistributedApplicationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplication(
        this DovetailContextBuilder container,
        DistributedApplicationJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationJointDelegate](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.aspire/distributedapplicationjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredistributedapplication-dovetailcontextbuilder-distributedapplicationasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplication(DovetailContextBuilder, DistributedApplicationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplication(DovetailContextBuilder, DistributedApplicationAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DistributedApplicationHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplication(
        this DovetailContextBuilder container,
        DistributedApplicationAsyncJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationAsyncJointDelegate](/Dovetail/pr-preview/pr-248/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
