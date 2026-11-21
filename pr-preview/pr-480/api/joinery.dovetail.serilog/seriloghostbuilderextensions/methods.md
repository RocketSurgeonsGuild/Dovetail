# SerilogHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.serilog.md)
- Type: [SerilogHostBuilderExtensions](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.serilog/seriloghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Serilog parts to a `Dovetail.DovetailContextBuilder`.

<a id="configureserilog"></a>
<a id="configureserilog-dovetailcontextbuilder-serilogjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureSerilog(DovetailContextBuilder, SerilogJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureSerilog(DovetailContextBuilder, SerilogJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-480-merge.Sha.bbd00571859e9fa257a29d29502064fe14badc48/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/DovetailJoint.Serilog.g.cs)

```csharp
public static class SerilogHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureSerilog(
        this DovetailContextBuilder container,
        SerilogJointDelegate @delegate,
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
- `delegate` ([SerilogJointDelegate](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.serilog/serilogjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configureserilog-dovetailcontextbuilder-serilogasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureSerilog(DovetailContextBuilder, SerilogAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureSerilog(DovetailContextBuilder, SerilogAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-480-merge.Sha.bbd00571859e9fa257a29d29502064fe14badc48/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/DovetailJoint.Serilog.g.cs)

```csharp
public static class SerilogHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureSerilog(
        this DovetailContextBuilder container,
        SerilogAsyncJointDelegate @delegate,
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
- `delegate` ([SerilogAsyncJointDelegate](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.serilog/serilogasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
