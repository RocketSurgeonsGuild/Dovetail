using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     Validates a JSON instance against a draft-07 schema document, for the keywords Dovetail's schemas use: local <c>$ref</c>,
///     <c>type</c>, <c>enum</c>, <c>const</c>, <c>pattern</c>, <c>minLength</c>, <c>minItems</c>, <c>uniqueItems</c>, <c>required</c>, <c>properties</c>, <c>patternProperties</c>,
///     <c>additionalProperties</c>, <c>propertyNames</c>, <c>items</c>, <c>allOf</c>, <c>anyOf</c>, <c>oneOf</c>, <c>not</c> and
///     <c>if</c>/<c>then</c>/<c>else</c>. The test project's Corvus package only checks schemas against the meta-schema, and
///     this saves a validator package for the few tests that check instances. Unknown keywords (<c>format</c>, annotations, the
///     editor-only <c>patternErrorMessage</c>) are ignored, like a validator that doesn't assert formats. Patterns are
///     ECMA-262, the dialect JSON Schema specifies.
/// </summary>
internal sealed class SchemaInstanceValidator(JsonNode document)
{
    private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(2);

    /// <summary>
    ///     The reasons <paramref name="instance" /> doesn't satisfy the document's root schema (empty when it does).
    /// </summary>
    public IReadOnlyList<string> Validate(string instanceJson)
    {
        var errors = new List<string>();
        Check(document, JsonNode.Parse(instanceJson), "#", errors);
        return errors;
    }

    public bool IsValid(string instanceJson) => Validate(instanceJson).Count == 0;

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

    private void Check(JsonNode? schema, JsonNode? instance, string path, List<string> errors)
    {
        switch (schema)
        {
            case JsonValue value when value.GetValueKind() == JsonValueKind.True:
                return;
            case JsonValue value when value.GetValueKind() == JsonValueKind.False:
                errors.Add($"{path}: nothing is allowed here");
                return;
            case JsonObject:
                break;
            default:
                return;
        }

        var keywords = (JsonObject)schema;
        if (keywords["$ref"] is JsonValue reference)
        {
            // Draft-07: siblings of $ref are ignored.
            Check(Resolve(document, reference.GetValue<string>()), instance, path, errors);
            return;
        }

        if (keywords["type"] is { } type && !TypeMatches(type, instance)) errors.Add($"{path}: expected {type.ToJsonString()}, found {Kind(instance)}");
        if (keywords["enum"] is JsonArray values && !values.Any(z => JsonNode.DeepEquals(z, instance))) errors.Add($"{path}: not one of {values.ToJsonString()}");
        if (keywords["const"] is { } constant && !JsonNode.DeepEquals(constant, instance)) errors.Add($"{path}: not {constant.ToJsonString()}");

        if (instance is JsonValue scalar && scalar.TryGetValue<string>(out var text))
        {
            if (keywords["pattern"] is JsonValue pattern && !Matches(pattern.GetValue<string>(), text)) errors.Add($"{path}: \"{text}\" doesn't match {pattern.GetValue<string>()}");
            if (keywords["minLength"] is JsonValue minLength && text.Length < minLength.GetValue<int>()) errors.Add($"{path}: shorter than {minLength.GetValue<int>()}");
        }

        if (instance is JsonObject properties) CheckObject(keywords, properties, path, errors);
        if (instance is JsonArray array)
        {
            if (keywords["minItems"] is JsonValue minItems && array.Count < minItems.GetValue<int>()) errors.Add($"{path}: fewer than {minItems.GetValue<int>()} items");
            if (keywords["uniqueItems"] is JsonValue { } unique && unique.GetValue<bool>() && array.Where((z, index) => array.Take(index).Any(other => JsonNode.DeepEquals(z, other))).Any())
            {
                errors.Add($"{path}: items are not unique");
            }
        }

        if (instance is JsonArray items && keywords["items"] is { } itemSchema)
        {
            for (var index = 0; index < items.Count; index++)
            {
                Check(itemSchema, items[index], $"{path}/{index}", errors);
            }
        }

        CheckCombinators(keywords, instance, path, errors);
    }

