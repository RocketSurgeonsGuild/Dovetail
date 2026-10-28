# DovetailContextDiagram Methods

- Package: [Dovetail.Mermaid](/Dovetail/pr-preview/pr-236/api/dovetail.mermaid.md)
- Type: [DovetailContextDiagram](/Dovetail/pr-preview/pr-236/api/dovetail.mermaid/dovetailcontextdiagram.md)
- Kind: `Methods`
- Members: `4`

Adds support for rendering an `IDovetailContext` 's resolved joints as a Mermaid diagram.

<a id="tomermaidjointflowdiagram"></a>
<a id="tomermaidjointflowdiagram-idovetailcontext-ienumerable-dovetailcategory"></a>

## ToMermaidJointFlowDiagram(IDovetailContext, IEnumerable<DovetailCategory>)

- Name: `ToMermaidJointFlowDiagram(IDovetailContext, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: `string`

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
