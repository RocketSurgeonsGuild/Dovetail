using System.Text;
using System.Text.Json.Nodes;

namespace Dovetail.Build.Tasks;

/// <summary>
///     Hoists fragments' <c>definitions</c> into the output document's root <c>definitions</c>. Each definition is renamed so
///     names are unique across fragments: the declaring section's path (segments joined with <c>.</c>, characters outside
///     <c>[A-Za-z0-9_-]</c> replaced by <c>_</c>), a <c>.</c>, the original name, and <c>.2</c>, <c>.3</c>... when that is
///     taken (ignoring case). <c>SchemaStoreAppSettings</c> is reserved for the embedded SchemaStore schema.
/// </summary>
/// <remarks>
///     Fragments are hoisted in the order the builders visit them, which follows the section tree, never reference order, so
///     the names are stable for the same set of fragments.
/// </remarks>
internal sealed class DefinitionRegistry
{
    private readonly HashSet<string> _names = [with(StringComparer.OrdinalIgnoreCase), SchemaStoreAppSettings.DefinitionName,];
    private readonly Dictionary<ConfigurationSchemaFragment, JsonNode> _roots = [];
    private readonly Dictionary<string, HoistedDefinition> _definitions = [with(StringComparer.Ordinal)];
    private readonly List<KeyValuePair<string, JsonNode>> _locations = [];

    /// <summary>
    ///     The hoisted definitions, ordered by name.
    /// </summary>
    public IEnumerable<HoistedDefinition> Definitions => _definitions.Values.OrderBy(z => z.Name, StringComparer.Ordinal);

    /// <summary>
    ///     The schemas added with <see cref="AddLocation" />.
    /// </summary>
    public IReadOnlyList<KeyValuePair<string, JsonNode>> Locations => _locations;

    /// <summary>
    ///     Stores the finished schema of a section (or of a key several schemas describe) as a definition named after its
    ///     configuration path, segments sanitized as above but joined with <c>:</c> (<c>Shared:Http</c>, next to a hoisted
    ///     <c>Shared.Retry</c>), and returns a reference to it. Such a schema is used by its key and by the key's case-insensitive
    ///     twin, and through a reference it is written once rather than twice per level.
    /// </summary>
    public string AddLocation(IReadOnlyList<string> path, JsonNode schema)
    {
        var name = Reserve(string.Join(":", path.Select(z => Sanitize([z]))));
        _locations.Add(new(name, schema));
        return ConfigurationSchemaFragment.DefinitionReferencePrefix + name;
    }

    /// <summary>
    ///     The fragment's schema without its definitions, with its references rewritten to the hoisted names. The first call
    ///     hoists the definitions; later calls return the same (shared, so don't modify it) node.
    /// </summary>
    public JsonNode Root(ConfigurationSchemaFragment fragment, IReadOnlyList<string> sectionPath)
    {
        if (_roots.TryGetValue(fragment, out var root)) return root;

        var schema = fragment.Schema.DeepClone();
        var renames = new Dictionary<string, string>(StringComparer.Ordinal);
        var hoisted = new List<HoistedDefinition>();
        if (schema is JsonObject value && value["definitions"] is JsonObject definitions)
        {
            value.Remove("definitions");
            var prefix = Sanitize(sectionPath);
            foreach (var definition in definitions.ToArray())
            {
                definitions.Remove(definition.Key);
                var name = Reserve($"{prefix}.{definition.Key}");
                renames[definition.Key] = name;
                hoisted.Add(new(name, definition.Key, sectionPath, definition.Value ?? new JsonObject()));
            }
        }

        RewriteReferences(schema, renames);
        foreach (var definition in hoisted)
        {
            RewriteReferences(definition.Schema, renames);
            _definitions.Add(definition.Name, definition);
        }

        _roots.Add(fragment, schema);
        return schema;
    }

    /// <summary>
    ///     The definition <paramref name="node" /> uses, when it is a reference to one (<c>{"$ref": ...}</c>) or a wrapper around
    ///     one (<c>{"allOf": [{"$ref": ...}], ...}</c> without <c>properties</c>).
    /// </summary>
    public HoistedDefinition? Resolve(JsonNode? node) => node switch
    {
        JsonObject value when value.ContainsKey("properties") => null,
        JsonObject value when Find(value["$ref"]) is { } definition => definition,
        JsonObject value when value["allOf"] is JsonArray { Count: 1, } allOf && allOf[0] is JsonObject member && member.Count == 1 => Find(member["$ref"]),
        _ => null,
    };

