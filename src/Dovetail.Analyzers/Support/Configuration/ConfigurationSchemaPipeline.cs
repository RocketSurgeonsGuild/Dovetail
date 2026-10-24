using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Dovetail.Support.Configuration;

/// <summary>
///     Describes every <c>[DovetailConfiguration]</c> section as a JSON Schema fragment, emitted as
///     <c>[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:&lt;Section&gt;", json)]</c>. The build merges the fragments of an
///     application and everything it references into the application's schema.
/// </summary>
internal static class ConfigurationSchemaPipeline
{
    public const string AttributeMetadataName = "Dovetail.Attributes.DovetailConfigurationAttribute";
    public const string MetadataPrefix = "Dovetail.ConfigurationSchema.v1:";

    /// <summary>Marks the XML documentation files of package references, which Dovetail's targets pass in as additional files.</summary>
    private const string ReferenceDocumentationMetadata = "build_metadata.AdditionalFiles.DovetailReferenceDocumentation";

    public static void Register(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<bool> supportedLanguageVersion)
    {
        var classDeclarations = context
                               .SyntaxProvider
                               .ForAttributeWithMetadataName(
                                    AttributeMetadataName,
                                    static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                                    static (syntaxContext, cancellationToken) => syntaxContext
                                                                               .Attributes
                                                                               .Select(z => ModelBuilder.Build(z, (INamedTypeSymbol)syntaxContext.TargetSymbol, cancellationToken))
                                                                               .ToEquatableArray()
                                )
                               .WithTrackingName("dovetail:configuration_classes");

        // Assembly attributes target the compilation unit: [assembly: DovetailConfiguration("Section", typeof(T))].
        var assemblyDeclarations = context
                                  .SyntaxProvider
                                  .ForAttributeWithMetadataName(
                                       AttributeMetadataName,
                                       static (node, _) => node is CompilationUnitSyntax,
                                       static (syntaxContext, cancellationToken) => syntaxContext
                                                                                  .Attributes
                                                                                  .Select(z => ModelBuilder.Build(z, null, cancellationToken))
                                                                                  .ToEquatableArray()
                                   )
                                  .WithTrackingName("dovetail:configuration_assembly");

        var declarations = classDeclarations
                          .Collect()
                          .Combine(assemblyDeclarations.Collect())
                          .Select(static (pair, _) => pair.Left.Concat(pair.Right).SelectMany(z => z).ToEquatableArray());

        var referenceDocumentation = context
                                    .AdditionalTextsProvider
                                    .Combine(context.AnalyzerConfigOptionsProvider)
                                    .Where(
                                         static pair => pair.Right.GetOptions(pair.Left).TryGetValue(ReferenceDocumentationMetadata, out var value)
                                          && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                                     )
                                    .Select(static (pair, _) => pair.Left)
                                    .Collect();

        var described = declarations
                       .Combine(referenceDocumentation)
                       .Select(static (pair, cancellationToken) => ResolveDocumentation(pair.Left, pair.Right, cancellationToken))
                       .WithTrackingName("dovetail:configuration_sections");

        var includeRequired = context.AnalyzerConfigOptionsProvider.Select(
            static (options, _) => options.GlobalOptions.GetBuildProperty("DovetailConfigurationSchemaRequired", z => bool.TryParse(z, out var value) && value)
        );

        context.RegisterSourceOutput(
            described.Combine(includeRequired).Combine(supportedLanguageVersion),
            static (production, input) => Emit(production, input.Left.Left, input.Left.Right, input.Right)
        );
    }

    /// <summary>
    ///     Fills in descriptions for types from referenced assemblies, from their XML documentation files. Only the files of the
    ///     assemblies that are actually needed are read.
    /// </summary>
    private static EquatableArray<ConfigurationDeclaration> ResolveDocumentation(
        EquatableArray<ConfigurationDeclaration> declarations,
        ImmutableArray<AdditionalText> files,
        CancellationToken cancellationToken
    )
    {
        var wanted = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var declaration in declarations)
        {
            if (declaration.Section is { } section)
            {
                Collect(section.Schema, wanted);
            }
        }

