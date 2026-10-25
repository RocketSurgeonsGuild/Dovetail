using System.Globalization;
using System.Text;

namespace Dovetail.Build.Tasks;

/// <summary>
///     A node of a parsed JSONC document. Positions are offsets into the text that was parsed, so a caller can splice new text
///     into the original without reformatting anything else.
/// </summary>
internal abstract class JsoncNode(int start, int end)
{
    /// <summary>
    ///     The offset of the node's first character.
    /// </summary>
    public int Start { get; } = start;

    /// <summary>
    ///     The offset after the node's last character.
    /// </summary>
    public int End { get; } = end;
}

/// <summary>
///     A string value; <see cref="Value" /> is the unescaped text.
/// </summary>
internal sealed class JsoncString(int start, int end, string value) : JsoncNode(start, end)
{
    public string Value { get; } = value;
}

/// <summary>
///     A number, <c>true</c>, <c>false</c> or <c>null</c>.
/// </summary>
internal sealed class JsoncLiteral(int start, int end) : JsoncNode(start, end);

/// <summary>
///     A property of an object. <see cref="NameStart" /> is the offset of the name's opening quote.
/// </summary>
internal sealed class JsoncMember(string name, int nameStart, JsoncNode value)
{
    public string Name { get; } = name;

    public int NameStart { get; } = nameStart;

    public JsoncNode Value { get; } = value;
}

/// <summary>
///     An object. <see cref="TrailingCommaEnd" /> is the offset after a comma that follows the last member, or -1.
/// </summary>
internal sealed class JsoncObject(int start, int end, IReadOnlyList<JsoncMember> members, int trailingCommaEnd) : JsoncNode(start, end)
{
    public IReadOnlyList<JsoncMember> Members { get; } = members;

    public int TrailingCommaEnd { get; } = trailingCommaEnd;
}

/// <summary>
///     An array. <see cref="TrailingCommaEnd" /> is the offset after a comma that follows the last item, or -1.
/// </summary>
internal sealed class JsoncArray(int start, int end, IReadOnlyList<JsoncNode> items, int trailingCommaEnd) : JsoncNode(start, end)
{
    public IReadOnlyList<JsoncNode> Items { get; } = items;

    public int TrailingCommaEnd { get; } = trailingCommaEnd;
}

/// <summary>
///     The document <see cref="JsoncParser" /> read: its root value (null when the text holds nothing but whitespace and
///     comments).
/// </summary>
internal sealed class JsoncDocument(string text, JsoncNode? root)
{
    public string Text { get; } = text;

    public JsoncNode? Root { get; } = root;
}

/// <summary>
///     A small, strict JSONC reader: JSON plus <c>//</c> and <c>/* */</c> comments and trailing commas, the dialect VS Code's
///     settings files use. It keeps offsets rather than building a value tree, and rejects anything else (single quotes,
///     unquoted keys, duplicate commas, content after the root), so a caller only edits files it understood completely.
/// </summary>
internal sealed class JsoncParser
{
    private const int MaxDepth = 32;

    private readonly string _text;
    private int _position;

    private JsoncParser(string text) => _text = text;

    /// <summary>
    ///     Parses <paramref name="text" />; throws <see cref="FormatException" /> when it isn't JSONC this parser fully understands.
    /// </summary>
    public static JsoncDocument Parse(string text)
    {
        var parser = new JsoncParser(text);
        parser.SkipTrivia();
        if (parser._position == text.Length) return new(text, null);

        var root = parser.ParseValue(0);
        parser.SkipTrivia();
        if (parser._position != text.Length) throw parser.Error("unexpected content after the root value");

        return new(text, root);
    }

    private FormatException Error(string message) => new($"{message} (at offset {_position})");

    /// <summary>
    ///     Skips whitespace and comments.
    /// </summary>
    private void SkipTrivia()
    {
        while (_position < _text.Length)
        {
            var c = _text[_position];
            if (c is ' ' or '\t' or '\r' or '\n')
            {
                _position++;
            }
            else if (c == '/' && _position + 1 < _text.Length && _text[_position + 1] == '/')
            {
                _position += 2;
                while (_position < _text.Length && _text[_position] is not ( '\r' or '\n' )) _position++;
            }
            else if (c == '/' && _position + 1 < _text.Length && _text[_position + 1] == '*')
            {
                var close = _text.IndexOf("*/", _position + 2, StringComparison.Ordinal);
                if (close < 0) throw Error("unterminated block comment");

                _position = close + 2;
            }
            else
            {
                return;
            }
        }
    }

    private JsoncNode ParseValue(int depth)
    {
        if (_position >= _text.Length) throw Error("unexpected end of text");
        if (depth > MaxDepth) throw Error("nested too deeply");

        return _text[_position] switch
        {
            '{' => ParseObject(depth),
            '[' => ParseArray(depth),
            '"' => ParseString(),
            _ => ParseLiteral(),
        };
    }

