using Microsoft.CodeAnalysis.CSharp;

namespace Joinery.Dovetail;

internal static class Helpers
{
    /// <summary>The warnings every generated file disables. Declared before the trivia built from it, which reads it while initializing.</summary>
    private static readonly string[] _disabledWarnings =
    [
        "CA1002",
        "CA1034",
        "CA1822",
        "CS0105",
        "CS1573",
        "CS8602",
        "CS8603",
        "CS8618",
        "CS8669",
    ];

    /// <summary>Writes the directives at the top of every generated file: nullable annotations on, and the shared warnings off.</summary>
    public static SourceWriter WriteSharedHeader(this SourceWriter writer) =>
        writer.Line("#nullable enable").Line(DisableSharedWarnings);

    /// <summary>Writes the directives at the end of every generated file, which undo <see cref="WriteSharedHeader" />.</summary>
    public static SourceWriter WriteSharedFooter(this SourceWriter writer) =>
        writer.Line(RestoreSharedWarnings).Line("#nullable restore");

    private static readonly string DisableSharedWarnings = $"#pragma warning disable {string.Join(", ", _disabledWarnings)}";
    private static readonly string RestoreSharedWarnings = $"#pragma warning restore {string.Join(", ", _disabledWarnings)}";

    /// <summary><paramref name="value" /> as a C# string literal.</summary>
    public static string StringLiteral(string value) => SymbolDisplay.FormatLiteral(value, quote: true);

    /// <summary>The attributes on every generated type.</summary>
    internal static readonly string CompilerGeneratedAttributes =
        $"[System.CodeDom.Compiler.GeneratedCode({StringLiteral(AssemblyInfo.Project.AssemblyName)}, {StringLiteral(AssemblyInfo.Attributes.FileVersion)}), "
      + "System.Runtime.CompilerServices.CompilerGenerated, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

    /// <summary>An <c>[assembly: AssemblyMetadata(key, value)]</c> attribute.</summary>
    internal static string AssemblyMetadataAttribute(string key, string? value) =>
        $"[assembly: System.Reflection.AssemblyMetadata({StringLiteral(key)}, {( value is null ? "null" : StringLiteral(value) )})]";
}