    private void CheckObject(JsonObject keywords, JsonObject instance, string path, List<string> errors)
    {
        if (keywords["required"] is JsonArray required)
        {
            foreach (var name in required.Select(z => z!.GetValue<string>()).Where(z => !instance.ContainsKey(z)))
            {
                errors.Add($"{path}: {name} is required");
            }
        }

        var declared = keywords["properties"] as JsonObject;
        var patterns = keywords["patternProperties"] as JsonObject;
        foreach (var property in instance)
        {
            var matched = false;
            if (declared?[property.Key] is { } declaredSchema)
            {
                matched = true;
                Check(declaredSchema, property.Value, $"{path}/{property.Key}", errors);
            }

            foreach (var pattern in patterns ?? [])
            {
                if (!Matches(pattern.Key, property.Key)) continue;
                matched = true;
                Check(pattern.Value, property.Value, $"{path}/{property.Key}", errors);
            }

            if (!matched && keywords["additionalProperties"] is { } additional) Check(additional, property.Value, $"{path}/{property.Key}", errors);
            if (keywords["propertyNames"] is { } names) Check(names, JsonValue.Create(property.Key), $"{path}/{property.Key} (key)", errors);
        }
    }

    private void CheckCombinators(JsonObject keywords, JsonNode? instance, string path, List<string> errors)
    {
        if (keywords["allOf"] is JsonArray allOf)
        {
            foreach (var member in allOf)
            {
                Check(member, instance, path, errors);
            }
        }

        if (keywords["anyOf"] is JsonArray anyOf && !anyOf.Any(z => Passes(z, instance))) errors.Add($"{path}: matches none of the {anyOf.Count} anyOf alternatives");
        if (keywords["oneOf"] is JsonArray oneOf && oneOf.Count(z => Passes(z, instance)) != 1) errors.Add($"{path}: doesn't match exactly one oneOf alternative");
        if (keywords["not"] is { } not && Passes(not, instance)) errors.Add($"{path}: matches a schema it must not");
        if (keywords["if"] is { } condition)
        {
            var branch = Passes(condition, instance) ? keywords["then"] : keywords["else"];
            if (branch is not null) Check(branch, instance, path, errors);
        }
    }

    private bool Passes(JsonNode? schema, JsonNode? instance)
    {
        var errors = new List<string>();
        Check(schema, instance, "#", errors);
        return errors.Count == 0;
    }

    private static bool Matches(string pattern, string text) => new Regex(pattern, RegexOptions.ECMAScript | RegexOptions.CultureInvariant, MatchTimeout).IsMatch(text);

    private static bool TypeMatches(JsonNode type, JsonNode? instance) => type switch
    {
        JsonArray names => names.Any(z => TypeMatches(z!, instance)),
        JsonValue name => name.GetValue<string>() switch
        {
            "object" => instance is JsonObject,
            "array" => instance is JsonArray,
            "string" => instance is JsonValue s && s.GetValueKind() == JsonValueKind.String,
            "number" => instance is JsonValue n && n.GetValueKind() == JsonValueKind.Number,
            "integer" => instance is JsonValue i && i.GetValueKind() == JsonValueKind.Number && i.ToJsonString().IndexOfAny(['.', 'e', 'E']) < 0,
            "boolean" => instance is JsonValue b && b.GetValueKind() is JsonValueKind.True or JsonValueKind.False,
            "null" => instance is null,
            _ => false,
        },
        _ => false,
    };

    private static string Kind(JsonNode? instance) => instance switch
    {
        null => "null",
        JsonObject => "object",
        JsonArray => "array",
        JsonValue value => value.GetValueKind().ToString().ToLowerInvariant(),
        _ => "unknown",
    };
}
