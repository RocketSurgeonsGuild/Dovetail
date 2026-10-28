using System.Text;
using System.Text.Json;

namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     What <see cref="VsCodeSettingsEditor.Apply" /> decided.
/// </summary>
internal enum EditorSettingsStatus
{
    /// <summary>
    ///     The settings already hold Dovetail's mappings; nothing to write.
    /// </summary>
    Unchanged,

    /// <summary>
    ///     <see cref="EditorSettingsResult.Content" /> is the new file.
    /// </summary>
    Updated,

    /// <summary>
    ///     The file can't be edited with confidence; <see cref="EditorSettingsResult.Reason" /> says why.
    /// </summary>
    Unsafe,
}

/// <summary>
///     The outcome of <see cref="VsCodeSettingsEditor.Apply" />.
/// </summary>
internal readonly struct EditorSettingsResult(EditorSettingsStatus status, byte[] content, string reason)
{
    public EditorSettingsStatus Status { get; } = status;

    public byte[] Content { get; } = content;

    public string Reason { get; } = reason;
}

/// <summary>
///     Makes sure a VS Code <c>settings.json</c> holds Dovetail's mappings for a host (<see cref="EditorSettingsMappings" />) by
///     splicing text into the original, so everything else stays byte for byte: comments, trailing commas, indentation, line
///     endings, the byte-order mark, other settings and other entries in the three maps. Only the host's own entries are looked
///     at: the <c>json.schemas</c> entry whose <c>url</c> is the host's schema, the <c>yaml.schemas</c> key that is the host's
///     schema, and the <c>evenBetterToml.schema.associations</c> key that is the host's pattern. A file it doesn't fully
///     understand, or one where an edit wouldn't survive a re-read, is reported as unsafe and never written.
/// </summary>
internal static class VsCodeSettingsEditor
{
    private static readonly byte[] Utf8Bom = [0xEF, 0xBB, 0xBF];
    private static readonly UTF8Encoding StrictUtf8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>
    ///     Adds or completes Dovetail's mappings in <paramref name="original" /> (null or empty when the file doesn't exist).
    /// </summary>
    public static EditorSettingsResult Apply(byte[]? original, EditorSettingsMappings mappings) => ApplyCore(original, mappings, verify: true);

    /// <summary>
    ///     A complete settings document holding only Dovetail's mappings.
    /// </summary>
    internal static string RenderDocument(EditorSettingsMappings mappings, string newLine, string indentUnit)
    {
        var writer = new Renderer(newLine, indentUnit, multiline: true);
        return "{" + newLine + indentUnit + string.Join("," + newLine + indentUnit, writer.Sections(mappings, indentUnit)) + newLine + "}" + newLine;
    }

    private static EditorSettingsResult ApplyCore(byte[]? original, EditorSettingsMappings mappings, bool verify)
    {
        original ??= [];
        var hasBom = original.Length >= Utf8Bom.Length && original.Take(Utf8Bom.Length).SequenceEqual(Utf8Bom);
        var body = hasBom ? [.. original.Skip(Utf8Bom.Length)] : original;
        if (body.Length >= 2 && ( ( body[0] == 0xFF && body[1] == 0xFE ) || ( body[0] == 0xFE && body[1] == 0xFF ) ))
        {
            return Rejected("the file isn't UTF-8");
        }

        string text;
        try
        {
            text = StrictUtf8.GetString(body);
        }
        catch (ArgumentException)
        {
            return Rejected("the file isn't valid UTF-8");
        }

        var newLine = DetectNewLine(text);
        if (text.All(z => z is ' ' or '\t' or '\r' or '\n'))
        {
            return Updated(hasBom, RenderDocument(mappings, newLine, "  "));
        }

        JsoncDocument document;
        try
        {
            document = JsoncParser.Parse(text);

            // A second opinion, from the parser VS Code's JSON is read with elsewhere; it has no tokens to read in a comments-only file.
            if (document.Root is not null) JsonDocument.Parse(text, new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }).Dispose();
        }
        catch (Exception exception) when (exception is FormatException or JsonException)
        {
            return Rejected($"it isn't valid JSON with comments: {exception.Message}");
        }

