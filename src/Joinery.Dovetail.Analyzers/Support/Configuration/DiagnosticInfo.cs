using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     A diagnostic as plain data: unlike <see cref="Diagnostic" /> and <see cref="Location" /> it compares by content, so it can
///     travel through the incremental pipeline without defeating caching.
/// </summary>
internal sealed record DiagnosticInfo(DiagnosticDescriptor Descriptor, LocationInfo? Location, EquatableArray<string> Arguments)
{
    public static DiagnosticInfo Create(DiagnosticDescriptor descriptor, Location? location, params string[] arguments) =>
        new(descriptor, LocationInfo.From(location), arguments.ToEquatableArray());

    /// <summary>The diagnostic, located in <paramref name="compilation" />'s syntax tree so <c>#pragma warning disable</c> applies.</summary>
    public Diagnostic ToDiagnostic(Compilation compilation) =>
        Diagnostic.Create(Descriptor, Location?.ToLocation(compilation) ?? Microsoft.CodeAnalysis.Location.None, [.. Arguments]);
}

internal sealed record LocationInfo(string FilePath, TextSpan Span, LinePositionSpan LineSpan)
{
    public static LocationInfo? From(Location? location) =>
        location is { IsInSource: true, SourceTree: { } tree } ? new(tree.FilePath, location.SourceSpan, location.GetLineSpan().Span) : null;

    public Location ToLocation(Compilation compilation) =>
        compilation.SyntaxTrees.FirstOrDefault(z => string.Equals(z.FilePath, FilePath, StringComparison.Ordinal)) is { } tree
            ? Location.Create(tree, Span)
            : Location.Create(FilePath, Span, LineSpan);
}
