using System.Text.Json.Nodes;

namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     Builds the Dovetail part of an application's appsettings.schema.json from the section tree, hoisting the fragments'
///     definitions into a <see cref="DefinitionRegistry" />.
/// </summary>
/// <remarks>
///     <para>
///         Strictness lives here, not in the fragments (which stay permissive): every object with <c>properties</c> (section roots
///         and the hoisted definitions) is closed with a <c>propertyNames</c> pattern that lists those keys ignoring case. That
///         rejects typos while still accepting <c>message</c> for <c>Message</c>, which <c>additionalProperties: false</c>
///         wouldn't. The configuration root, shared parents (nodes that only exist because of deeper sections), definition
///         wrappers (<c>allOf</c> + <c>$ref</c>, no <c>properties</c>) and dictionaries (<c>additionalProperties</c>) stay open.
///     </para>
///     <para>
///         Several schemas can describe the same object: a section several fragments declare, a fragment's property that is also
///         a section of its own, or a property two of those both declare. Closing each of them would make every key fail one of
///         them, so the object becomes an <c>allOf</c> of the schemas with their own level left open (inline objects are copied
///         open; a definition is marked <see cref="HoistedDefinition.Open" /> unless its keys already cover the others'), and it
///         is closed once on the merged object, with everyone's keys. Keys they share are merged the same way, one level down.
///     </para>
///     <para>
///         Keys also get a case-insensitive <c>patternProperties</c> entry with the same subschema (fragments already carry
///         them for their own keys), so values under differently-cased keys are validated too.
///     </para>
/// </remarks>
internal sealed class ApplicationSchemaBuilder(bool strict, DefinitionRegistry definitions)
{
    private const string AnyKey = "*";
    private readonly HashSet<string> _merging = [with(StringComparer.Ordinal)];

    /// <summary>
    ///     The schema of the configuration root: an open object with one property per top-level section.
    /// </summary>
    public JsonObject Build(SectionNode root) => OpenObject([], root.Children);

    /// <summary>
    ///     The definitions <see cref="Build" /> referenced, ordered by name: the sections, and the fragments' hoisted definitions,
    ///     closed unless <see cref="Build" /> left them open. Call after it.
    /// </summary>
    public IReadOnlyList<KeyValuePair<string, JsonNode?>> BuildDefinitions() =>
        definitions
           .Definitions
           .Select(z => new KeyValuePair<string, JsonNode?>(z.Name, CloseDefinition(z)))
           .Concat(definitions.Locations.Select(z => new KeyValuePair<string, JsonNode?>(z.Key, z.Value)))
           .OrderBy(z => z.Key, StringComparer.Ordinal)
           .ToArray();

    private static string Display(IEnumerable<string> path) => string.Join(":", path);

    private static IReadOnlyList<string> Append(IReadOnlyList<string> path, string segment) => [.. path, segment];

    private static bool IsClosable(JsonObject schema) =>
        schema["properties"] is JsonObject
     && !schema.ContainsKey("additionalProperties")
     && !schema.ContainsKey("propertyNames")
     && Allows(schema["type"], "object", whenMissing: true);

    private static bool Allows(JsonNode? type, string name, bool whenMissing) => type switch
    {
        null => whenMissing,
        JsonValue value => value.TryGetValue<string>(out var single) && single == name,
        JsonArray values => values.Any(z => Allows(z, name, whenMissing: false)),
        _ => false,
    };

    private static JsonNode AllOf(IReadOnlyList<JsonNode> parts) => parts.Count == 1 ? parts[0] : new JsonObject { ["allOf"] = new JsonArray([.. parts]), };

    private static void AddProperty(JsonObject properties, JsonObject patternProperties, string name, JsonNode schema)
    {
        properties[name] = schema;
        var pattern = CaseInsensitivePattern.For(name);
        if (!patternProperties.ContainsKey(pattern)) patternProperties[pattern] = schema.DeepClone();
    }

