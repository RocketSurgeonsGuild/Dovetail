using System.Collections.Concurrent;
using System.Collections.Immutable;
using Joinery.Dovetail.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Joinery.Dovetail;

/// <summary>
///     Reports what is wrong with the joints an assembly exports. The generator leaves those joints out without saying why: its
///     pipeline holds no locations, so an edit that only moves a joint doesn't generate the exports again.
/// </summary>
/// <remarks>
///     A circular dependency (DT0005) needs every joint of the compilation, so it is reported when the compilation ends. That is
///     on build, and in the editor only when the IDE analyzes the whole solution.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DovetailJointAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        Diagnostics.DovetailHasToManyConstructors,
        Diagnostics.DovetailHasNoCallableConstructor,
        Diagnostics.DovetailCannotBeGeneric,
        Diagnostics.MustBeTypeOf,
        Diagnostics.UnhandledSymbol,
        Diagnostics.CircularJointDependency,
    ];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        // The generator reads every file it is given, generated or not, so the analyzer does too.
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(static start =>
            {
                var joints = new ConcurrentBag<(INamedTypeSymbol Joint, ImmutableArray<DovetailDependencyParser.DependencyEdge> Edges)>();
                start.RegisterSymbolAction(symbolContext => AnalyzeJoint(symbolContext, joints), SymbolKind.NamedType);
                start.RegisterCompilationEndAction(end => ReportCycles(end, joints));
            }
        );
    }

    private static void AnalyzeJoint(
        SymbolAnalysisContext context,
        ConcurrentBag<(INamedTypeSymbol Joint, ImmutableArray<DovetailDependencyParser.DependencyEdge> Edges)> joints
    )
    {
        // The generator only looks at types declared with a type declaration: classes, records, structs and interfaces.
        if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Struct or TypeKind.Interface } joint) return;
        if (!JointChecks.IsExported(joint)) return;

        var location = joint.Locations.FirstOrDefault();
        if (JointChecks.Rejection(joint, out _) is { } rejection)
        {
            context.ReportDiagnostic(
                rejection == Diagnostics.DovetailHasNoCallableConstructor
                    ? Diagnostic.Create(rejection, location, joint.ToDisplayString())
                    : Diagnostic.Create(rejection, location)
            );
            return;
        }

        var edges = DovetailDependencyParser.ParseDependencies(
            joint,
            (issueLocation, descriptor) => context.ReportDiagnostic(Diagnostic.Create(descriptor, issueLocation ?? location))
        );
        joints.Add((joint, edges));
    }

    private static void ReportCycles(
        CompilationAnalysisContext context,
        ConcurrentBag<(INamedTypeSymbol Joint, ImmutableArray<DovetailDependencyParser.DependencyEdge> Edges)> joints
    )
    {
        if (joints.IsEmpty) return;

        // In the order the generator lists the joints, so the cycles are found, and reported, the same way every time.
        var ordered = joints
                     .Select(static z => (Name: z.Joint.ToDisplayString(), z.Joint, z.Edges))
                     .OrderBy(static z => z.Joint.MetadataName, StringComparer.Ordinal)
                     .ThenBy(static z => z.Name, StringComparer.Ordinal)
                     .ToList();
        var byName = new Dictionary<string, INamedTypeSymbol>(StringComparer.Ordinal);
        foreach (var (name, joint, _) in ordered)
        {
            byName[name] = joint;
        }

        foreach (var (name, cycle) in DovetailDependencyParser.DetectCycles(ordered.Select(static z => (z.Name, (IEnumerable<DovetailDependencyParser.DependencyEdge>)z.Edges))))
        {
            var joint = byName[name];
            var chain = string.Join(" -> ", cycle.Select(c => byName[c].Name));
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.CircularJointDependency, joint.Locations.FirstOrDefault(), joint.Name, chain));
        }
    }
}
