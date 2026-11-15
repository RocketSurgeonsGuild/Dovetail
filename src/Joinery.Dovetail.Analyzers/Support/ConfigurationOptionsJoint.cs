using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support;

/// <summary>
///     The joint that registers this assembly's configuration options. The configuration generator emits
///     <c>OptionaryConfigurationOptions.AddOptionaryConfigurationOptions(services)</c> in the project's <c>RootNamespace</c> whenever
///     the assembly declares <c>[DovetailConfiguration]</c>; this joint, a private class nested in the exports class, calls it. The
///     two generators agree only on those names and on the declarations they both look for, so neither reads the other's output.
/// </summary>
internal static class ConfigurationOptionsJoint
{
    public const string JointClassName = "DovetailConfigurationOptions";

    private const string AttributeMetadataName = "Joinery.Dovetail.Attributes.DovetailConfigurationAttribute";
    private const string GenericAttributeMetadataName = "Joinery.Dovetail.Attributes.DovetailConfigurationAttribute`1";
    private const string RegistrationClassName = "OptionaryConfigurationOptions";
    private const string RegistrationMethodName = "AddOptionaryConfigurationOptions";

    /// <summary>Whether the assembly declares any configuration section, on a class or record or with the assembly form.</summary>
    public static IncrementalValueProvider<bool> HasDeclarations(IncrementalGeneratorInitializationContext context)
    {
        var classes = context
                     .SyntaxProvider
                     .ForAttributeWithMetadataName(
                          AttributeMetadataName,
                          static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                          static (_, _) => true
                      )
                     .Collect()
                     .Select(static (z, _) => !z.IsEmpty);
        var assembly = context
                      .SyntaxProvider
                      .ForAttributeWithMetadataName(GenericAttributeMetadataName, static (node, _) => node is CompilationUnitSyntax, static (_, _) => true)
                      .Collect()
                      .Select(static (z, _) => !z.IsEmpty);
        return classes.Combine(assembly).Select(static (z, _) => z.Left || z.Right).WithTrackingName("dovetail:configuration_declarations");
    }

    /// <summary>The joint class, which calls the registration method in <paramref name="rootNamespace" />.</summary>
    /// <param name="rootNamespace">The <c>RootNamespace</c> build property, used verbatim; empty or whitespace is the global namespace.</param>
    public static MemberDeclarationSyntax Declaration(string rootNamespace)
    {
        var registrationClass = string.IsNullOrWhiteSpace(rootNamespace) ? $"global::{RegistrationClassName}" : $"global::{rootNamespace}.{RegistrationClassName}";
        var joint = (ClassDeclarationSyntax)ParseMemberDeclaration(
            $$"""
            private sealed class {{JointClassName}} : global::Joinery.Dovetail.Joints.IServiceJoint
            {
                public void Register(global::Joinery.Dovetail.IDovetailContext context, global::Microsoft.Extensions.DependencyInjection.IServiceCollection services) =>
                    {{registrationClass}}.{{RegistrationMethodName}}(services);
            }
            """
        )!;
        return joint.WithAttributeLists(
            SingletonList(CompilerGeneratedAttributes.WithLeadingTrivia(GetXmlSummary("Registers this assembly's [DovetailConfiguration] types with the options pipeline.")))
        );
    }
}