    private static JsonNode MergedType(IEnumerable<JsonNode> parts) =>
        parts.All(z => z is JsonObject value && Allows(value["type"], "null", whenMissing: false)) ? new JsonArray("object", "null") : "object";

    private Part Classify(JsonNode? schema) => schema switch
    {
        JsonObject value when IsClosable(value) => new(PartKind.Inline, value, null),
        JsonObject value when definitions.Resolve(value) is { Schema: JsonObject holder, } definition && IsClosable(holder) => new(PartKind.Definition, holder, definition),
        JsonObject value when value["items"] is JsonObject && Allows(value["type"], "array", whenMissing: false) => new(PartKind.Array, null, null),
        JsonObject value when value["additionalProperties"] is JsonObject && !value.ContainsKey("properties") => new(PartKind.Dictionary, null, null),
        _ => new(PartKind.Other, null, null),
    };

    /// <summary>
    ///     The keyword that describes the elements when every part is an array (<c>items</c>) or every part a dictionary
    ///     (<c>additionalProperties</c>).
    /// </summary>
    private static string? ElementKeyword(IReadOnlyList<Part> kinds) =>
        kinds.All(z => z.Kind == PartKind.Array) ? "items" : kinds.All(z => z.Kind == PartKind.Dictionary) ? "additionalProperties" : null;

    /// <summary>
    ///     The root and shared parents: open, since other code may bind other keys there.
    /// </summary>
    private JsonObject OpenObject(IReadOnlyList<string> path, IReadOnlyList<SectionNode> children)
    {
        var properties = new JsonObject();
        var patternProperties = new JsonObject();
        foreach (var child in children)
        {
            AddProperty(properties, patternProperties, child.Name, Location(Append(path, child.Name), [], child));
        }

        var result = new JsonObject { ["type"] = "object", ["properties"] = properties, };
        if (patternProperties.Count > 0) result["patternProperties"] = patternProperties;
        return result;
    }

    /// <summary>
    ///     <see cref="Emit" />'s schema for <paramref name="path" />, stored as a definition: a reference to it.
    /// </summary>
    private JsonObject Location(IReadOnlyList<string> path, IReadOnlyList<JsonNode> inherited, SectionNode? section) =>
        new() { ["$ref"] = definitions.AddLocation(path, Emit(path, inherited, section)), };

    /// <summary>
    ///     The schema of the value at <paramref name="path" />: the schemas inherited from the parent level, plus the fragments and
    ///     subsections of <paramref name="section" />.
    /// </summary>
    private JsonNode Emit(IReadOnlyList<string> path, IReadOnlyList<JsonNode> inherited, SectionNode? section)
    {
        var parts = inherited
                   .Concat(section?.Fragments.Select(z => definitions.Root(z, path)) ?? [])
                   .GroupBy(z => z.ToJsonString(), StringComparer.Ordinal)
                   .Select(z => z.First())
                   .ToArray();
        var children = section?.Children ?? [];
        var isSection = section?.Fragments.Count > 0;

        if (parts.Length == 0) return OpenObject(path, children);
        if (parts.Length == 1 && children.Count == 0) return Close(parts[0], path);

        var kinds = parts.Select(Classify).ToArray();
        if (kinds.All(z => z.IsObject)) return MergeObjects(path, parts, kinds, children, isSection);

        var elements = children.Count == 0 ? ElementKeyword(kinds) : null;
        return elements is null ? Combine(path, parts, kinds, children, isSection) : MergeValues(path, parts, elements);
    }