    /// <summary>
    ///     Every definition reachable from <paramref name="nodes" /> through references, transitively.
    /// </summary>
    public IReadOnlyCollection<HoistedDefinition> Reachable(IEnumerable<JsonNode> nodes)
    {
        var found = new Dictionary<string, HoistedDefinition>(StringComparer.Ordinal);
        var pending = new Stack<JsonNode>(nodes);
        while (pending.Count > 0)
        {
            switch (pending.Pop())
            {
                case JsonObject value:
                    foreach (var property in value)
                    {
                        if (property.Key == "$ref" && Find(property.Value) is { } definition)
                        {
                            if (found.ContainsKey(definition.Name)) continue;
                            found.Add(definition.Name, definition);
                            pending.Push(definition.Schema);
                        }
                        else if (property.Value is not null)
                        {
                            pending.Push(property.Value);
                        }
                    }

                    break;
                case JsonArray array:
                    foreach (var item in array)
                    {
                        if (item is not null) pending.Push(item);
                    }

                    break;
                default:
                    break;
            }
        }

        return found.Values;
    }

    private HoistedDefinition? Find(JsonNode? reference) =>
        reference is JsonValue value
     && value.TryGetValue<string>(out var target)
     && target.StartsWith(ConfigurationSchemaFragment.DefinitionReferencePrefix, StringComparison.Ordinal)
     && _definitions.TryGetValue(target.Substring(ConfigurationSchemaFragment.DefinitionReferencePrefix.Length), out var definition)
            ? definition
            : null;

    private string Reserve(string name)
    {
        var candidate = name;
        for (var index = 2; !_names.Add(candidate); index++)
        {
            candidate = $"{name}.{index}";
        }

        return candidate;
    }

    private static string Sanitize(IReadOnlyList<string> path)
    {
        var builder = new StringBuilder();
        foreach (var segment in path)
        {
            if (builder.Length > 0) builder.Append('.');
            foreach (var character in segment)
            {
                builder.Append(character is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '_' or '-' ? character : '_');
            }
        }

        return builder.ToString();
    }

    private static void RewriteReferences(JsonNode? node, Dictionary<string, string> renames)
    {
        switch (node)
        {
            case JsonObject value:
                if (value["$ref"] is JsonValue reference
                 && reference.TryGetValue<string>(out var target)
                 && target.StartsWith(ConfigurationSchemaFragment.DefinitionReferencePrefix, StringComparison.Ordinal)
                 && renames.TryGetValue(target.Substring(ConfigurationSchemaFragment.DefinitionReferencePrefix.Length), out var name))
                {
                    value["$ref"] = ConfigurationSchemaFragment.DefinitionReferencePrefix + name;
                }

                foreach (var property in value)
                {
                    if (property.Key != "$ref") RewriteReferences(property.Value, renames);
                }

                break;
            case JsonArray array:
                foreach (var item in array)
                {
                    RewriteReferences(item, renames);
                }

                break;
            default:
                break;
        }
    }
}

/// <summary>
///     A definition moved from a fragment to the document root.
/// </summary>
internal sealed class HoistedDefinition(string name, string originalName, IReadOnlyList<string> sectionPath, JsonNode schema)
{
    /// <summary>
    ///     Its name in the document's <c>definitions</c>.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     Its name in the fragment.
    /// </summary>
    public string OriginalName { get; } = originalName;

    /// <summary>
    ///     The path of the section whose fragment defined it.
    /// </summary>
    public IReadOnlyList<string> SectionPath { get; } = sectionPath;

    /// <summary>
    ///     The definition, references rewritten.
    /// </summary>
    public JsonNode Schema { get; } = schema;

    /// <summary>
    ///     Set when the definition describes an object that something else describes too, so closing the definition would
    ///     reject that other schema's keys: it is left open, and the merged object is closed instead.
    /// </summary>
    public bool Open { get; set; }
}
