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
}
