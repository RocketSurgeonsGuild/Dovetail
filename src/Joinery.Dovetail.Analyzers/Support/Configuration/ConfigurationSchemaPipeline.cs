using System.Collections.Immutable;
using System.Text;
using Joinery.Dovetail.Support.Configuration.Binding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     Describes every <c>[DovetailConfiguration]</c> section as a JSON Schema fragment, emitted as
///     <c>[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:&lt;Section&gt;", json)]</c>. The build merges the fragments of an
///     application and everything it references into the application's schema.
/// </summary>
internal static class ConfigurationSchemaPipeline
{
    public const string AttributeMetadataName = "Joinery.Dovetail.Attributes.DovetailConfigurationAttribute";
    public const string GenericAttributeMetadataName = "Joinery.Dovetail.Attributes.DovetailConfigurationAttribute`1";
    public const string MetadataPrefix = "Dovetail.ConfigurationSchema.v1:";

    /// <summary>Marks the XML documentation files of package references, which Dovetail's targets pass in as additional files.</summary>
    private const string ReferenceDocumentationMetadata = "build_metadata.AdditionalFiles.DovetailReferenceDocumentation";

    /// <summary>Registers the schema and options outputs; the result says whether this assembly exports an options joint.</summary>
    public static IncrementalValueProvider<bool> Register(
        IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<bool> supportedLanguageVersion,
        IncrementalValueProvider<MsBuildConfig> msBuildConfig
    )
    {
        var classDeclarations = context
                               .SyntaxProvider
                               .ForAttributeWithMetadataName(
                                    AttributeMetadataName,
                                    static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                                    static (syntaxContext, cancellationToken) => syntaxContext
                                                                               .Attributes
                                                                               .Select(
                                                                                    z => ModelBuilder.Build(
                                                                                        z,
                                                                                        (INamedTypeSymbol)syntaxContext.TargetSymbol,
                                                                                        syntaxContext.SemanticModel.Compilation,
                                                                                        cancellationToken
                                                                                    )
                                                                                )
                                                                               .ToEquatableArray()
                                )
                               .WithTrackingName("dovetail:configuration_classes");

        // Assembly attributes target the compilation unit: [assembly: DovetailConfiguration<T>("Section")].
        var assemblyDeclarations = context
                                  .SyntaxProvider
                                  .ForAttributeWithMetadataName(
                                       GenericAttributeMetadataName,
                                       static (node, _) => node is CompilationUnitSyntax,
                                       static (syntaxContext, cancellationToken) => syntaxContext
                                                                                  .Attributes
                                                                                  .Select(z => ModelBuilder.Build(z, null, syntaxContext.SemanticModel.Compilation, cancellationToken))
                                                                                  .ToEquatableArray()
                                   )
                                  .WithTrackingName("dovetail:configuration_assembly");

        var declarations = classDeclarations
                          .Collect()
                          .Combine(assemblyDeclarations.Collect())
                          .Select(static (pair, _) => RejectRepeatedTypes(pair.Left.Concat(pair.Right).SelectMany(z => z)).ToEquatableArray());

        // Sections carry no source positions, so an edit that only moves code leaves them equal and everything after cached.
        var sections = declarations.Select(static (declarations, _) => declarations.Select(z => z.Section).OfType<ConfigurationSection>().ToEquatableArray());
        var diagnostics = declarations.Select(static (declarations, _) => declarations.SelectMany(z => z.Diagnostics).Distinct().ToEquatableArray());
        var options = declarations
                     .Select(static (declarations, _) => declarations.Select(z => z.Options).OfType<ConfigurationOptions>().ToEquatableArray())
                     .WithTrackingName("dovetail:configuration_options");

        // [OptionsValidator] classes for the options types are registered with them.
        var validators = context
                        .SyntaxProvider
                        .ForAttributeWithMetadataName(
                             "Microsoft.Extensions.Options.OptionsValidatorAttribute",
                             static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                             static (syntaxContext, _) => ConfigurationOptionsEmitter.ReadValidator((INamedTypeSymbol)syntaxContext.TargetSymbol, syntaxContext.SemanticModel.Compilation)
                         )
                        .Where(static z => z is not null)
                        .Select(static (z, _) => z!)
                        .Collect()
                        .Select(static (z, _) => z.OrderBy(validator => validator.TypeName, StringComparer.Ordinal).ToEquatableArray());

        var referenceDocumentation = context
                                    .AdditionalTextsProvider
                                    .Combine(context.AnalyzerConfigOptionsProvider)
                                    .Where(
                                         static pair => pair.Right.GetOptions(pair.Left).TryGetValue(ReferenceDocumentationMetadata, out var value)
                                          && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                                     )
                                    .Select(static (pair, _) => pair.Left)
                                    .Collect();

        var described = sections
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

        context.RegisterSourceOutput(
            options.Combine(validators).Combine(msBuildConfig).Combine(supportedLanguageVersion),
            static (production, input) =>
            {
                if (input.Right
                 && ConfigurationOptionsEmitter.Emit(input.Left.Right.ExportConfiguration, input.Left.Left.Left, input.Left.Left.Right) is { } source)
                {
                    production.AddSource("Dovetail_ConfigurationOptions.g.cs", source);
                }
            }
        );

        // Reported against the compilation's syntax trees, so #pragma warning disable works. This step re-runs on every edit,
        // but all it does is report.
        context.RegisterSourceOutput(
            diagnostics.Combine(context.CompilationProvider),
            static (production, input) =>
            {
                foreach (var diagnostic in input.Left)
                {
                    production.ReportDiagnostic(diagnostic.ToDiagnostic(input.Right));
                }
            }
        );

        return options.Select(static (options, _) => !options.IsEmpty);
    }