        string updated;
        if (document.Root is null)
        {
            // Only comments: keep them and add the document after them.
            updated = text + ( text.EndsWith("\n", StringComparison.Ordinal) ? "" : newLine ) + RenderDocument(mappings, newLine, "  ");
        }
        else if (document.Root is JsoncObject root)
        {
            var session = new EditSession(text, newLine, mappings);
            if (session.Plan(root) is { } reason) return Rejected(reason);

            if (session.Edits.Count == 0) return new(EditorSettingsStatus.Unchanged, original, "");

            updated = session.Apply();
        }
        else
        {
            return Rejected("the root isn't an object");
        }

        if (verify)
        {
            // The edit has to survive a re-read, and doing it again has to find nothing left to do.
            try
            {
                JsoncParser.Parse(updated);
                JsonDocument.Parse(updated, new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }).Dispose();
            }
            catch (Exception exception) when (exception is FormatException or JsonException)
            {
                return Rejected($"the edited file wouldn't be valid ({exception.Message})");
            }

            var again = ApplyCore(Encode(hasBom, updated), mappings, verify: false);
            if (again.Status != EditorSettingsStatus.Unchanged) return Rejected("the edit isn't stable");
        }

        return Updated(hasBom, updated);
    }

    private static EditorSettingsResult Rejected(string reason) => new(EditorSettingsStatus.Unsafe, [], reason);

    private static EditorSettingsResult Updated(bool bom, string text) => new(EditorSettingsStatus.Updated, Encode(bom, text), "");

    private static byte[] Encode(bool bom, string text)
    {
        var bytes = StrictUtf8.GetBytes(text);
        return bom ? [.. Utf8Bom, .. bytes] : bytes;
    }

    private static string DetectNewLine(string text)
    {
        var index = text.IndexOf('\n');
        return index > 0 && text[index - 1] == '\r' ? "\r\n" : "\n";
    }

    /// <summary>
    ///     Quotes a string as JSON.
    /// </summary>
    private static string Quote(string value)
    {
        var quoted = new StringBuilder("\"");
        foreach (var c in value)
        {
            switch (c)
            {
                case '"': quoted.Append("\\\""); break;
                case '\\': quoted.Append("\\\\"); break;
                case '\n': quoted.Append("\\n"); break;
                case '\r': quoted.Append("\\r"); break;
                case '\t': quoted.Append("\\t"); break;
                case < ' ': quoted.Append("\\u").Append(( (int)c ).ToString("x4", System.Globalization.CultureInfo.InvariantCulture)); break;
                default: quoted.Append(c); break;
            }
        }

        return quoted.Append('"').ToString();
    }

    /// <summary>
    ///     Renders Dovetail's JSON at a given indentation: multi-line (one line per property, matching the surrounding file) or
    ///     inline, for containers that sit on one line.
    /// </summary>
    private sealed class Renderer(string newLine, string unit, bool multiline)
    {
        private string Break(string indent) => multiline ? newLine + indent : " ";

        /// <summary>
        ///     A <c>json.schemas</c> entry whose first line is not indented, and whose other lines are indented from <paramref name="indent" />.
        /// </summary>
        public string JsonEntry(EditorSettingsMappings mappings, string indent)
        {
            var inner = indent + unit;
            return "{" + Break(inner) + "\"fileMatch\": [" + Quote(mappings.JsonFileMatch) + "]," + Break(inner) + "\"url\": " + Quote(mappings.SchemaUrl) + Break(indent) + "}";
        }

        public string FileMatchMember(EditorSettingsMappings mappings) => "\"fileMatch\": [" + Quote(mappings.JsonFileMatch) + "]";

        public string YamlMember(EditorSettingsMappings mappings) => Quote(mappings.SchemaUrl) + ": [" + string.Join(", ", mappings.YamlGlobs.Select(Quote)) + "]";

        public string TomlMember(EditorSettingsMappings mappings) => Quote(mappings.TomlPattern) + ": " + Quote(mappings.SchemaUrl);

        /// <summary>
        ///     The three settings, each as a property whose first line is not indented.
        /// </summary>
        public IEnumerable<string> Sections(EditorSettingsMappings mappings, string indent)
        {
            var inner = indent + unit;
            yield return JsonSchemasSection(mappings, indent, inner);
            yield return Quote(EditorSettingsMappings.YamlSchemasKey) + ": {" + Break(inner) + YamlMember(mappings) + Break(indent) + "}";
            yield return Quote(EditorSettingsMappings.TomlAssociationsKey) + ": {" + Break(inner) + TomlMember(mappings) + Break(indent) + "}";
        }

        public string JsonSchemasSection(EditorSettingsMappings mappings, string indent, string inner) =>
            Quote(EditorSettingsMappings.JsonSchemasKey) + ": [" + ( multiline ? Break(inner) : "" ) + JsonEntry(mappings, inner) + ( multiline ? Break(indent) : "" ) + "]";

        public string Section(string key, EditorSettingsMappings mappings, string indent) =>
            key switch
            {
                EditorSettingsMappings.JsonSchemasKey => JsonSchemasSection(mappings, indent, indent + unit),
                EditorSettingsMappings.YamlSchemasKey => Quote(key) + ": {" + Break(indent + unit) + YamlMember(mappings) + Break(indent) + "}",
                _ => Quote(key) + ": {" + Break(indent + unit) + TomlMember(mappings) + Break(indent) + "}",
            };

        public static string QuoteString(string value) => Quote(value);
    }

    /// <summary>
    ///     Plans the text insertions and replacements that complete the mappings in one parsed document.
    /// </summary>
    private sealed class EditSession(string text, string newLine, EditorSettingsMappings mappings)
    {
        private readonly List<(int Position, int Length, string Text)> _edits = [];
        private string _unit = "  ";

        public IReadOnlyList<(int Position, int Length, string Text)> Edits => _edits;

        /// <summary>
        ///     Plans the edits to <paramref name="root" />; returns why they can't be made safely, or null.
        /// </summary>
        public string? Plan(JsoncObject root)
        {
            _unit = DetectUnit(root);
            var missing = new List<string>();
            foreach (var key in new[] { EditorSettingsMappings.JsonSchemasKey, EditorSettingsMappings.YamlSchemasKey, EditorSettingsMappings.TomlAssociationsKey })
            {
                var found = root.Members.Where(z => z.Name == key).ToArray();
                if (found.Length > 1) return $"\"{key}\" appears more than once";

                if (found.Length == 0)
                {
                    missing.Add(key);
                    continue;
                }

                var reason = key switch
                {
                    EditorSettingsMappings.JsonSchemasKey => PlanJsonSchemas(found[0].Value),
                    EditorSettingsMappings.YamlSchemasKey => PlanYamlSchemas(found[0].Value),
                    _ => PlanTomlAssociations(found[0].Value),
                };
                if (reason is not null) return reason;
            }

            if (missing.Count > 0)
            {
                Append(root.Start, root.End - 1, root.Members.Count == 0 ? -1 : root.Members[root.Members.Count - 1].Value.End, root.Members.Count == 0 ? -1 : root.Members[root.Members.Count - 1].NameStart, root.TrailingCommaEnd, [.. missing.Select(key => (Func<string, bool, string>)( (indent, multiline) => new Renderer(newLine, _unit, multiline).Section(key, mappings, indent) ))]);
            }

            return null;
        }

        /// <summary>
        ///     Applies the planned edits, last position first so earlier offsets stay valid.
        /// </summary>
        public string Apply()
        {
            var result = text;
            foreach (var edit in _edits.Select((edit, index) => (edit, index)).OrderByDescending(z => z.edit.Position).ThenByDescending(z => z.index))
            {
                result = result.Substring(0, edit.edit.Position) + edit.edit.Text + result.Substring(edit.edit.Position + edit.edit.Length);
            }

            return result;
        }

        private string? PlanJsonSchemas(JsoncNode value)
        {
            if (value is not JsoncArray array) return "\"json.schemas\" isn't an array";

            foreach (var item in array.Items)
            {
                if (item is not JsoncObject entry
                 || entry.Members.FirstOrDefault(z => z.Name == "url")?.Value is not JsoncString url
                 || EditorSettingsMappings.NormalizePath(url.Value) != EditorSettingsMappings.NormalizePath(mappings.SchemaUrl))
                {
                    continue;
                }

                var fileMatch = entry.Members.Where(z => z.Name == "fileMatch").ToArray();
                if (fileMatch.Length > 1) return "a Dovetail entry of \"json.schemas\" has two \"fileMatch\" properties";

                if (fileMatch.Length == 0)
                {
                    Append(entry.Start, entry.End - 1, LastEnd(entry.Members), LastStart(entry.Members), entry.TrailingCommaEnd, [(_, multiline) => new Renderer(newLine, _unit, multiline).FileMatchMember(mappings)]);
                    return null;
                }

                if (fileMatch[0].Value is not JsoncArray patterns || patterns.Items.Any(z => z is not JsoncString)) return "a Dovetail entry of \"json.schemas\" has an unexpected \"fileMatch\"";

                if (patterns.Items.Cast<JsoncString>().All(z => z.Value != mappings.JsonFileMatch))
                {
                    Append(patterns.Start, patterns.End - 1, patterns.Items.Count == 0 ? -1 : patterns.Items[patterns.Items.Count - 1].End, patterns.Items.Count == 0 ? -1 : patterns.Items[patterns.Items.Count - 1].Start, patterns.TrailingCommaEnd, [(_, _) => Renderer.QuoteString(mappings.JsonFileMatch)]);
                }

                return null;
            }

            Append(array.Start, array.End - 1, array.Items.Count == 0 ? -1 : array.Items[array.Items.Count - 1].End, array.Items.Count == 0 ? -1 : array.Items[array.Items.Count - 1].Start, array.TrailingCommaEnd, [(indent, multiline) => new Renderer(newLine, _unit, multiline).JsonEntry(mappings, indent)]);
            return null;
        }

        private string? PlanYamlSchemas(JsoncNode value)
        {
            if (value is not JsoncObject map) return "\"yaml.schemas\" isn't an object";

            var matches = map.Members.Where(z => EditorSettingsMappings.NormalizePath(z.Name) == EditorSettingsMappings.NormalizePath(mappings.SchemaUrl)).ToArray();
            if (matches.Length > 1) return "\"yaml.schemas\" maps Dovetail's schema more than once";

            if (matches.Length == 0)
            {
                Append(map.Start, map.End - 1, LastEnd(map.Members), LastStart(map.Members), map.TrailingCommaEnd, [(_, multiline) => new Renderer(newLine, _unit, multiline).YamlMember(mappings)]);
                return null;
            }

            switch (matches[0].Value)
            {
                case JsoncString single:
                    {
                        var wanted = mappings.YamlGlobs.Where(z => z != single.Value).ToArray();
                        if (wanted.Length > 0)
                        {
                            _edits.Add((single.Start, single.End - single.Start, "[" + string.Join(", ", new[] { single.Value }.Concat(wanted).Select(Renderer.QuoteString)) + "]"));
                        }

                        return null;
                    }
                case JsoncArray globs when globs.Items.All(z => z is JsoncString):
                    {
                        var present = globs.Items.Cast<JsoncString>().Select(z => z.Value).ToArray();
                        var wanted = mappings.YamlGlobs.Where(z => !present.Contains(z, StringComparer.Ordinal)).ToArray();
                        if (wanted.Length > 0)
                        {
                            Append(globs.Start, globs.End - 1, globs.Items.Count == 0 ? -1 : globs.Items[globs.Items.Count - 1].End, globs.Items.Count == 0 ? -1 : globs.Items[globs.Items.Count - 1].Start, globs.TrailingCommaEnd, [.. wanted.Select(glob => (Func<string, bool, string>)( (_, _) => Renderer.QuoteString(glob) ))]);
                        }

                        return null;
                    }
                default:
                    return "a Dovetail entry of \"yaml.schemas\" has an unexpected value";
            }
        }

        private string? PlanTomlAssociations(JsoncNode value)
        {
            if (value is not JsoncObject map) return "\"evenBetterToml.schema.associations\" isn't an object";

            var matches = map.Members.Where(z => z.Name == mappings.TomlPattern).ToArray();
            if (matches.Length > 1) return "\"evenBetterToml.schema.associations\" maps Dovetail's pattern more than once";

            if (matches.Length == 0)
            {
                Append(map.Start, map.End - 1, LastEnd(map.Members), LastStart(map.Members), map.TrailingCommaEnd, [(_, multiline) => new Renderer(newLine, _unit, multiline).TomlMember(mappings)]);
                return null;
            }

            if (matches[0].Value is not JsoncString schema) return "a Dovetail entry of \"evenBetterToml.schema.associations\" has an unexpected value";

            if (EditorSettingsMappings.NormalizePath(schema.Value) != EditorSettingsMappings.NormalizePath(mappings.SchemaUrl))
            {
                _edits.Add((schema.Start, schema.End - schema.Start, Renderer.QuoteString(mappings.SchemaUrl)));
            }

            return null;
        }

        private static int LastEnd(IReadOnlyList<JsoncMember> members) => members.Count == 0 ? -1 : members[members.Count - 1].Value.End;

        private static int LastStart(IReadOnlyList<JsoncMember> members) => members.Count == 0 ? -1 : members[members.Count - 1].NameStart;

        /// <summary>
        ///     Appends items to the container that opens at <paramref name="open" /> and closes at <paramref name="close" />.
        ///     <paramref name="lastEnd" />/<paramref name="lastStart" /> locate its last item (-1 when it has none) and
        ///     <paramref name="trailingCommaEnd" /> the comma after it (-1 when there is none). Each renderer gets the indentation
        ///     the item will have and whether the container is multi-line.
        /// </summary>
        private void Append(int open, int close, int lastEnd, int lastStart, int trailingCommaEnd, Func<string, bool, string>[] renderers)
        {
            var multiline = text.IndexOf('\n', open, close - open) >= 0;
            var containerIndent = LineIndent(open);
            if (lastEnd < 0)
            {
                var itemIndent = containerIndent + _unit;
                var items = string.Join("," + newLine + itemIndent, renderers.Select(z => z(itemIndent, true)));
                var inside = text.Substring(open + 1, close - open - 1);
                if (inside.All(z => z is ' ' or '\t' or '\r' or '\n'))
                {
                    _edits.Add((open + 1, inside.Length, newLine + itemIndent + items + newLine + containerIndent));
                }
                else if (multiline)
                {
                    _edits.Add((EndOfLineAfter(open + 1), 0, newLine + itemIndent + items));
                }
                else
                {
                    _edits.Add((open + 1, 0, newLine + itemIndent + items + newLine + containerIndent));
                }

                return;
            }

            if (!multiline)
            {
                var items = string.Join(", ", renderers.Select(z => z("", false)));
                _edits.Add(trailingCommaEnd >= 0 ? (trailingCommaEnd, 0, " " + items + ",") : (lastEnd, 0, ", " + items));
                return;
            }

            var indent = StartsLine(lastStart) ? LineIndent(lastStart) : containerIndent + _unit;
            var lines = string.Join("," + newLine + indent, renderers.Select(z => z(indent, true)));
            if (trailingCommaEnd >= 0)
            {
                _edits.Add((EndOfLineAfter(trailingCommaEnd), 0, newLine + indent + lines + ","));
                return;
            }

            var lineEnd = EndOfLineAfter(lastEnd);
            if (lineEnd == lastEnd)
            {
                _edits.Add((lastEnd, 0, "," + newLine + indent + lines));
            }
            else
            {
                _edits.Add((lastEnd, 0, ","));
                _edits.Add((lineEnd, 0, newLine + indent + lines));
            }
        }

        /// <summary>
        ///     One level of indentation: what the root object's first property is indented by, else two spaces.
        /// </summary>
        private string DetectUnit(JsoncObject root)
        {
            if (root.Members.Count == 0 || !StartsLine(root.Members[0].NameStart)) return "  ";

            var indent = LineIndent(root.Members[0].NameStart);
            var rootIndent = LineIndent(root.Start);
            return indent.Length > rootIndent.Length && indent.StartsWith(rootIndent, StringComparison.Ordinal) ? indent.Substring(rootIndent.Length) : "  ";
        }

        private int LineStart(int position) => position == 0 ? 0 : text.LastIndexOf('\n', position - 1) + 1;

        private string LineIndent(int position)
        {
            var start = LineStart(position);
            var end = start;
            while (end < text.Length && text[end] is ' ' or '\t') end++;

            return text.Substring(start, end - start);
        }

        private bool StartsLine(int position)
        {
            for (var i = LineStart(position); i < position; i++)
            {
                if (text[i] is not ( ' ' or '\t' )) return false;
            }

            return true;
        }

        /// <summary>
        ///     Where the line holding <paramref name="position" /> ends, after any trailing spaces and comments, so new text goes
        ///     after a comment that comments on the preceding item instead of between them. Returns <paramref name="position" />
        ///     itself when something else follows on that line.
        /// </summary>
        private int EndOfLineAfter(int position)
        {
            var i = position;
            while (i < text.Length)
            {
                if (text[i] is ' ' or '\t')
                {
                    i++;
                }
                else if (string.CompareOrdinal(text, i, "//", 0, 2) == 0)
                {
                    while (i < text.Length && text[i] is not ( '\r' or '\n' )) i++;
                }
                else if (string.CompareOrdinal(text, i, "/*", 0, 2) == 0)
                {
                    var close = text.IndexOf("*/", i + 2, StringComparison.Ordinal);
                    if (close < 0 || text.IndexOf('\n', i, close - i) >= 0) return position;

                    i = close + 2;
                }
                else
                {
                    break;
                }
            }

            return i >= text.Length || text[i] is '\r' or '\n' ? i : position;
        }
    }
}
