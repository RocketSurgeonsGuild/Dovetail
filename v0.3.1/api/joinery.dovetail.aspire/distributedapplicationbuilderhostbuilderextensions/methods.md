# DistributedApplicationBuilderHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/v0.3.1/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationBuilderHostBuilderExtensions](/Dovetail/v0.3.1/api/joinery.dovetail.aspire/distributedapplicationbuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying DistributedApplicationBuilder parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuredistributedapplicationbuilder"></a>
<a id="configuredistributedapplicationbuilder-dovetailcontextbuilder-distributedapplicationbuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplicationBuilder(DovetailContextBuilder, DistributedApplicationBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplicationBuilder(DovetailContextBuilder, DistributedApplicationBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DistributedApplicationBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplicationBuilder(
        this DovetailContextBuilder container,
        DistributedApplicationBuilderJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationBuilderJointDelegate](/Dovetail/v0.3.1/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuredistributedapplicationbuilder-dovetailcontextbuilder-distributedapplicationbuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureDistributedApplicationBuilder(DovetailContextBuilder, DistributedApplicationBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureDistributedApplicationBuilder(DovetailContextBuilder, DistributedApplicationBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class DistributedApplicationBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureDistributedApplicationBuilder(
        this DovetailContextBuilder container,
        DistributedApplicationBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([DistributedApplicationBuilderAsyncJointDelegate](/Dovetail/v0.3.1/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
