# PipelineBuilderHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines.md)
- Type: [PipelineBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderhostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying PipelineBuilder parts to a `Dovetail.DovetailContextBuilder`.

<a id="configurepipelinebuilder"></a>
<a id="configurepipelinebuilder-dovetailcontextbuilder-pipelinebuilderjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigurePipelineBuilder(DovetailContextBuilder, PipelineBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigurePipelineBuilder(DovetailContextBuilder, PipelineBuilderJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class PipelineBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigurePipelineBuilder(
        this DovetailContextBuilder container,
        PipelineBuilderJointDelegate @delegate,
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
- `delegate` ([PipelineBuilderJointDelegate](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configurepipelinebuilder-dovetailcontextbuilder-pipelinebuilderasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigurePipelineBuilder(DovetailContextBuilder, PipelineBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigurePipelineBuilder(DovetailContextBuilder, PipelineBuilderAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`

```csharp
public static class PipelineBuilderHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigurePipelineBuilder(
        this DovetailContextBuilder container,
        PipelineBuilderAsyncJointDelegate @delegate,
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
- `delegate` ([PipelineBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