    /// <summary>
    ///     A type has one section. The compiler already rejects the same attribute twice for one type; this catches the class form
    ///     and the assembly form naming the same type. The class declaration is kept, and the assembly one is replaced by a
    ///     declaration that only reports DT0015, so it produces no section, registration or schema fragment.
    /// </summary>
    /// <remarks>
    ///     The generated binder exists once for each type, so it can read only one set of keys for it. A later declaration whose
    ///     binder would read different keys for a type an earlier one already binds (the same nested type under another
    ///     <c>KeyStyle</c>) is replaced the same way, by one that only reports DT0016.
    /// </remarks>
    private static IEnumerable<ConfigurationDeclaration> RejectRepeatedTypes(IEnumerable<ConfigurationDeclaration> declarations)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var boundKeys = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var declaration in declarations)
        {
            if (declaration.TypeName is not null && !seen.Add(declaration.TypeName))
            {
                yield return new(null, new[] { DiagnosticInfo.Create(Diagnostics.ConfigurationTypeDeclaredTwice, null, declaration.TypeName) with { Location = declaration.Location } }.ToEquatableArray());
                continue;
            }

            if (declaration is { Options: { } options } && ConflictingKeyType(options, boundKeys) is { } conflict)
            {
                yield return new(null, new[] { DiagnosticInfo.Create(Diagnostics.ConfigurationKeyStyleConflict, null, conflict) with { Location = declaration.Location } }.ToEquatableArray());
                continue;
            }

            yield return declaration;
        }
    }

    /// <summary>
    ///     The first object type of <paramref name="options" /> the binder already reads under different keys, or
    ///     <see langword="null" />. Otherwise the keys of its types are recorded.
    /// </summary>
    private static string? ConflictingKeyType(ConfigurationOptions options, Dictionary<string, string> boundKeys)
    {
        var keys = options
                  .Types
                  .OfType<ObjectSpec>()
                  .Select(z => (z.FullName, Name: z.TypeRef.FullyQualifiedName, Keys: string.Join("\n", z.Properties.Select(property => property.Name + "=" + property.ConfigurationKeyName))))
                  .ToArray();
        foreach (var (fullName, name, signature) in keys)
        {
            if (boundKeys.TryGetValue(name, out var existing) && !string.Equals(existing, signature, StringComparison.Ordinal))
            {
                return fullName;
            }
        }

        foreach (var (_, name, signature) in keys)
        {
            boundKeys[name] = signature;
        }

        return null;
    }

    /// <summary>
    ///     Fills in descriptions for types from referenced assemblies, from their XML documentation files. Only the files of the
    ///     assemblies that are actually needed are read.
    /// </summary>
    private static EquatableArray<ConfigurationSection> ResolveDocumentation(
        EquatableArray<ConfigurationSection> sections,
        ImmutableArray<AdditionalText> files,
        CancellationToken cancellationToken
    )
    {
        var wanted = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var section in sections)
        {
            Collect(section.Schema, wanted);
            foreach (var definition in section.Definitions)
            {
                Collect(definition.Schema, wanted);
            }
        }

        if (wanted.Count == 0 || files.IsEmpty)
        {
            return sections;
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
            ? sections
            : sections
             .Select(
                  z => z with
                  {
                      Schema = Apply(z.Schema, summaries),
                      Definitions = z.Definitions.Select(definition => definition with { Schema = Apply(definition.Schema, summaries) }).ToEquatableArray(),
                  }
              )
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

    private static void Emit(SourceProductionContext production, EquatableArray<ConfigurationSection> sections, bool includeRequired, bool supportedLanguageVersion)
    {
        if (!supportedLanguageVersion || sections.IsEmpty)
        {
            return;
        }

        var ordered = sections
                     .OrderBy(z => z.Path, StringComparer.OrdinalIgnoreCase)
                     .ThenBy(z => z.TypeName, StringComparer.Ordinal);
        var unit = CompilationUnit()
                  .WithAttributeLists(List(ordered.Select(z => Helpers.AddAssemblyAttribute(MetadataPrefix + z.Path, JsonSchemaWriter.Write(z, includeRequired)))))
                  .AddSharedTrivia();
        production.AddSource("Dovetail_ConfigurationSchema.g.cs", unit.NormalizeWhitespace().GetText(Encoding.UTF8));
    }
}