    private JsonNode MergeObjects(IReadOnlyList<string> path, IReadOnlyList<JsonNode> parts, IReadOnlyList<Part> kinds, IReadOnlyList<SectionNode> children, bool isSection)
    {
        // One inline object plus subsections: merge the subsections into its properties.
        if (parts.Count == 1 && kinds[0].Kind == PartKind.Inline) return MergeSubsections(path, kinds[0].Holder!, children);

        string? signature = null;
        if (kinds.All(z => z.Kind == PartKind.Definition))
        {
            // Definitions that accept the same keys at every level don't conflict: each can stay closed.
            if (children.Count == 0 && kinds.Skip(1).All(z => SameClosure(kinds[0].Holder!, z.Holder!, []))) return AllOf([.. parts.Select(z => z.DeepClone())]);

            // A recursive type met again below itself: the outer level made every decision already, so stop unrolling.
            signature = string.Join("|", kinds.Select(z => z.Definition!.Name).OrderBy(z => z, StringComparer.Ordinal));
            if (_merging.Contains(signature)) return AllOf([.. parts.Select(z => z.DeepClone())]);
        }

        var keys = MergedKey.Collect(kinds.Select(z => z.Holder!), children);
        var names = keys.Select(z => z.Name).ToArray();
        foreach (var part in kinds.Where(z => z.Kind == PartKind.Definition && !ContainsAll(z.Holder!, names)))
        {
            part.Definition!.Open = true;
        }

        var shared = new HashSet<string>(keys.Where(z => z.IsShared).Select(z => z.Name), StringComparer.OrdinalIgnoreCase);
        var result = new JsonObject { ["type"] = MergedType(parts), };
        if (isSection) result["title"] = Display(path);
        result["allOf"] = new JsonArray(
            [
                .. parts.Select((part, index) => kinds[index].Kind == PartKind.Inline
                                    ? CloseObject(kinds[index].Holder!, path, closeSelf: false, skip: shared)
                                    : part.DeepClone()
                ),
            ]
        );

        if (signature is not null) _merging.Add(signature);
        var properties = new JsonObject();
        var patternProperties = new JsonObject();
        foreach (var key in keys.Where(z => z.IsShared))
        {
            AddProperty(properties, patternProperties, key.Name, Location(Append(path, key.Name), key.Schemas, key.Section));
        }

        if (signature is not null) _merging.Remove(signature);

        if (properties.Count > 0)
        {
            result["properties"] = properties;
            result["patternProperties"] = patternProperties;
        }

        if (strict) result["propertyNames"] = PropertyNames(path, names);
        return result;
    }

    private JsonObject MergeSubsections(IReadOnlyList<string> path, JsonObject schema, IReadOnlyList<SectionNode> children)
    {
        var keys = MergedKey.Collect([schema], children);
        var properties = keys
                        .Select(key => (key.Name, key.IsShared
                                            ? Location(Append(path, key.Name), key.Schemas, key.Section)
                                            : Close(key.Schemas[0], Append(path, key.Name))))
                        .ToArray();
        var changed = new HashSet<string>(keys.Where(z => z.IsShared).Select(z => z.Name), StringComparer.OrdinalIgnoreCase);
        return Rebuild(schema, path, properties, changed, closeSelf: true);
    }

    /// <summary>
    ///     Arrays (or dictionaries) declared by several schemas: their elements are merged as one value.
    /// </summary>
    private JsonObject MergeValues(IReadOnlyList<string> path, IReadOnlyList<JsonNode> parts, string keyword) =>
        new()
        {
            ["allOf"] = new JsonArray([.. parts.Select(z => CopyOpen((JsonObject)z, path, skip: keyword))]),
            [keyword] = Emit(Append(path, AnyKey), [.. parts.Select(z => z[keyword]!)], null),
        };

    /// <summary>
    ///     Schemas that can't be merged (a dictionary and an object, say) are combined as declared. Everything they reach is left
    ///     open, since any of it may describe the same keys differently.
    /// </summary>
    private JsonNode Combine(IReadOnlyList<string> path, IReadOnlyList<JsonNode> parts, IReadOnlyList<Part> kinds, IReadOnlyList<SectionNode> children, bool isSection)
    {
        foreach (var definition in definitions.Reachable(parts))
        {
            definition.Open = true;
        }

        var list = parts.Select((part, index) => kinds[index].Kind == PartKind.Inline ? CloseObject(kinds[index].Holder!, path, closeSelf: false) : Close(part, path)).ToList();
        if (children.Count > 0) list.Add(OpenObject(path, children));
        if (list.Count == 1) return list[0];

        var result = new JsonObject();
        if (isSection) result["title"] = Display(path);
        result["allOf"] = new JsonArray([.. list]);
        return result;
    }

