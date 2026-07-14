using Dovetail.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ReSharper disable UnusedVariable
namespace Dovetail;
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
                           .Select((provider, _) => new MsBuildConfig(
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailMetadata", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailAssignExternal", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("IsTestProject", x => bool.TryParse(x, out var v) && v),
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("RootNamespace", s => s) ?? "",
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailHostType", s => s) ?? "Undefined",
                                       provider.Left.Left.GlobalOptions.GetBuildProperty("DovetailCategory", s => s) ?? "Unknown",
                                       provider.Left.Right,
                                       provider.Right
                                   )
                            )
                           .WithTrackingName("dovetail:msbuild");

        var exportedDovetails = context
                                 .SyntaxProvider
                                 .ForAttributeWithMetadataName(
                                      "Dovetail.Attributes.DovetailExportAttribute",
                                      (node, _) => node is TypeDeclarationSyntax,
                                      (syntaxContext, _) => (INamedTypeSymbol)syntaxContext.TargetSymbol
                                  )
                                 .Collect()
                                 .Select((z, _) => z.Sort(Comparer<INamedTypeSymbol>.Create((x, y) => string.Compare(x.MetadataName, y.MetadataName, StringComparison.Ordinal))))
                                 .WithTrackingName("dovetail_:self_exports");

        context.RegisterSourceOutput(
            msBuildConfig
               .Combine(context.CompilationProvider)
               .Combine(exportedDovetails),
            static (productionContext, tuple) => ExportDovetails.HandleDovetailExports(
                productionContext,
                new(tuple.Left.Left, tuple.Right)
            )
        );


        context.RegisterSourceOutput(
            context
               .CompilationProvider
               .Combine(exportedDovetails)
               .Combine(msBuildConfig),
            static (productionContext, tuple) => ImportDovetails.HandleDovetailImports(productionContext, new(tuple.Left.Left, tuple.Right, tuple.Left.Right))
        );
    }
}
