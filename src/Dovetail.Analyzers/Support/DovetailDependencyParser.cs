using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Dovetail.Support;

/// <summary>
///     Parses the <c>DependsOnJoint</c>/<c>DependentOfJoint</c>/<c>BeforeJoint</c>/<c>AfterJoint</c> attributes
///     declared on a joint type into a normalized edge list, and detects cycles across a set of joints. Shared
///     between <see cref="ExportDovetails" /> (which reports diagnostics and emits <c>.WithDependency(...)</c>
///     calls) and the Mermaid diagram builder (which only needs the edges).
/// </summary>
internal static class DovetailDependencyParser
{
    public enum DependencyKind
    {
        DependsOn,
        DependentOf,
    }

    public readonly record struct DependencyEdge(DependencyKind Direction, INamedTypeSymbol Target);

    /// <summary>
    ///     Parses the dependency attributes on <paramref name="convention" />. Malformed attribute arguments are
    ///     returned as issues rather than reported directly, so callers control whether/where to report them
    ///     (avoids double-reporting when both <see cref="ExportDovetails" /> and the diagram builder parse the
    ///     same joint).
    /// </summary>
    public static (ImmutableArray<DependencyEdge> Edges, ImmutableArray<(Location? Location, DiagnosticDescriptor Descriptor)> Issues) ParseDependencies(INamedTypeSymbol convention)
    {
        var edges = ImmutableArray.CreateBuilder<DependencyEdge>();
        var issues = ImmutableArray.CreateBuilder<(Location? Location, DiagnosticDescriptor Descriptor)>();

        foreach (var attributeData in convention.GetAttributes())
        {
            var kind = attributeData.AttributeClass?.Name switch
            {
                "DependentOfJointAttribute" or "BeforeJointAttribute" => DependencyKind.DependentOf,
                "DependsOnJointAttribute" or "AfterJointAttribute" => DependencyKind.DependsOn,
                _ => (DependencyKind?)null,
            };

            if (kind is null) continue;

            var location = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();

            switch (attributeData)
            {
                case { AttributeClass.TypeArguments: [INamedTypeSymbol target] }:
                    edges.Add(new(kind.Value, target));
                    break;
                case { ConstructorArguments: [{ Kind: TypedConstantKind.Type, Value: INamedTypeSymbol target }] }:
                    edges.Add(new(kind.Value, target));
                    break;
                case { ConstructorArguments: [{ Kind: not TypedConstantKind.Type }] }:
                    issues.Add((location, Diagnostics.MustBeTypeOf));
                    break;
                default:
                    issues.Add((location, Diagnostics.UnhandledSymbol));
                    break;
            }
        }

        return (edges.ToImmutable(), issues.ToImmutable());
    }

    /// <summary>
    ///     Detects cycles across the given local joint dependency graph. Only edges pointing at another local joint
    ///     can participate in a cycle (an edge to a joint exported from a different assembly can't be verified at
    ///     this compile-time boundary). Returns one entry per joint that participates in a detected cycle, together
    ///     with the full cycle chain for diagnostic reporting.
    /// </summary>
    public static ImmutableArray<(INamedTypeSymbol Joint, ImmutableArray<INamedTypeSymbol> Cycle)> DetectCycles(
        ImmutableArray<(INamedTypeSymbol Joint, ImmutableArray<DependencyEdge> Edges)> graph
    )
    {
        var comparer = SymbolEqualityComparer.Default;
        var jointSet = new HashSet<INamedTypeSymbol>(graph.Select(g => g.Joint), comparer);

        // Normalize both attribute directions into a single "runs before" adjacency list: DependsOn means the
        // target runs before the joint; DependentOf (and its Before/After aliases) means the joint runs before
        // the target.
        var runsBefore = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(comparer);
        foreach (var (joint, edges) in graph)
        {
            foreach (var edge in edges)
            {
                if (!jointSet.Contains(edge.Target)) continue;

                var (from, to) = edge.Direction == DependencyKind.DependsOn ? (edge.Target, joint) : (joint, edge.Target);
                if (!runsBefore.TryGetValue(from, out var list)) runsBefore[from] = list = [];
                list.Add(to);
            }
        }

        var results = ImmutableArray.CreateBuilder<(INamedTypeSymbol, ImmutableArray<INamedTypeSymbol>)>();
        var state = new Dictionary<INamedTypeSymbol, int>(comparer); // 0/absent = unvisited, 1 = in-progress, 2 = done
        var stack = new List<INamedTypeSymbol>();
        var reported = new HashSet<INamedTypeSymbol>(comparer);

        foreach (var joint in graph.Select(g => g.Joint))
        {
            if (!state.ContainsKey(joint)) Visit(joint);
        }

        return results.ToImmutable();

        void Visit(INamedTypeSymbol node)
        {
            state[node] = 1;
            stack.Add(node);

            if (runsBefore.TryGetValue(node, out var successors))
            {
                foreach (var successor in successors)
                {
                    if (state.TryGetValue(successor, out var successorState))
                    {
                        if (successorState == 1)
                        {
                            var cycleStart = stack.FindIndex(x => comparer.Equals(x, successor));
                            var cycle = stack.Skip(cycleStart).Append(successor).ToImmutableArray();
                            foreach (var participant in cycle.Distinct(comparer).Cast<INamedTypeSymbol>())
                            {
                                if (reported.Add(participant)) results.Add((participant, cycle));
                            }
                        }

                        continue;
                    }

                    Visit(successor);
                }
            }

            stack.RemoveAt(stack.Count - 1);
            state[node] = 2;
        }
    }
}
