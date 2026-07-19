using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Dovetail.Diagnostics;
using Dovetail.Infrastructure;
using FoggyBalrog.MermaidDotNet;
using FoggyBalrog.MermaidDotNet.Flowchart;
using FoggyBalrog.MermaidDotNet.Flowchart.Model;

namespace Dovetail;

/// <summary>
///     Adds support for rendering an <see cref="IDovetailContext" />'s resolved joints as a Mermaid diagram.
/// </summary>
public static class DovetailContextDiagram
{

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2072",
        Justification = "This is a diagnostics helper that reflects over already-instantiated joints; their interfaces are not trimmable away.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "This is a diagnostics helper that reflects over already-instantiated joints; their attributes are not trimmable away.")]
    private static string ToMermaidJointFlowDiagramInternal(IEnumerable<DovetailDiagnosticMetadataJoint> metadata)
    {
        var wellKnownJointNames = BaselineJointNames.ToImmutableHashSet(StringComparer.Ordinal);

        var stepEdges = metadata
        .SelectMany(z => z.ConnectsToJointTypes, (meta, v) => (meta, v))
        .SelectMany(z => z.meta.ImplementsJointTypes.Select(jointName => (From: jointName, To: z.v)))
        .ToImmutableHashSet();

        stepEdges = stepEdges.Add((SetupJointName, ConfigurationJointName));
        stepEdges = stepEdges.Add((ConfigurationJointName, ServiceJointName));
        stepEdges = stepEdges.Add((ServiceJointName, HostJointName));
        stepEdges = stepEdges.Add((HostJointName, HostCreatedJointName));

        var connectorJoints = metadata
        .SelectMany(meta => meta.ConnectsToJointTypes.Select(v => (Step: v, ConnectedTo: meta.JointName)))
        .ToLookup(z => z.Step, z => z.ConnectedTo);

        var jointsByStep = metadata
        .SelectMany(meta => meta.ImplementsJointTypes.Select(jointName => (Step: jointName, JointType: meta)))
        .ToLookup(z => z.Step, z => z.JointType);

        var builder = Mermaid.Flowchart("Dovetail Joint Flow", orientation: FlowchartOrientation.TopToBottom);

        var hostTypeClasses = DefineHostTypeCssClasses(builder);

        var stepSubgraphs = new Dictionary<string, Subgraph>(StringComparer.Ordinal);
        var connectedStepNodes = new Dictionary<string, Node>();
        var metadataNodes = new Dictionary<DovetailDiagnosticMetadataJoint, Node>();
        var metadataTypeNodes = new Dictionary<Type, Node>();
        var discoveredJointNames = metadata.SelectMany(z => z.ImplementsJointTypes.Concat(z.ConnectsToJointTypes)).Distinct(StringComparer.Ordinal);

        foreach (var stepName in BaselineJointNames.Concat(discoveredJointNames).Distinct(StringComparer.Ordinal))
        {
            var connector = connectorJoints[stepName];
            var title = $"{stepName}{( connector.Any() ? $" via {string.Join(", ", connector)}" : "" )}";
            builder.AddSubgraph(
                title,
                out var stepSubgraph,
                subgraph =>
                {
                    // A joint already placed under an earlier step contributes no new node here, so an
                    // assembly group with nothing left to place is skipped rather than rendered as an empty box.
                    foreach (var assemblyGroup in jointsByStep[stepName]
                            .OrderBy(z => z.AssemblyName, StringComparer.Ordinal)
                                .GroupBy(z => z.AssemblyName))
                    {
                        subgraph.AddSubgraph(
                            assemblyGroup.Key,
                            out _,
                            assemblySubgraph =>
                            {
                                foreach (var jointType in assemblyGroup)
                                {
                                    getOrAddJoint(assemblySubgraph, jointType);
                                }
                            }
                        );
                    }
                },
                FlowchartOrientation.RightToLeft
            );

            stepSubgraphs[stepName] = stepSubgraph;
        }

        // foreach (var )

        // foreach (var connectedNode in connectedTo)
        // {
        //     sb.AppendLine($"  {connectedNode}");
        //     if (stepSubgraphs.TryGetValue(connectedNode, out var subgraph))
        //     {
        //         scope.AddLink(node, subgraph, out _);
        //     }
        // }

        foreach (var (stepName, node) in connectedStepNodes)
        {
            if (!stepSubgraphs.TryGetValue(stepName, out var subgraph)) continue;
            builder.AddLink(node, subgraph, out _, lineStyle: LinkLineStyle.Dotted);
        }

        foreach (var (from, to) in stepEdges.Where(z => !connectedStepNodes.ContainsKey(z.To)))
        {
            builder.AddLink(stepSubgraphs[from], stepSubgraphs[to], out _, lineStyle: LinkLineStyle.Dotted);
        }

        foreach (var item in metadata)
        {
            foreach (var dep in item.Dependencies)
            {
                if (!metadataNodes.TryGetValue(item, out var toNode)) continue;
                if (!metadataNodes.TryGetValue(dep, out var fromNode)) continue;
                if (dep.Direction == DependencyDirection.DependentOf)
                {
                    toNode = Interlocked.Exchange(ref fromNode, toNode);
                }
                builder.AddLink(fromNode, toNode, out _);
            }
        }

        return builder.Build();

        // A joint that belongs to more than one step (multiple [JointName] interfaces) can only physically live
        // in one subgraph; whichever step processes it first wins the placement, and later steps just link to it.
        Node getOrAddJoint(FlowchartBuilder scope, DovetailDiagnosticMetadataJoint item)
        {
            scope.AddMarkdownNode($"**{item.JointName}**<br/>{( item.HostType != DovetailHostType.Undefined ? $"HostType: **{item.HostType}**<br/>" : "" )}Category: **{item.Category}**", out var node);
            scope.StyleNodes(hostTypeClasses[item.HostType], node);
            metadataNodes[item] = node;

            // foreach (var stepName in jointType.GetConnectedTo().Select(JointName))
            // {
            //     connectedStepNodes[stepName] = node;
            // }
            return node;
        }
    }

