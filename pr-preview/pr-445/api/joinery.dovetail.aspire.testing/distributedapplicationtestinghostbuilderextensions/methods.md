# DistributedApplicationTestingHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingHostBuilderExtensions](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.aspire.testing/distributedapplicationtestinghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DistributedApplicationTesting parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredistributedapplicationtesting"></a>
<a id="configuredistributedapplicationtesting-dovetailcontextbuilder-distributedapplicationtestingjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplicationTesting(DovetailContextBuilder, DistributedApplicationTestingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplicationTesting(DovetailContextBuilder, DistributedApplicationTestingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail.Aspire.Testing/obj/Release/net10.0/DovetailJoint.DistributedApplicationTesting.g.cs)

```csharp
public static class DistributedApplicationTestingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplicationTesting(
        this DovetailContextBuilder container,
        DistributedApplicationTestingJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationTestingJointDelegate](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredistributedapplicationtesting-dovetailcontextbuilder-distributedapplicationtestingasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplicationTesting(DovetailContextBuilder, DistributedApplicationTestingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplicationTesting(DovetailContextBuilder, DistributedApplicationTestingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail.Aspire.Testing/obj/Release/net10.0/DovetailJoint.DistributedApplicationTesting.g.cs)

```csharp
public static class DistributedApplicationTestingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplicationTesting(
        this DovetailContextBuilder container,
        DistributedApplicationTestingAsyncJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationTestingAsyncJointDelegate](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.aspire.testing/distributedapplicationtestingasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
