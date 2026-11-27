using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Joinery.Dovetail;

/// <summary>
///     Writes a generated file as text, a line at a time, keeping track of the indentation. The layout lives here: the line break,
///     the indentation of one level, the braces of a block and the shape of a <c>/// &lt;summary&gt;</c> comment.
/// </summary>
internal sealed class SourceWriter(int capacity = 4096)
{
    /// <summary>The line break the generated files use.</summary>
    private const string NewLine = "\r\n";

    /// <summary>The line break inside documentation comments, which is what Roslyn's <c>XmlTextNewLine</c> wrote.</summary>
    private const string DocumentationNewLine = "\n";

    /// <summary>The indentation of one level.</summary>
    private const string IndentUnit = "    ";

    private readonly StringBuilder _builder = new(capacity);
    private int _depth;

    /// <summary>Writes <paramref name="text" /> on a line of its own at the current depth.</summary>
    public SourceWriter Line(string text) => StartLine().Append(text).EndLine();

    /// <summary>Writes an empty line.</summary>
    public SourceWriter BlankLine()
    {
        _builder.Append(NewLine);
        return this;
    }

    /// <summary>Starts a line at the current depth, to be written piece by piece with <see cref="Append" /> and ended with <see cref="EndLine" />.</summary>
    public SourceWriter StartLine()
    {
        for (var i = 0; i < _depth; i++)
        {
            _builder.Append(IndentUnit);
        }

        return this;
    }

    /// <summary>Appends <paramref name="text" /> to the current line.</summary>
    public SourceWriter Append(string text)
    {
        _builder.Append(text);
        return this;
    }

    /// <summary>Ends the current line.</summary>
    public SourceWriter EndLine()
    {
        _builder.Append(NewLine);
        return this;
    }

    /// <summary>Opens a block: a line with <c>{</c>, then one level deeper.</summary>
    public SourceWriter OpenBlock()
    {
        Line("{");
        _depth++;
        return this;
    }

    /// <summary>Closes a block: one level shallower, then a line with <c>}</c>.</summary>
    public SourceWriter CloseBlock()
    {
        _depth--;
        return Line("}");
    }

    /// <summary>Writes the statement or member after it one level deeper, without braces.</summary>
    public SourceWriter Indented(string text)
    {
        _depth++;
        Line(text);
        _depth--;
        return this;
    }

    /// <summary>Writes a <c>/// &lt;summary&gt;</c> comment at the current depth.</summary>
    public SourceWriter Summary(string text)
    {
        StartLine().Append("/// <summary>").Append(DocumentationNewLine);
        StartLine().Append("/// ").Append(text).Append(DocumentationNewLine);
        StartLine().Append("/// </summary>").Append(DocumentationNewLine);
        return this;
    }

    /// <summary>The text written so far.</summary>
    public SourceText ToSourceText() => SourceText.From(_builder.ToString(), Encoding.UTF8);

    public override string ToString() => _builder.ToString();
}
