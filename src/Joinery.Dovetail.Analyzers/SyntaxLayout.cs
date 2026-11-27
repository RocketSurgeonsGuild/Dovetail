using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail;

/// <summary>
///     Lays out the generated code as it is built, so the generator never runs <c>NormalizeWhitespace</c>, which walks and rebuilds
///     the whole tree and was the largest single cost of a run.
/// </summary>
/// <remarks>
///     <para>The layout has three rules, and every builder follows them through these helpers:</para>
///     <list type="bullet">
///         <item>
///             A node that owns a line (a using, a member, a statement) carries its indentation as leading trivia and its line break as
///             trailing trivia: <see cref="OnLine{TNode}" /> and <see cref="Member{TMember}" />.
///         </item>
///         <item>An opening brace starts its own line and ends it; a closing brace only starts its line, and its owner ends it.</item>
///         <item>Within a line, a token that needs separating carries the space: <see cref="SpaceAfter" /> and <see cref="Spaced" />.</item>
///     </list>
///     <para>Changing the indentation, the line break or the spacing of the generated files is a change to this class alone.</para>
/// </remarks>
internal static class SyntaxLayout
{
    /// <summary>The line break the generated files use.</summary>
    public static readonly SyntaxTrivia NewLine = CarriageReturnLineFeed;

    /// <summary>The indentation of one level.</summary>
    private const string IndentUnit = "    ";

    private static readonly SyntaxTrivia[] _indents = [.. Enumerable.Range(1, 8).Select(depth => Whitespace(string.Concat(Enumerable.Repeat(IndentUnit, depth))))];

    /// <summary>The indentation of a line at <paramref name="depth" />, which is nothing at the top level.</summary>
    public static SyntaxTriviaList Indent(int depth) => depth switch
    {
        <= 0 => default,
        _ when depth <= _indents.Length => TriviaList(_indents[depth - 1]),
        _ => TriviaList(Whitespace(string.Concat(Enumerable.Repeat(IndentUnit, depth)))),
    };

    /// <summary>A token followed by a space: a keyword or modifier, or the comma between two items of a list.</summary>
    public static SyntaxToken SpaceAfter(SyntaxKind kind) => Token(default, kind, TriviaList(Space));

    /// <summary>A token with a space on either side: a binary operator, <c>=</c>, <c>=&gt;</c> or the <c>:</c> before a base list.</summary>
    public static SyntaxToken Spaced(SyntaxKind kind) => Token(TriviaList(Space), kind, TriviaList(Space));

    /// <summary>Modifiers, each followed by a space.</summary>
    public static SyntaxTokenList Modifiers(params SyntaxKind[] kinds) => TokenList(kinds.Select(SpaceAfter));

    /// <summary>A list whose items are separated by a comma and a space.</summary>
    public static SeparatedSyntaxList<TNode> CommaSeparated<TNode>(params IEnumerable<TNode> nodes) where TNode : SyntaxNode
    {
        var items = nodes as IReadOnlyCollection<TNode> ?? nodes.ToList();
        return SeparatedList(items, Enumerable.Repeat(SpaceAfter(SyntaxKind.CommaToken), Math.Max(0, items.Count - 1)));
    }

    /// <summary>The opening brace of a body whose owner sits at <paramref name="depth" />, on a line of its own.</summary>
    public static SyntaxToken OpenBrace(int depth) => Token(TriviaList(NewLine).AddRange(Indent(depth)), SyntaxKind.OpenBraceToken, TriviaList(NewLine));

    /// <summary>The closing brace of a body whose owner sits at <paramref name="depth" />; the owner ends the line.</summary>
    public static SyntaxToken CloseBrace(int depth) => Token(Indent(depth), SyntaxKind.CloseBraceToken, default);

    /// <summary>A block at <paramref name="depth" />, whose statements were laid out one level deeper.</summary>
    public static BlockSyntax BlockAt(int depth, params IEnumerable<StatementSyntax> statements) => Block(OpenBrace(depth), List(statements), CloseBrace(depth));

    /// <summary>Puts <paramref name="node" /> on a line of its own at <paramref name="depth" />.</summary>
    public static TNode OnLine<TNode>(this TNode node, int depth) where TNode : SyntaxNode => node.WithLeadingTrivia(Indent(depth)).WithTrailingTrivia(NewLine);

    /// <summary>
    ///     Puts a member on its own lines at <paramref name="depth" />: its summary, then each attribute list, then the declaration.
    /// </summary>
    public static TMember Member<TMember>(this TMember member, int depth, string? summary = null) where TMember : MemberDeclarationSyntax
    {
        var indent = Indent(depth);
        if (member.AttributeLists.Count > 0)
        {
            var afterAttributes = member.AttributeLists[^1].GetLastToken().GetNextToken();
            member = member.ReplaceToken(afterAttributes, afterAttributes.WithLeadingTrivia(indent));
            member = (TMember)member.WithAttributeLists(
                List(member.AttributeLists.Select((list, index) => list.WithLeadingTrivia(index == 0 ? default : indent).WithTrailingTrivia(NewLine)))
            );
        }

        var leading = summary is null ? indent : indent.Add(DocumentationSummary(summary, depth)).AddRange(indent);
        return member.WithLeadingTrivia(leading).WithTrailingTrivia(NewLine);
    }

