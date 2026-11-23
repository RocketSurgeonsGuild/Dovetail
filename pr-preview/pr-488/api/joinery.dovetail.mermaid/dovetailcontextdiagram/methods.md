# DovetailContextDiagram Methods

- Package: [Joinery.Dovetail.Mermaid](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.mermaid.md)
- Type: [DovetailContextDiagram](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.mermaid/dovetailcontextdiagram.md)
- Kind: `Methods`
- Members: `4`

Adds support for rendering an `Dovetail.IDovetailContext` 's resolved joints as a Mermaid diagram.

<a id="tomermaidjointflowdiagram"></a>
<a id="tomermaidjointflowdiagram-idovetailcontext-ienumerable-dovetailcategory"></a>

## ToMermaidJointFlowDiagram(IDovetailContext, IEnumerable<DovetailCategory>)

- Name: `ToMermaidJointFlowDiagram(IDovetailContext, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.0e8baabd008e578e204d5b77e4720c2164a0a162/src/Joinery.Dovetail.Mermaid/DovetailContextDiagram.cs#L156)

```csharp
public static class DovetailContextDiagram
{
    public static string ToMermaidJointFlowDiagram(
        this IDovetailContext context,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `categories` (`IEnumerable<DovetailCategory>`)

<a id="tomermaidjointflowdiagram-dovetailcontextbuilder-ienumerable-dovetailcategory"></a>

## ToMermaidJointFlowDiagram(DovetailContextBuilder, IEnumerable<DovetailCategory>)

- Name: `ToMermaidJointFlowDiagram(DovetailContextBuilder, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.0e8baabd008e578e204d5b77e4720c2164a0a162/src/Joinery.Dovetail.Mermaid/DovetailContextDiagram.cs#L180)

```csharp
public static class DovetailContextDiagram
{
    public static string ToMermaidJointFlowDiagram(
        this DovetailContextBuilder contextBuilder,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `contextBuilder` (`DovetailContextBuilder`)
- `categories` (`IEnumerable<DovetailCategory>`)

<a id="tomermaidjointflowdiagrammarkdown"></a>
<a id="tomermaidjointflowdiagrammarkdown-idovetailcontext-ienumerable-dovetailcategory"></a>

## ToMermaidJointFlowDiagramMarkdown(IDovetailContext, IEnumerable<DovetailCategory>)

- Name: `ToMermaidJointFlowDiagramMarkdown(IDovetailContext, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.0e8baabd008e578e204d5b77e4720c2164a0a162/src/Joinery.Dovetail.Mermaid/DovetailContextDiagram.cs#L165-L169)

```csharp
public static class DovetailContextDiagram
{
    public static string ToMermaidJointFlowDiagramMarkdown(
        this IDovetailContext context,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `categories` (`IEnumerable<DovetailCategory>`)

<a id="tomermaidjointflowdiagrammarkdown-dovetailcontextbuilder-ienumerable-dovetailcategory"></a>

## ToMermaidJointFlowDiagramMarkdown(DovetailContextBuilder, IEnumerable<DovetailCategory>)

- Name: `ToMermaidJointFlowDiagramMarkdown(DovetailContextBuilder, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.0e8baabd008e578e204d5b77e4720c2164a0a162/src/Joinery.Dovetail.Mermaid/DovetailContextDiagram.cs#L189-L193)

```csharp
public static class DovetailContextDiagram
{
    public static string ToMermaidJointFlowDiagramMarkdown(
        this DovetailContextBuilder contextBuilder,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `contextBuilder` (`DovetailContextBuilder`)
- `categories` (`IEnumerable<DovetailCategory>`)