        if (wanted.Count == 0 || files.IsEmpty)
        {
            return declarations;
        }

        var summaries = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in wanted)
        {
            var file = files.FirstOrDefault(z => string.Equals(FileNameWithoutExtension(z.Path), pair.Key, StringComparison.OrdinalIgnoreCase));
            if (file?.GetText(cancellationToken)?.ToString() is not { } text)
            {
                continue;
            }

            foreach (var summary in DocumentationReader.ReadDocumentationFile(text, pair.Value))
            {
                summaries[pair.Key + "|" + summary.Key] = summary.Value;
            }
        }

        return summaries.Count == 0
            ? declarations
            : declarations
             .Select(z => z.Section is { } section ? z with { Section = section with { Schema = Apply(section.Schema, summaries) } } : z)
             .ToEquatableArray();
    }

    private static void Collect(SchemaNode node, Dictionary<string, HashSet<string>> wanted)
    {
        if (node is { DocumentationId: { } id, DocumentationAssembly: { } assembly })
        {
            if (!wanted.TryGetValue(assembly, out var ids))
            {
                // A collection expression can't take the comparer here: the analyzer also builds against Roslyn 5.0 (C# 14).
#pragma warning disable IDE0028
                wanted[assembly] = ids = new(StringComparer.Ordinal);
#pragma warning restore IDE0028
            }

            ids.Add(id);
        }

        if (node.Items is { } items)
        {
            Collect(items, wanted);
        }

        if (node.Values is { } values)
        {
            Collect(values, wanted);
        }

        foreach (var property in node.Properties)
        {
            Collect(property.Schema, wanted);
        }
    }

    private static SchemaNode Apply(SchemaNode node, Dictionary<string, string> summaries) => node with
    {
        Description = node is { Description: null, DocumentationId: { } id, DocumentationAssembly: { } assembly } && summaries.TryGetValue(assembly + "|" + id, out var summary)
            ? summary
            : node.Description,
        Items = node.Items is { } items ? Apply(items, summaries) : null,
        Values = node.Values is { } values ? Apply(values, summaries) : null,
        Properties = node.Properties.Select(z => z with { Schema = Apply(z.Schema, summaries) }).ToEquatableArray(),
    };

    private static string FileNameWithoutExtension(string path)
    {
        var start = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\')) + 1;
        var end = path.LastIndexOf('.');
        return end > start ? path[start..end] : path[start..];
    }

    private static void Emit(SourceProductionContext production, EquatableArray<ConfigurationDeclaration> declarations, bool includeRequired, bool supportedLanguageVersion)
    {
        // A nested type used by several sections reports its problems once per section; say each thing once.
        foreach (var diagnostic in new HashSet<DiagnosticInfo>(declarations.SelectMany(z => z.Diagnostics)))
        {
            production.ReportDiagnostic(diagnostic.ToDiagnostic());
        }

        if (!supportedLanguageVersion)
        {
            return;
        }

        var sections = declarations
                      .Select(z => z.Section)
                      .OfType<ConfigurationSection>()
                      .OrderBy(z => z.Path, StringComparer.OrdinalIgnoreCase)
                      .ThenBy(z => z.Order)
                      .ThenBy(z => z.TypeName, StringComparer.Ordinal)
                      .ToArray();
        if (sections.Length == 0)
        {
            return;
        }

        var unit = CompilationUnit()
                  .WithAttributeLists(List(sections.Select(z => Helpers.AddAssemblyAttribute(MetadataPrefix + z.Path, JsonSchemaWriter.Write(z, includeRequired)))))
                  .AddSharedTrivia();
        production.AddSource("Dovetail_ConfigurationSchema.g.cs", unit.NormalizeWhitespace().GetText(Encoding.UTF8));
    }
}
