using Dovetail.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
                                       provider.Right,
                                       // Unset (no Dovetail targets, e.g. a hand-wired analyzer) keeps the historical behavior of doing both.
                                       provider.Left.Left.GlobalOptions.GetBuildProperty<bool?>("ImportDovetailAssembly", x => bool.TryParse(x, out var v) ? v : null) ?? true,
                                       provider.Left.Left.GlobalOptions.GetBuildProperty<bool?>("ExportDovetailAssembly", x => bool.TryParse(x, out var v) ? v : null) ?? true
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

        // The generated code uses C# 12 syntax (collection expressions). The analyzer reaches projects that only reference
        // Dovetail transitively, so rather than break a project pinned to an older LangVersion, emit nothing and say why.
        var languageVersion = context
                             .ParseOptionsProvider
                             .Select((options, _) => options is CSharpParseOptions csharp ? csharp.LanguageVersion : LanguageVersion.Latest)
                             .WithTrackingName("dovetail:language_version");
        var supportedLanguageVersion = languageVersion.Select((version, _) => version >= LanguageVersion.CSharp12);

        context.RegisterSourceOutput(
            languageVersion,
            static (productionContext, version) =>
            {
                if (version < LanguageVersion.CSharp12)
                {
                    productionContext.ReportDiagnostic(Diagnostic.Create(Diagnostics.UnsupportedLanguageVersion, Location.None, version.ToDisplayString()));
                }
            }
        );

        context.RegisterSourceOutput(
            msBuildConfig
               .Combine(context.CompilationProvider)
               .Combine(exportedDovetails)
               .Combine(supportedLanguageVersion),
            static (productionContext, tuple) =>
            {
                if (!tuple.Right) return;
                ExportDovetails.HandleDovetailExports(productionContext, new(tuple.Left.Left.Left, tuple.Left.Right));
            }
        );


        context.RegisterSourceOutput(
            context
               .CompilationProvider
               .Combine(exportedDovetails)
               .Combine(msBuildConfig)
               .Combine(supportedLanguageVersion),
            static (productionContext, tuple) =>
            {
                if (!tuple.Right) return;
                ImportDovetails.HandleDovetailImports(productionContext, new(tuple.Left.Left.Left, tuple.Left.Right, tuple.Left.Left.Right));
            }
        );
    }
}
