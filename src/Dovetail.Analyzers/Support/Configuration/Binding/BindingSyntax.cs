// Ported from dotnet/runtime (MIT): the configuration binding generator's parser extensions and the shared source
// generator helpers (CSharpSyntaxUtilities, TypeModelHelper). See BindingModel.cs.

using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Dovetail.Support.Configuration.Binding;

internal static class BindingSyntax
{
    private static readonly SymbolDisplayFormat IdentifierCompatibleFormat = new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.None,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
    );

    private static readonly SymbolDisplayFormat MinimalDisplayFormat = new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
    );

    /// <summary>A C# literal for a parameter's default value.</summary>
    public static string FormatLiteral(object? value, TypeRef type) => value switch
    {
        null => $"default({type.FullyQualifiedName})",
        string text => SymbolDisplay.FormatLiteral(text, quote: true),
        char character => SymbolDisplay.FormatLiteral(character, quote: true),
        double.NegativeInfinity => "double.NegativeInfinity",
        double.PositiveInfinity => "double.PositiveInfinity",
        double.NaN => "double.NaN",
        double number => number.ToString("G17", CultureInfo.InvariantCulture) + "D",
        float.NegativeInfinity => "float.NegativeInfinity",
        float.PositiveInfinity => "float.PositiveInfinity",
        float.NaN => "float.NaN",
        float number => number.ToString("G9", CultureInfo.InvariantCulture) + "F",
        decimal number => number.ToString(CultureInfo.InvariantCulture) + "M",
        bool flag => flag ? "true" : "false",
        _ => $"({type.FullyQualifiedName})({Convert.ToString(value, CultureInfo.InvariantCulture)})",
    };

    /// <summary>Prefixes an identifier with <c>@</c> when it would otherwise be read as a keyword.</summary>
    public static string EscapeIdentifier(string identifier) =>
        SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None || SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None
            ? "@" + identifier
            : identifier;

    /// <summary>The name without the namespace, and a <c>Type.FullName</c>-like rendering.</summary>
    public static (string DisplayString, string FullName) GetTypeNames(this ITypeSymbol type)
    {
        var @namespace = type.ContainingNamespace is { IsGlobalNamespace: false } containingNamespace ? containingNamespace.ToDisplayString() : null;
        var displayString = type.ToDisplayString(MinimalDisplayFormat);
        var fullName = ( @namespace is null ? "" : @namespace + "." ) + displayString.Replace(".", "+");
        return (displayString, fullName);
    }

    /// <summary>
    ///     A unique identifier-safe rendering of the type, with its type arguments: <c>A.B.Type</c> becomes <c>A__B__Type</c>.
    ///     Names past 500 characters are cut and get a hash of the full name.
    /// </summary>
    public static string ToIdentifierCompatibleSubstring(this ITypeSymbol type)
    {
        var builder = new StringBuilder(50);
        AppendIdentifierCompatible(type, builder);
        builder.Replace(".", "__").Replace("[", "").Replace("]", "");
        if (builder.Length > 500)
        {
            // string.GetHashCode is randomized per process; generated code has to be the same on every build.
            var hash = StableHash(builder.ToString()).ToString("D10", CultureInfo.InvariantCulture);
            builder.Remove(490, builder.Length - 490).Append(hash);
        }

        return builder.ToString();
    }

    private static void AppendIdentifierCompatible(ITypeSymbol type, StringBuilder builder)
    {
        if (type is IArrayTypeSymbol arrayType)
        {
            AppendIdentifierCompatible(arrayType.ElementType, builder);
            builder.Append(arrayType.Rank == 1 ? "Array" : $"Array{arrayType.Rank}D");
        }

        builder.Append(type.ToDisplayString(IdentifierCompatibleFormat));
        if (type is not INamedTypeSymbol { IsGenericType: true } namedType || TypeArgumentsInScope(namedType) is not { } arguments)
        {
            return;
        }

        builder.Append("___");
        foreach (var argument in arguments)
        {
            AppendIdentifierCompatible(argument, builder);
        }

        builder.Append("___");
    }

    /// <summary>The type arguments of the type and of the types it's nested in, outermost first.</summary>
    private static List<ITypeSymbol>? TypeArgumentsInScope(INamedTypeSymbol type)
    {
        List<ITypeSymbol>? arguments = null;
        Traverse(type);
        return arguments;

        void Traverse(INamedTypeSymbol current)
        {
            if (current.ContainingType is { } parent)
            {
                Traverse(parent);
            }

            if (!current.TypeArguments.IsEmpty)
            {
                ( arguments ??= [] ).AddRange(current.TypeArguments);
            }
        }
    }

    private static uint StableHash(string text)
    {
        unchecked
        {
            var hash = 2166136261;
            foreach (var character in text)
            {
                hash = ( hash ^ character ) * 16777619;
            }

            return hash;
        }
    }
}