    /// <summary>
    ///     A copy of a schema nothing else describes: closed when it is an inline object, along with everything nested in it.
    ///     Definitions it uses are closed by <see cref="BuildDefinitions" />.
    /// </summary>
    private JsonNode Close(JsonNode schema, IReadOnlyList<string> path) => schema switch
    {
        JsonObject value when IsClosable(value) => CloseObject(value, path, closeSelf: true),
        JsonObject value => CopyOpen(value, path),
        _ => schema.DeepClone(),
    };

    private JsonObject CloseObject(JsonObject schema, IReadOnlyList<string> path, bool closeSelf, ISet<string>? skip = null, string? context = null)
    {
        var properties = schema["properties"]!
                        .AsObject()
                        .Where(z => z.Value is not null && skip?.Contains(z.Key) != true)
                        .Select(z => (z.Key, Close(z.Value!, Append(path, z.Key))))
                        .ToArray();
        return Rebuild(schema, path, properties, changed: null, closeSelf, context);
    }

    /// <summary>
    ///     Copies an object schema with <paramref name="properties" /> as its properties, and closes it when
    ///     <paramref name="closeSelf" /> is set. The schema's own case-insensitive <c>patternProperties</c> entries stay, except
    ///     those of keys that were left out or <paramref name="changed" />; those get new ones.
    /// </summary>
    private JsonObject Rebuild(
        JsonObject schema,
        IReadOnlyList<string> path,
        IReadOnlyList<(string Name, JsonNode Schema)> properties,
        ISet<string>? changed,
        bool closeSelf,
        string? context = null
    )
    {
        var result = new JsonObject();
        foreach (var keyword in schema)
        {
            switch (keyword.Key)
            {
                case "properties":
                    WriteProperties(result, schema, path, properties, changed);
                    break;
                case "patternProperties":
                    // Written along with the properties.
                    break;
                default:
                    result[keyword.Key] = Keyword(keyword.Key, keyword.Value, path);
                    break;
            }
        }

        if (closeSelf && strict) result["propertyNames"] = PropertyNames(path, properties.Select(z => z.Name), context);
        return result;
    }

    private void WriteProperties(JsonObject result, JsonObject schema, IReadOnlyList<string> path, IReadOnlyList<(string Name, JsonNode Schema)> properties, ISet<string>? changed)
    {
        var resultProperties = new JsonObject();
        var resultPatternProperties = new JsonObject();
        var kept = new HashSet<string>(properties.Select(z => z.Name), StringComparer.OrdinalIgnoreCase);
        foreach (var (name, value) in properties)
        {
            resultProperties[name] = value;
        }

        if (schema["patternProperties"] is JsonObject patternProperties)
        {
            var originalNames = new CaseInsensitivePattern.NameLookup(schema["properties"] is JsonObject originalProperties ? [.. originalProperties.Select(z => z.Key)] : []);
            foreach (var entry in patternProperties)
            {
                var key = originalNames.Find(entry.Key);
                if (key is not null && ( !kept.Contains(key) || changed?.Contains(key) == true )) continue;
                resultPatternProperties[entry.Key] = entry.Value is null ? null : Close(entry.Value, Append(path, key ?? AnyKey));
            }
        }

        foreach (var (name, value) in properties)
        {
            if (changed?.Contains(name) != true) continue;
            var pattern = CaseInsensitivePattern.For(name);
            if (!resultPatternProperties.ContainsKey(pattern)) resultPatternProperties[pattern] = value.DeepClone();
        }

        result["properties"] = resultProperties;
        if (resultPatternProperties.Count > 0) result["patternProperties"] = resultPatternProperties;
    }

