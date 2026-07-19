using System.Collections.Immutable;
using System.Reflection;
using System.Text;
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
    extension(IDovetailJointMetadata metadata)
    {
        /// <summary>
        ///    The assembly that is executing the convention
        /// </summary>
        public Assembly Assembly => metadata.Joint is IDovetailJointDelegate jointDelegate ? jointDelegate.Assembly : metadata.Joint.GetType().Assembly;

        /// <summary>
        ///    The name of the joint
        /// </summary>
        public string JointName => metadata.Joint is IDovetailJointDelegate jointDelegate ? jointDelegate.Expression : metadata.Joint.GetType().FullName!;

        /// <summary>
        ///   Gets the names of the steps that this joint implements
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetStepNames() => metadata.Joint.GetType().GetInterfaces()
                       .Where(z => z.IsAssignableTo(typeof(IDovetailJoint)) && z != typeof(IDovetailJoint))
                       .Select(t => t.GetCustomAttribute<JointNameAttribute>()?.Name!)
                       .Where(z => !string.IsNullOrWhiteSpace(z));

        /// <summary>
        ///   Gets the types of the steps that this joint is connected to
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetConnectedTo() => metadata.Joint.GetType()
               .GetCustomAttributesData()
               .Where(z => z.AttributeType.IsGenericType && z.AttributeType.GetGenericTypeDefinition() == typeof(ConnectsJointAttribute<>))
               .Select(z => z.AttributeType.GetGenericArguments()[0]);

        /// <summary>
        ///    Gets the names of the steps that this joint is connected to
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetConnectedToStepNames() => metadata.GetConnectedTo()
               .Select(z => z.GetCustomAttribute<JointNameAttribute>()?.Name!)
               .Where(z => !string.IsNullOrWhiteSpace(z));

    }

    extension(IDovetailContext context)
    {
        /// <summary>
        ///     Renders a Mermaid flowchart of the high-level flow between joint types (<see cref="JointNameAttribute" />)
        ///     rather than individual joint instances. The core pipeline (Configuration, Service, Host, HostCreated) is
        ///     always shown as a distinct "step" shape; every registered joint is its own node, tagged with its
        ///     category (if specified) and colored by the host type it's restricted to. A joint with a specific step
        ///     of its own (e.g. OpenTelemetry) is linked from every step it belongs to, and <see cref="ConnectsJointAttribute{T}" />
        ///     bridges that step to whatever it connects to. A joint with no specific step of its own - just a generic
        ///     bucket like Service - is instead homed directly under the step it connects to (e.g. Logging), since it's
        ///     really just a vehicle for that connection rather than a step in its own right. All joints are included
        ///     regardless of host type; only their color reflects the restriction.
        /// </summary>
        [UnconditionalSuppressMessage(
            "Trimming",
            "IL2072",
            Justification = "This is a diagnostics helper that reflects over already-instantiated joints; their interfaces are not trimmable away.")]
        [UnconditionalSuppressMessage(
            "Trimming",
            "IL2075",
            Justification = "This is a diagnostics helper that reflects over already-instantiated joints; their attributes are not trimmable away.")]
        public string ToMermaidJointFlowDiagram(params IEnumerable<DovetailCategory> categories)
        {
            var metadata = DovetailResolver.ResolveMetadata([.. categories], context.Metadata);

            var wellKnownJointNames = BaselineJointNames.ToImmutableHashSet(StringComparer.Ordinal);

            var stepEdges = metadata
            .SelectMany(z => z.GetConnectedTo(), (meta, v) => (meta, v))
            .SelectMany(z => z.meta.GetStepNames().Select(jointName => (From: jointName, To: JointName(z.v))))
            .ToImmutableHashSet();

            stepEdges = stepEdges.Add((SetupJointName, ConfigurationJointName));
            stepEdges = stepEdges.Add((ConfigurationJointName, ServiceJointName));
            stepEdges = stepEdges.Add((ServiceJointName, HostJointName));
            stepEdges = stepEdges.Add((HostJointName, HostCreatedJointName));

            var connectorJoints = metadata
            .SelectMany(meta => meta.GetConnectedTo().Select(v => (Step: JointName(v), ConnectedTo: meta.JointName)))
            .ToLookup(z => z.Step, z => z.ConnectedTo);

            var jointsByStep = metadata
            .SelectMany(meta => meta.GetStepNames().Select(jointName => (Step: jointName, JointType: meta)))
            .ToLookup(z => z.Step, z => z.JointType);

            var builder = Mermaid.Flowchart("Dovetail Joint Flow", orientation: FlowchartOrientation.TopToBottom);

            var hostTypeClasses = DefineHostTypeCssClasses(builder);

            var stepSubgraphs = new Dictionary<string, Subgraph>(StringComparer.Ordinal);
            var connectedStepNodes = new Dictionary<string, Node>();
            var metadataNodes = new Dictionary<IDovetailJointMetadata, Node>();
            var metadataTypeNodes = new Dictionary<Type, Node>();
            // Every step referenced anywhere - whether it has joints of its own, is part of the baseline pipeline,
            // shows up as the target of a connection, or is simply a known [JointName] extension point that
            // nothing has implemented yet (e.g. ISerilogJoint, discoverable via the Serilog assembly since
            // SerilogJoint itself is registered there) - gets a subgraph, even if it ends up empty. Step-to-step
            // edges link subgraph boundaries directly instead of a separate representative node.
            var discoveredJointNames = metadata.SelectMany(z => z.GetStepNames().Concat(z.GetConnectedTo().Select(JointName))).Distinct(StringComparer.Ordinal);

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
                                    .GroupBy(z => z.Assembly)
                                    .OrderBy(z => AssemblyName(z.Key), StringComparer.Ordinal))
                        {
                            subgraph.AddSubgraph(
                                AssemblyName(assemblyGroup.Key),
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
                    if (!metadataTypeNodes.TryGetValue(dep.Type, out var fromNode)) continue;
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
            Node getOrAddJoint(FlowchartBuilder scope, IDovetailJointMetadata jointType)
            {
                scope.AddMarkdownNode($"**{jointType.JointName}**<br/>{( jointType.HostType != DovetailHostType.Undefined ? $"HostType: **{jointType.HostType}**<br/>" : "" )}Category: **{jointType.Category}**", out var node);
                scope.StyleNodes(hostTypeClasses[jointType.HostType], node);
                metadataNodes[jointType] = node;
                metadataTypeNodes[jointType.Joint.GetType()] = node;

                // foreach (var stepName in jointType.GetConnectedTo().Select(JointName))
                // {
                //     connectedStepNodes[stepName] = node;
                // }
                return node;
            }
        }

        /// <summary>
        ///     Renders <see cref="ToMermaidJointFlowDiagram" /> wrapped in a Markdown mermaid code fence.
        /// </summary>
        public string ToMermaidJointFlowDiagramMarkdown(params IEnumerable<DovetailCategory> categories)
        {
            var sb2 = new StringBuilder();
            sb2.AppendLine("```mermaid");
            sb2.AppendLine(context.ToMermaidJointFlowDiagram(categories));
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

    private static string AssemblyName(Assembly assembly) => assembly.GetName().Name ?? "Unknown";

    // Joint interfaces (and their paired delegates) are tagged with a friendly [JointName],
    // e.g. "IServiceJoint" -> "Service". Fall back to the raw interface name for ad-hoc joints
    // that don't carry the attribute.
    private static string JointName(Type jointInterface) => jointInterface.GetCustomAttribute<JointNameAttribute>()?.Name ?? jointInterface.Name;

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
