using System.Globalization;
using System.Text;

namespace Dovetail.Support.Configuration;

/// <summary>
///     Writes a section's schema as compact draft-07 JSON. Output is deterministic: keywords in a fixed order, properties in
///     declaration order.
/// </summary>
/// <remarks>
///     <para>
///         The section's own schema is written inline; every object type it uses is written once under <c>definitions</c>
///         (without <c>type</c>, so a nullable use can still accept <c>null</c>), and each use of it is a small object with the
///         use's annotations and <c>allOf: [{"$ref": "#/definitions/…"}]</c> (draft-07 ignores siblings of a bare <c>$ref</c>).
///         The build merging fragments moves the definitions to the document root and rewrites the references.
///     </para>
///     <para>
///         Configuration keys are case-insensitive, so every property is also matched by a case-insensitive
///         <c>patternProperties</c> entry with the same schema, and enum values accept any casing. Regexes use character classes
///         (<c>[Mm]</c>) because JSON Schema's ECMA-262 dialect has no inline <c>(?i)</c>. Editors don't show <c>default</c>,
///         <c>minimum</c> or length keywords on hover, so those are repeated in the description.
///     </para>
/// </remarks>
internal static class JsonSchemaWriter
{
    public static string Write(ConfigurationSection section, bool includeRequired)
    {
        var builder = new StringBuilder();
        var json = new ObjectWriter(builder);
        WriteMembers(builder, json, section.Schema, section.Path, includeRequired, typed: true);
        if (!section.Definitions.IsEmpty)
        {
            json.Name("definitions");
            var definitions = new ObjectWriter(builder);
            foreach (var definition in section.Definitions)
            {
                definitions.Name(definition.Name);
                var body = new ObjectWriter(builder);
                WriteMembers(builder, body, definition.Schema, null, includeRequired, typed: false);
                body.End();
            }

            definitions.End();
        }

        json.End();
        return builder.ToString();
    }

    /// <summary>A JSON string literal.</summary>
    public static string String(string value)
    {
        var builder = new StringBuilder(value.Length + 2);
        AppendString(builder, value);
        return builder.ToString();
    }

    /// <summary>An anchored ECMA-262 pattern matching <paramref name="literals" /> case-insensitively.</summary>
    public static string CaseInsensitivePattern(IEnumerable<string> literals) => "^(?:" + CaseInsensitiveAlternatives(literals) + ")$";

    private static string CaseInsensitiveAlternatives(IEnumerable<string> literals) => string.Join("|", literals.Select(CaseInsensitive));

    private static string CaseInsensitive(string literal)
    {
        var builder = new StringBuilder(literal.Length * 4);
        foreach (var character in literal)
        {
            var upper = char.ToUpperInvariant(character);
            var lower = char.ToLowerInvariant(character);
            if (upper != lower)
            {
                builder.Append('[').Append(upper).Append(lower).Append(']');
            }
            else
            {
                // Only ECMA-262 syntax characters are escaped: identity escapes of anything else are errors in unicode mode.
                if ("^$\\.*+?()[]{}|".IndexOf(character) >= 0)
                {
                    builder.Append('\\');
                }

                builder.Append(character);
            }
        }

        return builder.ToString();
    }

    private static void WriteNode(StringBuilder builder, SchemaNode node, bool includeRequired)
    {
        var json = new ObjectWriter(builder);
        WriteMembers(builder, json, node, null, includeRequired, typed: true);
        json.End();
    }