    private JsoncObject ParseObject(int depth)
    {
        var start = _position++;
        var members = new List<JsoncMember>();
        var trailingComma = -1;
        while (true)
        {
            SkipTrivia();
            if (_position >= _text.Length) throw Error("unterminated object");

            if (_text[_position] == '}')
            {
                _position++;
                return new(start, _position, members, trailingComma);
            }

            if (_text[_position] != '"') throw Error("expected a property name");

            var name = ParseString();
            SkipTrivia();
            if (_position >= _text.Length || _text[_position] != ':') throw Error("expected ':'");

            _position++;
            SkipTrivia();
            members.Add(new(name.Value, name.Start, ParseValue(depth + 1)));
            SkipTrivia();
            if (_position < _text.Length && _text[_position] == ',')
            {
                _position++;
                trailingComma = _position;
            }
            else if (_position < _text.Length && _text[_position] == '}')
            {
                trailingComma = -1;
            }
            else
            {
                throw Error("expected ',' or '}'");
            }
        }
    }

    private JsoncArray ParseArray(int depth)
    {
        var start = _position++;
        var items = new List<JsoncNode>();
        var trailingComma = -1;
        while (true)
        {
            SkipTrivia();
            if (_position >= _text.Length) throw Error("unterminated array");

            if (_text[_position] == ']')
            {
                _position++;
                return new(start, _position, items, trailingComma);
            }

            items.Add(ParseValue(depth + 1));
            SkipTrivia();
            if (_position < _text.Length && _text[_position] == ',')
            {
                _position++;
                trailingComma = _position;
            }
            else if (_position < _text.Length && _text[_position] == ']')
            {
                trailingComma = -1;
            }
            else
            {
                throw Error("expected ',' or ']'");
            }
        }
    }

    private JsoncString ParseString()
    {
        var start = _position++;
        var value = new StringBuilder();
        while (true)
        {
            if (_position >= _text.Length) throw Error("unterminated string");

            var c = _text[_position++];
            if (c == '"') return new(start, _position, value.ToString());
            if (c < ' ') throw Error("control character in a string");

            if (c != '\\')
            {
                value.Append(c);
                continue;
            }

            if (_position >= _text.Length) throw Error("unterminated string");

            var escape = _text[_position++];
            switch (escape)
            {
                case '"': value.Append('"'); break;
                case '\\': value.Append('\\'); break;
                case '/': value.Append('/'); break;
                case 'b': value.Append('\b'); break;
                case 'f': value.Append('\f'); break;
                case 'n': value.Append('\n'); break;
                case 'r': value.Append('\r'); break;
                case 't': value.Append('\t'); break;
                case 'u':
                    if (_position + 4 > _text.Length || !int.TryParse(_text.Substring(_position, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var code))
                    {
                        throw Error("invalid \\u escape");
                    }

                    value.Append((char)code);
                    _position += 4;
                    break;
                default:
                    throw Error("invalid escape sequence");
            }
        }
    }

    private JsoncLiteral ParseLiteral()
    {
        var start = _position;
        while (_position < _text.Length && ( char.IsLetterOrDigit(_text[_position]) || _text[_position] is '-' or '+' or '.' )) _position++;

        var literal = _text.Substring(start, _position - start);
        if (literal is "true" or "false" or "null") return new(start, _position);
        if (IsNumber(literal)) return new(start, _position);

        _position = start;
        throw Error(literal.Length == 0 ? "unexpected character" : $"invalid value '{literal}'");
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';

    /// <summary>
    ///     JSON's number grammar: <c>-? (0 | [1-9][0-9]*) (. [0-9]+)? ([eE] [+-]? [0-9]+)?</c>.
    /// </summary>
    private static bool IsNumber(string literal)
    {
        var i = 0;
        if (i < literal.Length && literal[i] == '-') i++;
        if (i >= literal.Length || !IsDigit(literal[i])) return false;

        if (literal[i] == '0') i++;
        else while (i < literal.Length && IsDigit(literal[i])) i++;

        if (i < literal.Length && literal[i] == '.')
        {
            i++;
            if (i >= literal.Length || !IsDigit(literal[i])) return false;

            while (i < literal.Length && IsDigit(literal[i])) i++;
        }

        if (i < literal.Length && literal[i] is 'e' or 'E')
        {
            i++;
            if (i < literal.Length && literal[i] is '+' or '-') i++;
            if (i >= literal.Length || !IsDigit(literal[i])) return false;

            while (i < literal.Length && IsDigit(literal[i])) i++;
        }

        return i == literal.Length;
    }
}