    extension(IDovetailContext context)
    {
        /// <summary>
        ///    Renders the resolved joints of the context as a Mermaid flowchart diagram.
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public string ToMermaidJointFlowDiagram(params IEnumerable<DovetailCategory> categories) => ToMermaidJointFlowDiagramInternal(context.GetDiagnosticMetadata(categories));

        /// <summary>
        ///     Renders <see cref="ToMermaidJointFlowDiagram" /> wrapped in a Markdown mermaid code fence.
        /// </summary>
        /// <param name="categories"></param>
        public string ToMermaidJointFlowDiagramMarkdown(params IEnumerable<DovetailCategory> categories)
        {
            var sb2 = new StringBuilder();
            sb2.AppendLine("```mermaid");
            sb2.AppendLine(context.ToMermaidJointFlowDiagram(categories));
            sb2.AppendLine("```");
            return sb2.ToString();
        }
    }

    extension(DovetailContextBuilder contextBuilder)
    {
        /// <summary>
        ///    Renders the resolved joints of the context as a Mermaid flowchart diagram.
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public string ToMermaidJointFlowDiagram(params IEnumerable<DovetailCategory> categories) => ToMermaidJointFlowDiagramInternal(contextBuilder.GetDiagnosticMetadata(categories));

        /// <summary>
        ///     Renders <see cref="ToMermaidJointFlowDiagram" /> wrapped in a Markdown mermaid code fence.
        /// </summary>
        public string ToMermaidJointFlowDiagramMarkdown(params IEnumerable<DovetailCategory> categories)
        {
            var sb2 = new StringBuilder();
            sb2.AppendLine("```mermaid");
            sb2.AppendLine(contextBuilder.ToMermaidJointFlowDiagram(categories));
            sb2.AppendLine("```");
            return sb2.ToString();
        }
    }

    private const string ConfigurationJointName = "Configuration";
    private const string SetupJointName = "Setup";
    private const string ServiceJointName = "Service";
    private const string HostJointName = "Host";
    private const string HostCreatedJointName = "HostCreated";

    // The pipeline stages every Dovetail context goes through regardless of which optional joints are registered.
    private static readonly string[] BaselineJointNames = [SetupJointName, ConfigurationJointName, ServiceJointName, HostJointName, HostCreatedJointName];

    private static Dictionary<DovetailHostType, CssClass> DefineHostTypeCssClasses(FlowchartBuilder builder)
    {
        builder.DefineCssClass("hostUndefined", "text-align:left,fill:#eceff1,stroke:#607d8b,color:#263238", out var undefined);
        builder.DefineCssClass("hostLive", "text-align:left,fill:#c8e6c9,stroke:#2e7d32,color:#1b5e20", out var live);
        builder.DefineCssClass("hostUnitTest", "text-align:left,fill:#ffe0b2,stroke:#ef6c00,color:#e65100", out var unitTest);

        return new Dictionary<DovetailHostType, CssClass>
        {
            [DovetailHostType.Undefined] = undefined,
            [DovetailHostType.Live] = live,
            [DovetailHostType.UnitTest] = unitTest,
        };
    }
}
