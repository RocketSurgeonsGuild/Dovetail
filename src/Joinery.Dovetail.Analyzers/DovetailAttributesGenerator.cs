using Joinery.Dovetail.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

        // Each joint is described in the transform, so the pipeline holds equatable models rather than symbols: an edit that
        // doesn't touch a joint leaves the collected models equal, and the exports aren't generated again.
        var exportedDovetails = context
                               .SyntaxProvider
                               .ForAttributeWithMetadataName(
                                    "Joinery.Dovetail.Attributes.DovetailExportAttribute",
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

        // The generated code uses C# 12 syntax (collection expressions). The analyzer reaches projects that only reference
        // Dovetail transitively, so rather than break a project pinned to an older LangVersion, emit nothing and say why.
        var languageVersion = context
                             .ParseOptionsProvider
                             .Select(static (options, _) => options is CSharpParseOptions csharp ? csharp.LanguageVersion : LanguageVersion.Latest)
                             .WithTrackingName("dovetail:language_version");
        var supportedLanguageVersion = languageVersion.Select(static (version, _) => version >= LanguageVersion.CSharp12);

        // Optionary's generator emits the registration method whenever Optionary is referenced; when this assembly declares an
        // [Optionary] type, the exports list a joint that calls it. Found here rather than read from Optionary's generator.
        var hasConfigurationOptions = OptionaryJoint.HasDeclarations(context);

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
               .Combine(exportedDovetails)
               .Combine(hasConfigurationOptions)
               .Combine(supportedLanguageVersion),
            static (productionContext, tuple) =>
            {
                if (!tuple.Right) return;
                ExportDovetails.HandleDovetailExports(productionContext, new(tuple.Left.Left.Left, tuple.Left.Left.Right, tuple.Left.Right));
            }
        );

        context.RegisterSourceOutput(
            referencedExports
               .Combine(exportedDovetails.Select(static (z, _) => z.Count > 0))
               .Combine(msBuildConfig)
               .Combine(hasConfigurationOptions)
               .Combine(supportedLanguageVersion),
            static (productionContext, tuple) =>
            {
                if (!tuple.Right) return;
                ImportDovetails.HandleDovetailImports(
                    productionContext,
                    new(tuple.Left.Left.Right, tuple.Left.Left.Left.Left, tuple.Left.Left.Left.Right, tuple.Left.Right)
                );
            }
        );
    }
}
