using Microsoft.CodeAnalysis;

namespace Dovetail;

internal static class Diagnostics
{
    public static readonly DiagnosticDescriptor DovetailHasToManyConstructors = new(
        "DT0001",
        "Joint has too Many Constructors",
        "Joints only allow one (or the default) constructor",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor DovetailCannotBeGeneric = new(
        "DT0002",
        "Joint cannot be generic",
        "Joints cannot be generic types",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor MustBeTypeOf = new(
        "DT0003",
        "Must use typeof",
        "Method must be called with typeof, variables are not allowed",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor UnhandledSymbol = new(
        "DT0004",
        "Symbol could not be handled",
        "The indicated symbol could not be handled correctly",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor CircularJointDependency = new(
        "DT0005",
        "Circular joint dependency",
        "Joint '{0}' participates in a circular dependency chain ({1}) and cannot be ordered",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor UnsupportedLanguageVersion = new(
        "DT0006",
        "C# version too old for Dovetail source generation",
        "Dovetail's source generator needs C# 12 or later, but this project uses C# {0}, so no Dovetail code was generated; set LangVersion to 12 or later to enable it",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor InvalidConfigurationType = new(
        "DT0007",
        "Invalid configuration type",
        "'{0}' can't be described as configuration: {1}",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor InvalidConfigurationSection = new(
        "DT0008",
        "Invalid configuration section",
        "'{0}' isn't a valid configuration section for '{1}': {2}",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor UnsupportedConfigurationMember = new(
        "DT0009",
        "Unsupported configuration member",
        "'{0}' has type '{1}', which configuration binding doesn't support, so it's left out of the schema",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor ConfigurationKeyCollision = new(
        "DT0010",
        "Configuration key collision",
        "'{0}' and '{1}' both bind the key '{2}' (configuration keys are case-insensitive), so only '{0}' is described",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor UnsupportedConfigurationConstructor = new(
        "DT0011",
        "Configuration type can't be constructed",
        "'{0}' can't be created from configuration because {1}",
        "Dovetail",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor ConfigurationTypeTooLarge = new(
        "DT0012",
        "Configuration type too large",
        "'{0}' uses more than {1} distinct types, so the schema stops describing them at '{2}'",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor InexpressibleConfigurationAnnotation = new(
        "DT0013",
        "Annotation not expressible in JSON Schema",
        "'{0}' on '{1}' is left out of the schema because {2}",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor ConfigurationSchemaFailed = new(
        "DT0014",
        "Configuration schema generation failed",
        "Dovetail couldn't describe '{0}' as configuration ({1}), so it has no schema; please report this",
        "Dovetail",
        DiagnosticSeverity.Warning,
        true
    );
}