    /// <summary>
    ///     Copies a schema that stays open (a definition wrapper, a dictionary, a scalar), closing the inline objects nested in it.
    /// </summary>
    private JsonObject CopyOpen(JsonObject schema, IReadOnlyList<string> path, string? skip = null)
    {
        var result = new JsonObject();
        var names = new CaseInsensitivePattern.NameLookup(schema["properties"] is JsonObject properties ? [.. properties.Select(z => z.Key)] : []);
        foreach (var keyword in schema)
        {
            if (keyword.Key == skip) continue;
            result[keyword.Key] = keyword switch
            {
                { Key: "properties", Value: JsonObject value, } => new JsonObject(
                    value.Select(z => new KeyValuePair<string, JsonNode?>(z.Key, z.Value is null ? null : Close(z.Value, Append(path, z.Key))))
                ),
                { Key: "patternProperties", Value: JsonObject value, } => new JsonObject(
                    value.Select(
                        z => new KeyValuePair<string, JsonNode?>(
                            z.Key,
                            z.Value is null ? null : Close(z.Value, Append(path, names.Find(z.Key) ?? AnyKey))
                        )
                    )
                ),
                _ => Keyword(keyword.Key, keyword.Value, path),
            };
        }

        return result;
    }

    /// <summary>
    ///     Copies a keyword, closing the inline objects reachable through the ones that describe values: array items, dictionary
    ///     values and the alternatives of <c>anyOf</c>/<c>oneOf</c>. <c>allOf</c> members are copied as they are, since closing
    ///     each of them would reject the others' keys.
    /// </summary>
    private JsonNode? Keyword(string name, JsonNode? value, IReadOnlyList<string> path) => (name, value) switch
    {
        ("items", JsonArray items) => new JsonArray([.. items.Select(z => z is null ? null : Close(z, Append(path, AnyKey)))]),
        ("items" or "additionalItems" or "additionalProperties", JsonObject schema) => Close(schema, Append(path, AnyKey)),
        ("anyOf" or "oneOf", JsonArray alternatives) => new JsonArray([.. alternatives.Select(z => z is null ? null : Close(z, path))]),
        _ => value?.DeepClone(),
    };

    private JsonNode CloseDefinition(HoistedDefinition definition)
    {
        // Messages for objects nested inline in a definition name it as their section.
        IReadOnlyList<string> path = [.. definition.SectionPath, definition.OriginalName];
        return definition.Schema switch
        {
            JsonObject value when IsClosable(value) => CloseObject(
                value,
                path,
                closeSelf: !definition.Open,
                context: $"Unknown key in \"{definition.OriginalName}\" (section \"{Display(definition.SectionPath)}\")"
            ),
            JsonObject value => CopyOpen(value, path),
            var value => value.DeepClone(),
        };
    }

    private static bool ContainsAll(JsonObject schema, IEnumerable<string> names)
    {
        var own = new HashSet<string>(schema["properties"]!.AsObject().Select(z => z.Key), StringComparer.OrdinalIgnoreCase);
        return names.All(own.Contains);
    }

    /// <summary>
    ///     Whether two object schemas accept the same keys (ignoring case) at every level, following definitions; recursion is
    ///     assumed to agree (a bisimulation).
    /// </summary>
    private bool SameClosure(JsonObject left, JsonObject right, HashSet<(JsonObject, JsonObject)> assumed)
    {
        if (ReferenceEquals(left, right) || !assumed.Add((left, right))) return true;

        var rightProperties = new Dictionary<string, JsonNode?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in right["properties"]!.AsObject())
        {
            rightProperties[property.Key] = property.Value;
        }

