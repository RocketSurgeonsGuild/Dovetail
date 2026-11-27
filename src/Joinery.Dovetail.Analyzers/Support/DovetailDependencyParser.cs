using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail.Support;

/// <summary>
///     Parses the <c>DependsOnJoint</c>/<c>DependentOfJoint</c>/<c>BeforeJoint</c>/<c>AfterJoint</c> attributes declared on a
///     joint type into a normalized edge list, and detects cycles across a set of joints. The results hold names, not symbols, so
///     they can be part of the generator's cached pipeline values.
/// </summary>
internal static class DovetailDependencyParser
{
    public enum DependencyKind
    {
        DependsOn,
        DependentOf,
    }

    /// <summary>An edge from the joint that declares it to <paramref name="Target" />, named by its display string.</summary>
    public readonly record struct DependencyEdge(DependencyKind Direction, string Target);

    /// <summary>A malformed dependency attribute, reported where the attribute is applied.</summary>
    public readonly record struct DependencyIssue(Location? Location, DiagnosticDescriptor Descriptor);

    /// <summary>
    ///     Parses the dependency attributes on <paramref name="convention" />. Malformed attribute arguments are returned as issues
    ///     rather than reported directly, so the caller controls whether and where to report them.
    /// </summary>
    public static (ImmutableArray<DependencyEdge> Edges, ImmutableArray<DependencyIssue> Issues) ParseDependencies(INamedTypeSymbol convention)
    {
        var edges = ImmutableArray.CreateBuilder<DependencyEdge>();
        var issues = ImmutableArray.CreateBuilder<DependencyIssue>();

        foreach (var attributeData in convention.GetAttributes())
        {
            var kind = attributeData.AttributeClass?.Name switch
            {
                "DependentOfJointAttribute" or "BeforeJointAttribute" => DependencyKind.DependentOf,
                "DependsOnJointAttribute" or "AfterJointAttribute" => DependencyKind.DependsOn,
                _ => (DependencyKind?)null,
            };

            if (kind is null) continue;

            switch (attributeData)
            {
                case { AttributeClass.TypeArguments: [INamedTypeSymbol target] }:
                    edges.Add(new(kind.Value, target.ToDisplayString()));
                    break;
                case { ConstructorArguments: [{ Kind: TypedConstantKind.Type, Value: INamedTypeSymbol target }] }:
                    edges.Add(new(kind.Value, target.ToDisplayString()));
                    break;
                case { ConstructorArguments: [{ Kind: not TypedConstantKind.Type }] }:
                    issues.Add(new(LocationOf(attributeData), Diagnostics.MustBeTypeOf));
                    break;
                default:
                    issues.Add(new(LocationOf(attributeData), Diagnostics.UnhandledSymbol));
                    break;
            }
        }

        return (edges.ToImmutable(), issues.ToImmutable());

        static Location? LocationOf(AttributeData attribute) => attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation();
    }

    /// <summary>
    ///     Detects cycles across the given local joint dependency graph, whose joints and edge targets are named by their display
    ///     strings. Only edges pointing at another local joint can participate in a cycle (an edge to a joint exported from a
    ///     different assembly can't be verified at this compile-time boundary). Returns one entry per joint that participates in a
    ///     detected cycle, together with the full cycle chain for diagnostic reporting.
    /// </summary>
    public static ImmutableArray<(string Joint, ImmutableArray<string> Cycle)> DetectCycles(
        IEnumerable<(string Joint, IEnumerable<DependencyEdge> Edges)> graph
    )
    {
        var joints = graph.ToList();
        var comparer = StringComparer.Ordinal;
        var jointSet = new HashSet<string>(joints.Select(g => g.Joint), comparer);

        // Normalize both attribute directions into a single "runs before" adjacency list: DependsOn means the
        // target runs before the joint; DependentOf (and its Before/After aliases) means the joint runs before
        // the target.
        var runsBefore = new Dictionary<string, List<string>>(comparer);
        foreach (var (joint, edges) in joints)
        {
            foreach (var edge in edges)
            {
                if (!jointSet.Contains(edge.Target)) continue;

                var (from, to) = edge.Direction == DependencyKind.DependsOn ? (edge.Target, joint) : (joint, edge.Target);
                if (!runsBefore.TryGetValue(from, out var list)) runsBefore[from] = list = [];
                list.Add(to);
            }
        }

        var results = ImmutableArray.CreateBuilder<(string, ImmutableArray<string>)>();
        var state = new Dictionary<string, int>(comparer); // 0/absent = unvisited, 1 = in-progress, 2 = done
        var stack = new List<string>();
        var reported = new HashSet<string>(comparer);

        foreach (var (joint, _) in joints)
        {
            if (!state.ContainsKey(joint)) Visit(joint);
        }

        return results.ToImmutable();

        void Visit(string node)
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
                            var cycleBuilder = ImmutableArray.CreateBuilder<string>(stack.Count - cycleStart + 1);
                            for (var i = cycleStart; i < stack.Count; i++) cycleBuilder.Add(stack[i]);
                            cycleBuilder.Add(successor);
                            var cycle = cycleBuilder.MoveToImmutable();
                            // Each node is on the DFS stack at most once, so stack[cycleStart..] are the distinct
                            // participants; the closing successor is stack[cycleStart] again.
                            for (var i = cycleStart; i < stack.Count; i++)
                            {
                                if (reported.Add(stack[i])) results.Add((stack[i], cycle));
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
