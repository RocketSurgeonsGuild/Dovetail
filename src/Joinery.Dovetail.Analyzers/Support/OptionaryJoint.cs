using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;

namespace Joinery.Dovetail.Support;

/// <summary>
///     The joint that registers this assembly's Optionary options. Optionary's generator emits
///     <c>OptionaryConfigurationOptions.AddOptionaryConfigurationOptions(services)</c> in the project's <c>RootNamespace</c> whenever
///     Optionary is referenced; this joint, a private class nested in the exports class, calls it when the assembly declares at least
///     one <c>[Optionary]</c> type. The two generators agree only on those names, so neither reads the other's output.
/// </summary>
/// <remarks>
///     The attribute names, the registration class and its method are a cross-repository contract with Optionary. A match counts only
///     when the attribute comes from an assembly named <c>Optionary</c>, so a look-alike attribute elsewhere doesn't add a joint that
///     calls a method nobody emits.
/// </remarks>
internal static class OptionaryJoint
{
    public const string JointClassName = "OptionaryConfigurationOptionsJoint";

    private const string OptionaryAssemblyName = "Optionary";
    private const string AttributeMetadataName = "Optionary.OptionaryAttribute";
    private const string GenericAttributeMetadataName = "Optionary.OptionaryAttribute`1";
    private const string RegistrationClassName = "OptionaryConfigurationOptions";
    private const string RegistrationMethodName = "AddOptionaryConfigurationOptions";

    /// <summary>Whether the assembly declares any Optionary section, on a class or record or with the assembly form.</summary>
    public static IncrementalValueProvider<bool> HasDeclarations(IncrementalGeneratorInitializationContext context)
    {
        var classes = context
                     .SyntaxProvider
                     .ForAttributeWithMetadataName(
                          AttributeMetadataName,
                          static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                          static (syntaxContext, _) => FromOptionary(syntaxContext)
                      )
                     .Where(static z => z)
                     .Collect()
                     .Select(static (z, _) => !z.IsEmpty);
        var assembly = context
                      .SyntaxProvider
                      .ForAttributeWithMetadataName(
                           GenericAttributeMetadataName,
                           static (node, _) => node is CompilationUnitSyntax,
                           static (syntaxContext, _) => FromOptionary(syntaxContext)
                       )
                      .Where(static z => z)
                      .Collect()
                      .Select(static (z, _) => !z.IsEmpty);
        return classes.Combine(assembly).Select(static (z, _) => z.Left || z.Right).WithTrackingName("dovetail:optionary_declarations");
    }

    /// <summary>Writes the joint class, which calls the registration method in <paramref name="rootNamespace" />.</summary>
    /// <param name="writer">The exports file, at the depth of the exports class's members.</param>
    /// <param name="rootNamespace">The <c>RootNamespace</c> build property, used verbatim; empty or whitespace is the global namespace.</param>
    public static SourceWriter Write(SourceWriter writer, string rootNamespace)
    {
        var registrationClass = string.IsNullOrWhiteSpace(rootNamespace)
            ? $"global::{RegistrationClassName}"
            : $"global::{rootNamespace}.{RegistrationClassName}";
        return writer
              .Summary("Registers this assembly's [Optionary] types with the options pipeline.")
              .Line(CompilerGeneratedAttributes)
              .Line($"private sealed class {JointClassName} : global::Joinery.Dovetail.Joints.IServiceJoint")
              .OpenBlock()
              .Line(
                   "public void Register(global::Joinery.Dovetail.IDovetailContext context, global::Microsoft.Extensions.DependencyInjection.IServiceCollection services) => "
                 + $"{registrationClass}.{RegistrationMethodName}(services);"
               )
              .CloseBlock();
    }

    private static bool FromOptionary(GeneratorAttributeSyntaxContext syntaxContext) =>
        syntaxContext.Attributes.Any(static attribute => attribute.AttributeClass?.OriginalDefinition.ContainingAssembly?.Name == OptionaryAssemblyName);
}