    /// <summary>
    ///     Lays out the members of a type or namespace at <paramref name="depth" />, with a blank line after a member that ends in a
    ///     closing brace when another member follows it.
    /// </summary>
    public static SyntaxList<MemberDeclarationSyntax> Members(params IEnumerable<MemberDeclarationSyntax> members)
    {
        var laidOut = new List<MemberDeclarationSyntax>();
        foreach (var member in members)
        {
            if (laidOut is [.., var previous] && previous.GetLastToken().IsKind(SyntaxKind.CloseBraceToken))
            {
                laidOut.Add(member.WithLeadingTrivia(member.GetLeadingTrivia().Insert(0, NewLine)));
                continue;
            }

            laidOut.Add(member);
        }

        return List(laidOut);
    }

    /// <summary>A namespace at the top level holding <paramref name="members" />, which were laid out one level in.</summary>
    public static NamespaceDeclarationSyntax InNamespace(string name, params IEnumerable<MemberDeclarationSyntax> members) =>
        NamespaceDeclaration(ParseName(name))
           .WithNamespaceKeyword(SpaceAfter(SyntaxKind.NamespaceKeyword))
           .WithOpenBraceToken(OpenBrace(0))
           .WithMembers(Members(members))
           .WithCloseBraceToken(CloseBrace(0))
           .OnLine(0);

    /// <summary>The end of the file, preceded by <paramref name="trivia" />.</summary>
    public static SyntaxToken EndOfFile(SyntaxTriviaList trivia) => Token(trivia, SyntaxKind.EndOfFileToken, default);

    /// <summary>
    ///     Puts a blank line between the usings and what follows them, then <paramref name="trivia" /> before the first line after them.
    /// </summary>
    public static CompilationUnitSyntax SeparateUsings(this CompilationUnitSyntax compilationUnit, SyntaxTriviaList trivia = default)
    {
        if (compilationUnit.Usings.Count == 0) return compilationUnit;
        var next = compilationUnit.Usings[^1].GetLastToken().GetNextToken();
        return compilationUnit.ReplaceToken(next, next.WithLeadingTrivia(TriviaList(NewLine).AddRange(trivia).AddRange(next.LeadingTrivia)));
    }

    /// <summary>A <c>/// &lt;summary&gt;</c> comment for a member at <paramref name="depth" />, ending with its line break.</summary>
    public static SyntaxTrivia DocumentationSummary(string text, int depth)
    {
        var exterior = string.Concat(Enumerable.Repeat(IndentUnit, Math.Max(0, depth))) + "///";
        return Trivia(
            DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                List<XmlNodeSyntax>(
                    [
                        XmlText(XmlTextLiteral(TriviaList(DocumentationCommentExterior("///")), " ", " ", default)),
                        XmlElement(
                            XmlElementStartTag(XmlName("summary")),
                            SingletonList<XmlNodeSyntax>(
                                XmlText(
                                    XmlDocumentationNewLine,
                                    XmlTextLiteral(TriviaList(DocumentationCommentExterior(exterior)), $" {text}", $" {text}", default),
                                    XmlDocumentationNewLine,
                                    XmlTextLiteral(TriviaList(DocumentationCommentExterior(exterior)), " ", " ", default)
                                )
                            ),
                            XmlElementEndTag(XmlName("summary"))
                        ),
                        XmlText(XmlDocumentationNewLine),
                    ]
                )
            )
        );
    }

    /// <summary>A <c>#nullable</c> directive, ending with its line break.</summary>
    public static SyntaxTrivia NullableDirective(SyntaxKind setting) =>
        Trivia(
            NullableDirectiveTrivia(Token(setting), true)
               .WithNullableKeyword(SpaceAfter(SyntaxKind.NullableKeyword))
               .WithEndOfDirectiveToken(EndOfDirective)
        );

    /// <summary>A <c>#pragma warning</c> directive for <paramref name="codes" />, ending with its line break.</summary>
    public static SyntaxTrivia PragmaWarningDirective(SyntaxKind disableOrRestore, params IEnumerable<string> codes) =>
        Trivia(
            PragmaWarningDirectiveTrivia(SpaceAfter(disableOrRestore), CommaSeparated(codes.Select(ExpressionSyntax (code) => IdentifierName(code))), true)
               .WithPragmaKeyword(SpaceAfter(SyntaxKind.PragmaKeyword))
               .WithWarningKeyword(SpaceAfter(SyntaxKind.WarningKeyword))
               .WithEndOfDirectiveToken(EndOfDirective)
        );

    /// <summary>A using directive on its own line.</summary>
    public static UsingDirectiveSyntax Using(string name) => UsingDirective(ParseName(name)).WithUsingKeyword(SpaceAfter(SyntaxKind.UsingKeyword)).OnLine(0);

    private static SyntaxToken EndOfDirective => Token(default, SyntaxKind.EndOfDirectiveToken, TriviaList(NewLine));

    private static readonly SyntaxToken XmlDocumentationNewLine = XmlTextNewLine(default, "\n", "\n", default);
}