    /// <summary>Writes a node's keywords into an object that's already open, so a section root can add <c>definitions</c> after them.</summary>
    private static void WriteMembers(StringBuilder builder, ObjectWriter json, SchemaNode node, string? title, bool includeRequired, bool typed)
    {
        if (typed && node.Kind != SchemaKind.Any)
        {
            json.Raw("type", TypeText(node));
        }

        if (title is not null)
        {
            json.String("title", title);
        }

        if (Description(node) is { } description)
        {
            json.String("description", description);
        }

        if (node.Deprecated)
        {
            json.Raw("deprecated", "true");
            json.String("deprecationMessage", node.DeprecationMessage ?? "Deprecated.");
        }

        if (node.DefaultJson is not null)
        {
            json.Raw("default", node.DefaultJson);
        }

        if (!node.EnumValues.IsEmpty)
        {
            var names = CaseInsensitiveAlternatives(node.EnumValues);
            if (node.FlagsEnum)
            {
                json.String("pattern", @"^\s*(?:" + names + @")(?:\s*,\s*(?:" + names + @"))*\s*$");
            }
            else
            {
                // The exact names keep completion; the pattern accepts the binder's case-insensitive, whitespace-tolerant parse.
                var values = string.Join(",", node.EnumValues.Select(String).Concat(node.Nullable ? ["null"] : Array.Empty<string>()));
                json.Raw("anyOf", "[{\"enum\":[" + values + "]},{\"pattern\":" + String(@"^\s*(?:" + names + @")\s*$") + "}]");
            }
        }

        if (node.Format is not null)
        {
            json.String("format", node.Format);
        }

        if (node.Pattern is not null)
        {
            json.String("pattern", node.Pattern);
        }

        if (node.ContentEncoding is not null)
        {
            json.String("contentEncoding", node.ContentEncoding);
        }

        if (node.Minimum is not null)
        {
            json.Raw(node.ExclusiveMinimum ? "exclusiveMinimum" : "minimum", node.Minimum);
        }

        if (node.Maximum is not null)
        {
            json.Raw(node.ExclusiveMaximum ? "exclusiveMaximum" : "maximum", node.Maximum);
        }

        Number(json, "minLength", node.MinLength);
        Number(json, "maxLength", node.MaxLength);
        Number(json, "minItems", node.MinItems);
        Number(json, "maxItems", node.MaxItems);

        if (node.Items is not null)
        {
            json.Name("items");
            WriteNode(builder, node.Items, includeRequired);
        }

        if (node.Kind == SchemaKind.Map && node.Values is not null)
        {
            json.Name("additionalProperties");
            WriteNode(builder, node.Values, includeRequired);
            if (!node.KeyNames.IsEmpty)
            {
                json.Raw("propertyNames", "{\"pattern\":" + String(CaseInsensitivePattern(node.KeyNames)) + "}");
            }
            else if (node.KeyPattern is not null)
            {
                json.Raw("propertyNames", "{\"pattern\":" + String(node.KeyPattern) + "}");
            }
        }

        if (node.Reference is not null)
        {
            json.Raw("allOf", "[{\"$ref\":" + String("#/definitions/" + node.Reference) + "}]");
        }

        if (!node.Properties.IsEmpty)
        {
            json.Name("properties");
            var properties = new ObjectWriter(builder);
            foreach (var property in node.Properties)
            {
                properties.Name(property.Key);
                WriteNode(builder, property.Schema, includeRequired);
            }

            properties.End();

            json.Name("patternProperties");
            var patterns = new ObjectWriter(builder);
            foreach (var property in node.Properties)
            {
                patterns.Name(CaseInsensitivePattern([property.Key]));
                WriteNode(builder, property.Schema, includeRequired);
            }

            patterns.End();

            if (includeRequired && node.Properties.Any(z => z.Required))
            {
                json.Raw("required", "[" + string.Join(",", node.Properties.Where(z => z.Required).Select(z => String(z.Key))) + "]");
            }
        }
    }

