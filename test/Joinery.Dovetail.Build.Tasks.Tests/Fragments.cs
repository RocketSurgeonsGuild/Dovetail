using System.Text.Json.Nodes;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     Builds configuration schema fragments in the shape Dovetail's generator emits: an inline section object whose keys each
///     have a case-insensitive <c>patternProperties</c> twin, object types in the fragment's <c>definitions</c> (no <c>type</c>),
///     used through <c>allOf</c> + <c>$ref</c> wrappers.
/// </summary>
internal static class Fragments
{
    public static ConfigurationSchemaFragment Parse(string section, JsonNode schema, string source = "Library") => Parse(section, schema.ToJsonString(), source);

    public static ConfigurationSchemaFragment Parse(string section, string json, string source = "Library") =>
        ConfigurationSchemaFragment.TryParse(ConfigurationSchemaFragment.VersionPrefix + section, json, source, out var problem)
     ?? throw new InvalidOperationException($"Not a valid fragment: {problem}");

    /// <summary>
    ///     A section root.
    /// </summary>
    public static JsonObject Section(string title, JsonObject properties, JsonObject? definitions = null, string? description = null)
    {
        var section = new JsonObject { ["type"] = "object", ["title"] = title, };
        if (description is not null) section["description"] = description;
        AddProperties(section, properties);
        if (definitions is not null) section["definitions"] = definitions;
        return section;
    }

    /// <summary>
    ///     An object type, for a fragment's <c>definitions</c>.
    /// </summary>
    public static JsonObject Definition(JsonObject properties, string? description = null)
    {
        var definition = new JsonObject();
        if (description is not null) definition["description"] = description;
        AddProperties(definition, properties);
        return definition;
    }

    public static JsonObject Properties(params (string Name, JsonNode Schema)[] properties) =>
        new(properties.Select(z => new KeyValuePair<string, JsonNode?>(z.Name, z.Schema)));

    public static JsonObject Definitions(params (string Name, JsonNode Schema)[] definitions) => Properties(definitions);

    /// <summary>
    ///     A use of one of the fragment's object types.
    /// </summary>
    public static JsonObject Uses(string definition, bool nullable = false, string? description = null)
    {
        var use = new JsonObject { ["type"] = nullable ? new JsonArray("object", "null") : "object", };
        if (description is not null) use["description"] = description;
        use["allOf"] = new JsonArray(new JsonObject { ["$ref"] = "#/definitions/" + definition, });
        return use;
    }

    public static JsonObject String(string? description = null) => Scalar("string", description);

    public static JsonObject Integer(string? description = null) => Scalar("integer", description);

    public static JsonObject Boolean() => Scalar("boolean", null);

    public static JsonObject ArrayOf(JsonNode items) => new() { ["type"] = "array", ["items"] = items, };

    public static JsonObject DictionaryOf(JsonNode values) => new() { ["type"] = "object", ["additionalProperties"] = values, };

    private static JsonObject Scalar(string type, string? description)
    {
        var scalar = new JsonObject { ["type"] = type, };
        if (description is not null) scalar["description"] = description;
        return scalar;
    }

    private static void AddProperties(JsonObject schema, JsonObject properties)
    {
        var patternProperties = new JsonObject();
        foreach (var property in properties)
        {
            patternProperties[CaseInsensitivePattern.For(property.Key)] = property.Value?.DeepClone();
        }

        schema["properties"] = properties;
        schema["patternProperties"] = patternProperties;
    }
}