        var leftProperties = left["properties"]!.AsObject();
        var leftNames = new HashSet<string>(leftProperties.Select(z => z.Key), StringComparer.OrdinalIgnoreCase);
        return leftNames.SetEquals(rightProperties.Keys)
         && leftProperties.All(z => rightProperties.TryGetValue(z.Key, out var other) && SameValueClosure(z.Value, other, assumed));
    }

    private bool SameValueClosure(JsonNode? left, JsonNode? right, HashSet<(JsonObject, JsonObject)> assumed)
    {
        var leftPart = Classify(left);
        var rightPart = Classify(right);
        if (leftPart.IsObject || rightPart.IsObject) return leftPart.IsObject && rightPart.IsObject && SameClosure(leftPart.Holder!, rightPart.Holder!, assumed);

        // Neither is an object: compare what nests under them.
        foreach (var keyword in new[] { "items", "additionalProperties", })
        {
            var leftValue = ( left as JsonObject )?[keyword] as JsonObject;
            var rightValue = ( right as JsonObject )?[keyword] as JsonObject;
            if (leftValue is null != rightValue is null) return false;
            if (leftValue is not null && !SameValueClosure(leftValue, rightValue, assumed)) return false;
        }

        return true;
    }

    private JsonObject PropertyNames(IReadOnlyList<string> path, IEnumerable<string> names, string? context = null)
    {
        var known = names.ToArray();
        var unknownKey = context ?? $"Unknown key in section \"{Display(path)}\"";
        var knownKeys = known.Length == 0 ? "none" : string.Join(", ", known);
        return new()
        {
            ["pattern"] = CaseInsensitivePattern.For(known),
            ["patternErrorMessage"] = $"{unknownKey}. Known keys (case-insensitive): {knownKeys}.",
            ["doNotSuggest"] = true,
        };
    }

    private enum PartKind
    {
        Inline,
        Definition,
        Array,
        Dictionary,
        Other,
    }

    /// <summary>
    ///     What a schema describing a value is: an object (inline, or a wrapper around a definition, with
    ///     <see cref="Holder" /> the schema holding its <c>properties</c>), an array, a dictionary, or something else.
    /// </summary>
    private readonly struct Part(PartKind kind, JsonObject? holder, HoistedDefinition? definition)
    {
        public PartKind Kind { get; } = kind;

        public JsonObject? Holder { get; } = holder;

        public HoistedDefinition? Definition { get; } = definition;

        public bool IsObject => Kind is PartKind.Inline or PartKind.Definition;
    }

    /// <summary>
    ///     A key of an object several schemas describe.
    /// </summary>
    private sealed class MergedKey(string name)
    {
        /// <summary>
        ///     The key's display spelling: the ordinal-smallest of the spellings declared.
        /// </summary>
        public string Name { get; private set; } = name;

        /// <summary>
        ///     The key's schema in each schema that declares it, in their order.
        /// </summary>
        public List<JsonNode> Schemas { get; } = [];

        /// <summary>
        ///     The subsection of the same name, if any.
        /// </summary>
        public SectionNode? Section { get; private set; }

        /// <summary>
        ///     Whether more than one thing describes the key, so its value has to be merged rather than closed in place.
        /// </summary>
        public bool IsShared => Schemas.Count != 1 || Section is not null;

        /// <summary>
        ///     The keys of <paramref name="schemas" /> and <paramref name="children" />, ignoring case, in the order they first appear.
        /// </summary>
        public static IReadOnlyList<MergedKey> Collect(IEnumerable<JsonObject> schemas, IEnumerable<SectionNode> children)
        {
            var keys = new List<MergedKey>();
            var byName = new Dictionary<string, MergedKey>(StringComparer.OrdinalIgnoreCase);
            foreach (var schema in schemas)
            {
                foreach (var property in schema["properties"]!.AsObject())
                {
                    if (property.Value is not null) Get(property.Key).Schemas.Add(property.Value);
                }
            }

            foreach (var child in children)
            {
                Get(child.Name).Section = child;
            }

            return keys;

            MergedKey Get(string spelling)
            {
                if (byName.TryGetValue(spelling, out var key))
                {
                    if (string.CompareOrdinal(spelling, key.Name) < 0) key.Name = spelling;
                    return key;
                }

                byName.Add(spelling, key = new(spelling));
                keys.Add(key);
                return key;
            }
        }
    }
}