    private static string TypeText(SchemaNode node)
    {
        var type = node.Kind switch
        {
            SchemaKind.String => "string",
            SchemaKind.Integer => "integer",
            SchemaKind.Number => "number",
            SchemaKind.Boolean => "boolean",
            SchemaKind.Array => "array",
            SchemaKind.Object or SchemaKind.Map => "object",
            SchemaKind.Any => throw new ArgumentOutOfRangeException(nameof(node), node.Kind, "Any has no type."),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.Kind, null),
        };
        return node.Nullable ? "[\"" + type + "\",\"null\"]" : "\"" + type + "\"";
    }

    /// <summary>The description, plus the hints editors don't show on hover: format, range, length and default.</summary>
    private static string? Description(SchemaNode node)
    {
        var parts = new List<string>();
        if (node.Description is { Length: > 0 } description)
        {
            parts.Add(description.EndsWith(".", StringComparison.Ordinal) || description.EndsWith("!", StringComparison.Ordinal) || description.EndsWith("?", StringComparison.Ordinal) ? description : description + ".");
        }

        if (node.FormatHint is not null)
        {
            parts.Add(node.FormatHint);
        }

        if (node.FlagsEnum && !node.EnumValues.IsEmpty)
        {
            parts.Add("One or more of " + string.Join(", ", node.EnumValues) + ", separated by commas.");
        }

        if (Bounds(node.Minimum, node.ExclusiveMinimum, node.Maximum, node.ExclusiveMaximum) is { } range)
        {
            parts.Add(range);
        }

        if (( Lengths(node.MinLength, node.MaxLength, "characters") ?? Lengths(node.MinItems, node.MaxItems, "items") ) is { } length)
        {
            parts.Add(length);
        }

        if (node.DefaultJson is not null)
        {
            parts.Add("Default: " + ( node.DefaultJson.StartsWith("\"", StringComparison.Ordinal) ? node.DefaultJson[1..^1].Replace("\\\"", "\"").Replace("\\\\", "\\") : node.DefaultJson ) + ".");
        }

        return parts.Count == 0 ? null : string.Join(" ", parts);
    }

    // Implicit bounds of a numeric type (e.g. byte's 0 to 255) are worth stating; int's full range isn't written at all.
    private static string? Bounds(string? minimum, bool exclusiveMinimum, string? maximum, bool exclusiveMaximum) => (minimum, maximum) switch
    {
        ({ } low, { } high) when exclusiveMinimum || exclusiveMaximum =>
            $"Greater than {( exclusiveMinimum ? "" : "or equal to " )}{low} and less than {( exclusiveMaximum ? "" : "or equal to " )}{high}.",
        ({ } low, { } high) => $"Range: {low} to {high}.",
        ({ } low, null) => exclusiveMinimum ? $"Greater than {low}." : $"At least {low}.",
        (null, { } high) => exclusiveMaximum ? $"Less than {high}." : $"At most {high}.",
        _ => null,
    };

    private static string? Lengths(int? minimum, int? maximum, string unit) => (minimum, maximum) switch
    {
        ({ } low, { } high) when low == high => $"Exactly {low} {unit}.",
        ({ } low, { } high) => $"{low} to {high} {unit}.",
        ({ } low, null) => $"At least {low} {unit}.",
        (null, { } high) => $"At most {high} {unit}.",
        _ => null,
    };

    private static void Number(ObjectWriter json, string name, int? value)
    {
        if (value is { } number)
        {
            json.Raw(name, number.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AppendString(StringBuilder builder, string value)
    {
        builder.Append('"');
        foreach (var character in value)
        {
            switch (character)
            {
                case '"':
                    builder.Append("\\\"");
                    break;
                case '\\':
                    builder.Append("\\\\");
                    break;
                case '\n':
                    builder.Append("\\n");
                    break;
                case '\r':
                    builder.Append("\\r");
                    break;
                case '\t':
                    builder.Append("\\t");
                    break;
                case '\b':
                    builder.Append("\\b");
                    break;
                case '\f':
                    builder.Append("\\f");
                    break;
                case < ' ' or (char)0x2028 or (char)0x2029:
                    builder.Append("\\u").Append(( (int)character ).ToString("x4", CultureInfo.InvariantCulture));
                    break;
                default:
                    builder.Append(character);
                    break;
            }
        }

        builder.Append('"');
    }

    /// <summary>Writes one JSON object, inserting commas between members.</summary>
    private sealed class ObjectWriter
    {
        private readonly StringBuilder _builder;
        private bool _first = true;

        public ObjectWriter(StringBuilder builder)
        {
            _builder = builder;
            _builder.Append('{');
        }

        /// <summary>Writes a member name; the caller writes its value next.</summary>
        public void Name(string name)
        {
            if (!_first)
            {
                _builder.Append(',');
            }

            _first = false;
            AppendString(_builder, name);
            _builder.Append(':');
        }

        public void String(string name, string value)
        {
            Name(name);
            AppendString(_builder, value);
        }

        public void Raw(string name, string json)
        {
            Name(name);
            _builder.Append(json);
        }

        public void End() => _builder.Append('}');
    }
}
