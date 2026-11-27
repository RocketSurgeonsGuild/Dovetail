using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Joinery.Dovetail;

internal static class Helpers
{
    public static string GetFullMetadataName(ISymbol? s)
    {
        if (s == null || isRootNamespace(s)) return string.Empty;

        var sb = new StringBuilder(s.MetadataName);
        var last = s;

        s = s.ContainingSymbol;

        while (!isRootNamespace(s))
        {
            if (s is ITypeSymbol && last is ITypeSymbol)
                sb.Insert(0, '+');
            else
                sb.Insert(0, '.');

            sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            //sb.Insert(0, s.MetadataName);
            s = s.ContainingSymbol;
        }

        return sb.ToString();

        static bool isRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol? s;
            return ( s = symbol as INamespaceSymbol ) != null && s.IsGlobalNamespace;
        }
    }

    public static IEnumerable<INamedTypeSymbol> GetBaseTypes(Compilation compilation, INamedTypeSymbol namedTypeSymbol)
    {
        while (namedTypeSymbol.BaseType != null)
        {
            if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol.BaseType, compilation.ObjectType)) yield break;
            yield return namedTypeSymbol.BaseType;
            namedTypeSymbol = namedTypeSymbol.BaseType;
        }
    }

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

    public static bool IsOpenGenericType(this INamedTypeSymbol type) => type.IsGenericType && ( type.IsUnboundGenericType || type.TypeArguments.All(z => z.TypeKind == TypeKind.TypeParameter) );

    public static string GetGenericDisplayName(ISymbol? symbol)
    {
        if (symbol == null || IsRootNamespace(symbol)) return string.Empty;

        var sb = new StringBuilder(symbol.MetadataName);
        if (symbol is INamedTypeSymbol namedTypeSymbol && ( namedTypeSymbol.IsOpenGenericType() || namedTypeSymbol.IsGenericType ))
        {
            sb = new(symbol.Name);
            if (namedTypeSymbol.IsOpenGenericType())
            {
                sb.Append('<');
                for (var i = 1; i < namedTypeSymbol.Arity; i++)
                {
                    sb.Append(',');
                }

                sb.Append('>');
            }
            else
            {
                sb.Append('<');
                for (var index = 0; index < namedTypeSymbol.TypeArguments.Length; index++)
                {
                    var argument = namedTypeSymbol.TypeArguments[index];
                    sb.Append(GetGenericDisplayName(argument));
                    if (index < namedTypeSymbol.TypeArguments.Length - 1)
                        sb.Append(',');
                }

                sb.Append('>');
            }
        }

        var last = symbol;

        var workingSymbol = symbol.ContainingSymbol;

        while (!IsRootNamespace(workingSymbol))
        {
            if (workingSymbol is ITypeSymbol && last is ITypeSymbol)
                sb.Insert(0, '+');
            else
                sb.Insert(0, '.');

            sb.Insert(0, workingSymbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Trim());
            //sb.Insert(0, symbol.MetadataName);
            workingSymbol = workingSymbol.ContainingSymbol;
        }

        sb.Insert(0, "global::");
        return sb.ToString();

        static bool IsRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol? s;
            return ( s = symbol as INamespaceSymbol ) != null && s.IsGlobalNamespace;
        }
    }

    public static string GetTypeOfName(ISymbol? symbol)
    {
        if (symbol == null || IsRootNamespace(symbol)) return string.Empty;

        var sb = new StringBuilder(symbol.MetadataName);
        if (symbol is INamedTypeSymbol namedTypeSymbol && ( namedTypeSymbol.IsOpenGenericType() || namedTypeSymbol.IsGenericType ))
        {
            sb = new(symbol.Name);
            if (namedTypeSymbol.IsOpenGenericType())
            {
                sb.Append('<');
                for (var i = 1; i < namedTypeSymbol.Arity; i++)
                {
                    sb.Append(',');
                }

                sb.Append('>');
            }
            else
            {
                sb.Append('<');
                for (var index = 0; index < namedTypeSymbol.TypeArguments.Length; index++)
                {
                    var argument = namedTypeSymbol.TypeArguments[index];
                    sb.Append(GetGenericDisplayName(argument));
                    if (index < namedTypeSymbol.TypeArguments.Length - 1)
                        sb.Append(',');
                }

                sb.Append('>');
            }
        }

        var last = symbol;

        var workingSymbol = symbol.ContainingSymbol;

        while (!IsRootNamespace(workingSymbol))
        {
            if (workingSymbol is ITypeSymbol && last is ITypeSymbol)
                sb.Insert(0, '.');
            else
                sb.Insert(0, '.');

            sb.Insert(0, workingSymbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Trim());
            //sb.Insert(0, symbol.MetadataName);
            workingSymbol = workingSymbol.ContainingSymbol;
        }

        sb.Insert(0, "global::");
        return sb.ToString();

        static bool IsRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol? s;
            return ( s = symbol as INamespaceSymbol ) != null && s.IsGlobalNamespace;
        }
    }

    public static INamedTypeSymbol? GetUnboundGenericType(INamedTypeSymbol symbol)
    {
        return symbol switch
        {
            { IsGenericType: true, IsUnboundGenericType: true, } => symbol,
            { IsGenericType: true, } => symbol.ConstructUnboundGenericType(),
            _ => default,
        };
    }

    public static bool HasImplicitGenericConversion(
        Compilation compilation,
        INamedTypeSymbol assignableToType,
        INamedTypeSymbol assignableFromType
    )
    {
        if (SymbolEqualityComparer.Default.Equals(compilation.ObjectType, assignableToType)) return false;
        if (SymbolEqualityComparer.Default.Equals(compilation.ObjectType, assignableFromType)) return false;
        if (SymbolEqualityComparer.Default.Equals(assignableToType, assignableFromType))
            return true;
        if (compilation.HasImplicitConversion(assignableFromType, assignableToType)) return true;
        if (assignableToType is not { Arity: > 0, IsUnboundGenericType: true, }) return false;

        var matchingBaseTypes = GetBaseTypes(compilation, assignableFromType)
                               .Select(GetUnboundGenericType)
                               .Where(symbol => symbol is { } && compilation.HasImplicitConversion(symbol, assignableToType));
        if (matchingBaseTypes.Any()) return true;

        var matchingInterfaces = assignableFromType
                                .AllInterfaces
                                .Select(GetUnboundGenericType)
                                .Where(symbol => symbol is { } && compilation.HasImplicitConversion(symbol, assignableToType));
        return matchingInterfaces.Any();
    }


    /// <summary>The attributes on every generated type.</summary>
    internal static readonly string CompilerGeneratedAttributes =
        $"[System.CodeDom.Compiler.GeneratedCode({StringLiteral(AssemblyInfo.Project.AssemblyName)}, {StringLiteral(AssemblyInfo.Attributes.FileVersion)}), "
      + "System.Runtime.CompilerServices.CompilerGenerated, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

    /// <summary>An <c>[assembly: AssemblyMetadata(key, value)]</c> attribute.</summary>
    internal static string AssemblyMetadataAttribute(string key, string? value) =>
        $"[assembly: System.Reflection.AssemblyMetadata({StringLiteral(key)}, {( value is null ? "null" : StringLiteral(value) )})]";
}
