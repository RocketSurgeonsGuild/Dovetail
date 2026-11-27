using Joinery.Dovetail.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ReSharper disable UnusedVariable
namespace Joinery.Dovetail;
// TODO: analyzers
//

/// <summary>
///     Generator to handle materializing conventions as code instead of loading them at runtime
/// </summary>
[Generator]
public class DovetailAttributesGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(z => z.AddEmbeddedAttributeDefinition());
        var exportConfiguration = DovetailConfigurationData
                                 .Read(context, "Export")
                                 .WithTrackingName("dovetail:export_configuration");

        var importConfiguration = DovetailConfigurationData
                                 .Read(context, "Import")
                                 .WithTrackingName("dovetail:import_configuration");

        var msBuildConfig = context
                           .AnalyzerConfigOptionsProvider
                           .Combine(exportConfiguration)
                           .Combine(importConfiguration)
                           .Select(static (provider, _) => new MsBuildConfig(
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailMetadata", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailAssignExternal", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("IsTestProject", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("RootNamespace", s => s) ?? "",
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailHostType", s => s) ?? "Undefined",
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailCategory", s => s) ?? "Unknown",
                                       provider.Left.Right,
                                       provider.Right,
                                       // Unset (no Dovetail targets, e.g. a hand-wired analyzer) keeps the historical behavior of doing both.
                                       provider.Left.Left.GlobalOptions.GetBuildProperty<bool?>("ImportDovetailAssembly", x => bool.TryParse(x, out var v) ? v : null) ?? true,
                                       provider.Left.Left.GlobalOptions.GetBuildProperty<bool?>("ExportDovetailAssembly", x => bool.TryParse(x, out var v) ? v : null) ?? true,
                                       // Read the way Specular's own generator reads it, so Dovetail only names a SpecularProvider that Specular emits.
                                       provider.Left.Left.GlobalOptions.GetBuildProperty<bool?>("SpecularEmitProvider", x => bool.TryParse(x, out var v) ? v : null) ?? true
                                   )
                            )
                           .WithTrackingName("dovetail:msbuild");

        // Each joint is described in the transform, so the pipeline holds equatable models rather than symbols or locations: an
        // edit that doesn't change a joint, even one in the joint's own file, leaves the collected models equal, and the exports
        // aren't generated again. DovetailJointAnalyzer reports what is wrong with a joint.
        var exportedDovetails = context
                               .SyntaxProvider
                               .ForAttributeWithMetadataName(
                                    JointChecks.ExportAttributeMetadataName,
                                    static (node, _) => node is TypeDeclarationSyntax,
                                    static (syntaxContext, _) => ExportDovetails.Describe((INamedTypeSymbol)syntaxContext.TargetSymbol)
                                )
                               .Collect()
                               .Select(static (z, _) => z.Sort(static (x, y) => string.Compare(x.MetadataName, y.MetadataName, StringComparison.Ordinal)).ToEquatableArray())
                               .WithTrackingName("dovetail:self_exports");

        // Read from every compilation, but compared by value, so the imports are generated again only when the references change.
        var referencedExports = context
                               .CompilationProvider
                               .Select(static (compilation, _) => ImportDovetails.ReferencedExports.From(compilation))
                               .WithTrackingName("dovetail:referenced_exports");

        // Optionary's generator emits the registration method whenever Optionary is referenced; when this assembly declares an
        // [Optionary] type, the exports list a joint that calls it. Found here rather than read from Optionary's generator.
        var hasConfigurationOptions = OptionaryJoint.HasDeclarations(context);

        context.RegisterSourceOutput(
            msBuildConfig
               .Combine(exportedDovetails)
               .Combine(hasConfigurationOptions),
            static (productionContext, tuple) => ExportDovetails.HandleDovetailExports(productionContext, new(tuple.Left.Left, tuple.Left.Right, tuple.Right))
        );

        context.RegisterSourceOutput(
            referencedExports
               .Combine(exportedDovetails.Select(static (z, _) => z.Count > 0))
               .Combine(msBuildConfig)
               .Combine(hasConfigurationOptions),
            static (productionContext, tuple) => ImportDovetails.HandleDovetailImports(
                productionContext,
                new(tuple.Left.Right, tuple.Left.Left.Left, tuple.Left.Left.Right, tuple.Right)
            )
        );
    }
}
