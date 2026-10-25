using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Draft7Schema = Corvus.Json.JsonSchema.Draft7.Schema;
using ValidationContext = Corvus.Json.ValidationContext;
using ValidationLevel = Corvus.Json.ValidationLevel;

namespace Dovetail.Build.Tasks.Tests;

internal static class SchemaAssert
{
    /// <summary>
    ///     What every file the tasks write has to be: a valid draft-07 schema (checked against the meta-schema), whose local
    ///     <c>$ref</c>s all resolve, that references nothing remote, and whose Dovetail patterns are valid ECMA-262.
    /// </summary>
    public static JsonObject ShouldBeWellFormed(string json)
    {
        ShouldBeValidDraft07(json);
        var document = JsonNode.Parse(json)!.AsObject();
        foreach (var reference in References(document))
        {
            reference.ShouldStartWith("#", customMessage: $"{reference} is not a local reference");
            Resolve(document, reference).ShouldNotBeNull($"{reference} doesn't resolve");
        }

        foreach (var pattern in DovetailPatterns(document))
        {
            Should.NotThrow(() => new Regex(pattern, RegexOptions.ECMAScript), $"{pattern} isn't an ECMA-262 pattern");
        }

        return document;
    }

    public static void ShouldBeValidDraft07(string json)
    {
        // Corvus checks "format": "regex" for a pattern with a stricter dialect than ECMA-262 and rejects valid ones (\d in
        // Dovetail's own, named groups in SchemaStore's), failing the schemas around them too. Every pattern of Dovetail's is
        // checked as an ECMAScript regular expression by ShouldBeWellFormed instead, so neutralize them here.
        var result = Draft7Schema.Parse(NeutralizePatterns(json)).Validate(ValidationContext.ValidContext, ValidationLevel.Detailed);
        var failures = result.Results.Where(z => !z.Valid).Select(z => $"{z.Location}: {z.Message}").ToArray();
        failures.ShouldBeEmpty(string.Join(Environment.NewLine, failures));
    }

    private static string NeutralizePatterns(string json)
    {
        var document = JsonNode.Parse(json)!;
        foreach (var schema in Objects(document).ToArray())
        {
            if (schema["pattern"] is JsonValue pattern && pattern.TryGetValue<string>(out _)) schema["pattern"] = ".*";

            // patternProperties keys are patterns too (SchemaStore's use named groups); keep the schemas, number the keys.
            if (schema["patternProperties"] is JsonObject patternProperties)
            {
                var entries = patternProperties.ToArray();
                patternProperties.Clear();
                for (var index = 0; index < entries.Length; index++)
                {
                    patternProperties[$"^p{index}$"] = entries[index].Value;
                }
            }
        }

        return document.ToJsonString();
    }

    public static bool IsValidDraft07(string json) => Draft7Schema.Parse(json).Validate(ValidationContext.ValidContext, ValidationLevel.Flag).IsValid;

    /// <summary>
    ///     Resolves a local JSON pointer reference (<c>#/definitions/X</c>) in <paramref name="document" />.
    /// </summary>
    public static JsonNode? Resolve(JsonNode document, string reference)
    {
        var node = document;
        foreach (var segment in reference.TrimStart('#').Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            var name = segment.Replace("~1", "/", StringComparison.Ordinal).Replace("~0", "~", StringComparison.Ordinal);
            node = node switch
            {
                JsonObject value => value[name],
                JsonArray array when int.TryParse(name, out var index) && index < array.Count => array[index],
                _ => null,
            };
        }

        return node;
    }

    /// <summary>
    ///     Every object in <paramref name="node" />, depth first.
    /// </summary>
    public static IEnumerable<JsonObject> Objects(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject value:
                yield return value;
                foreach (var child in value.SelectMany(z => Objects(z.Value)))
                {
                    yield return child;
                }

                break;
            case JsonArray array:
                foreach (var child in array.SelectMany(Objects))
                {
                    yield return child;
                }

                break;
            default:
                break;
        }
    }

    public static IEnumerable<string> References(JsonNode node) =>
        Objects(node).Select(z => z["$ref"]).OfType<JsonValue>().Select(z => z.GetValue<string>());

    /// <summary>
    ///     The Dovetail part of an application schema: the root without SchemaStore, or the second <c>allOf</c> member.
    /// </summary>
    public static JsonObject DovetailPart(JsonObject document) => document["allOf"] is JsonArray allOf ? allOf[1]!.AsObject() : document;

    /// <summary>
    ///     The schema of the value at <paramref name="path" /> (section path segments and property keys), following
    ///     <c>properties</c> from the Dovetail part and resolving <c>$ref</c>s on the way.
    /// </summary>
    public static JsonObject Section(JsonObject document, params string[] path)
    {
        var node = DovetailPart(document);
        foreach (var segment in path)
        {
            node = Dereference(document, node["properties"]![segment]!.AsObject());
        }

        return node;
    }

    /// <summary>
    ///     Follows <c>{"$ref": ...}</c> to what it references.
    /// </summary>
    public static JsonObject Dereference(JsonObject document, JsonObject node)
    {
        while (node["$ref"] is JsonValue reference)
        {
            node = Resolve(document, reference.GetValue<string>())!.AsObject();
        }

        return node;
    }

    /// <summary>
    ///     Dovetail's definitions: every one but SchemaStore's.
    /// </summary>
    public static IEnumerable<KeyValuePair<string, JsonNode?>> DovetailDefinitions(JsonObject document) =>
        document["definitions"] is JsonObject definitions ? definitions.Where(z => z.Key != SchemaStoreAppSettings.DefinitionName) : [];

    private static IEnumerable<string> DovetailPatterns(JsonObject document) =>
        Objects(DovetailPart(document))
           .Concat(DovetailDefinitions(document).SelectMany(z => Objects(z.Value)))
           .SelectMany(
                z => ( z["patternProperties"] is JsonObject patternProperties ? patternProperties.Select(p => p.Key) : [] )
                   .Concat(z["pattern"] is JsonValue keyword && keyword.TryGetValue<string>(out var text) ? [text] : [])
                   .Concat(z["propertyNames"] is JsonObject { } names && names["pattern"] is JsonValue pattern ? [pattern.GetValue<string>()] : [])
            );
}
