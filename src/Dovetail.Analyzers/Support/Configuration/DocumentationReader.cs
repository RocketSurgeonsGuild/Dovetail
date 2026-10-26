using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Dovetail.Support.Configuration;

/// <summary>
///     Reads <c>&lt;summary&gt;</c> text for configuration types and members.
/// </summary>
/// <remarks>
///     For source symbols it parses the comment text in the declaration's leading trivia. That works whether or not the
///     project generates a documentation file: without one the compiler doesn't parse doc comments, and
///     <see cref="ISymbol.GetDocumentationCommentXml" /> returns nothing. Types from referenced assemblies have no syntax; their
///     docs come from the package's XML documentation file instead (<see cref="ReadDocumentationFile" />).
/// </remarks>
internal static class DocumentationReader
{
    /// <summary>The summary of a source symbol, or <see langword="null" />.</summary>
    public static string? Summary(ISymbol symbol, CancellationToken cancellationToken) =>
        ReadSourceDocumentation(symbol, cancellationToken)?.Element("summary") is { } summary ? Normalize(summary) : null;

    /// <summary>The <c>&lt;param name="…"&gt;</c> text in <paramref name="owner" />'s docs: how positional record parameters are documented.</summary>
    public static string? Parameter(ISymbol owner, string name, CancellationToken cancellationToken) =>
        ReadSourceDocumentation(owner, cancellationToken)
          ?.Elements("param")
           .FirstOrDefault(z => string.Equals((string?)z.Attribute("name"), name, StringComparison.Ordinal)) is { } parameter
            ? Normalize(parameter)
            : null;

    /// <summary>
    ///     Reads the summaries of <paramref name="ids" /> (documentation comment ids such as <c>P:Ns.Type.Property</c>) from an XML
    ///     documentation file. Malformed files yield nothing.
    /// </summary>
    public static Dictionary<string, string> ReadDocumentationFile(string text, ISet<string> ids)
    {
        var summaries = new Dictionary<string, string>(StringComparer.Ordinal);
        try
        {
            var document = XDocument.Parse(text);
            foreach (var member in document.Descendants("member"))
            {
                if ((string?)member.Attribute("name") is { } name && ids.Contains(name) && member.Element("summary") is { } summary && Normalize(summary) is { } value)
                {
                    summaries[name] = value;
                }
            }
        }
        catch (XmlException)
        {
            // A documentation file we can't read only costs descriptions.
        }

        return summaries;
    }

    private static XElement? ReadSourceDocumentation(ISymbol symbol, CancellationToken cancellationToken)
    {
        foreach (var reference in symbol.DeclaringSyntaxReferences)
        {
            var lines = new List<string>();
            foreach (var trivia in reference.GetSyntax(cancellationToken).GetLeadingTrivia())
            {
                var text = trivia.ToFullString();
                if (trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || ( trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) && text.StartsWith("///", StringComparison.Ordinal) ))
                {
                    lines.AddRange(
                        text.Split('\n')
                            .Select(z => z.Trim())
                            .Where(z => z.StartsWith("///", StringComparison.Ordinal))
                            .Select(z => z[3..])
                    );
                }
                // "/**/" is an ordinary empty comment that also starts with "/**": only longer ones have contents.
                else if (text.Length >= 5 && ( trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia) || ( trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) && text.StartsWith("/**", StringComparison.Ordinal) ) ))
                {
                    lines.AddRange(
                        text[3..^2]
                            .Split('\n')
                            .Select(z => z.Trim().TrimStart('*'))
                    );
                }
            }

            if (lines.Count == 0)
            {
                continue;
            }

            try
            {
                return XElement.Parse("<doc>" + string.Join("\n", lines) + "</doc>", LoadOptions.None);
            }
            catch (XmlException)
            {
                return null;
            }
        }

        return null;
    }

    /// <summary>The element's text with references replaced by readable names and whitespace collapsed.</summary>
    private static string? Normalize(XElement element)
    {
        var copy = new XElement(element);
        foreach (var reference in copy.Descendants().Where(z => z.Name.LocalName is "see" or "seealso" or "paramref" or "typeparamref").ToList())
        {
            reference.ReplaceWith(
                (string?)reference.Attribute("langword")
             ?? (string?)reference.Attribute("name")
             ?? ShortName((string?)reference.Attribute("cref"))
             ?? reference.Value
            );
        }

        foreach (var lineBreak in copy.Descendants().Where(z => z.Name.LocalName is "br" or "para").ToList())
        {
            lineBreak.AddBeforeSelf(" ");
            lineBreak.AddAfterSelf(" ");
        }

        var text = Regex.Replace(copy.Value, @"\s+", " ").Trim();
        return text.Length == 0 ? null : text;
    }

    /// <summary>
    ///     <c>T:Ns.Type</c> → <c>Type</c>; <c>M:Ns.Type.Member(System.String)</c> → <c>Type.Member</c>; an unprefixed cref (as written
    ///     in source) is kept.
    /// </summary>
    private static string? ShortName(string? cref)
    {
        if (cref is null)
        {
            return null;
        }

        if (cref.Length < 2 || cref[1] != ':')
        {
            return cref;
        }

        var kind = cref[0];
        var name = cref[2..];
        var parenthesis = name.IndexOf('(');
        if (parenthesis >= 0)
        {
            name = name[..parenthesis];
        }

        var segments = Regex.Replace(name, @"`+\d+", "").Split('.');
        return kind == 'T' || segments.Length < 2 ? segments[^1] : segments[^2] + "." + segments[^1];
    }
}
