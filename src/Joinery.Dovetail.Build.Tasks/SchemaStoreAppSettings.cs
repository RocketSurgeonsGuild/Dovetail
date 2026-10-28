using System.Text.Json.Nodes;

namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     SchemaStore's appsettings.json schema (Apache-2.0, see THIRD-PARTY-NOTICES.txt), pinned in this assembly. Application
///     schemas embed it rather than <c>$ref</c>-ing https://json.schemastore.org/appsettings.json: editors that can't (or may
///     not) download it would otherwise skip validation entirely.
/// </summary>
internal static class SchemaStoreAppSettings
{
    /// <summary>
    ///     The name of the definition the schema is embedded under.
    /// </summary>
    public const string DefinitionName = "SchemaStoreAppSettings";

    /// <summary>
    ///     A reference to the embedded schema.
    /// </summary>
    public const string Reference = "#/definitions/" + DefinitionName;

    /// <summary>
    ///     Where the pinned copy comes from.
    /// </summary>
    public const string SourceUrl = "https://json.schemastore.org/appsettings.json";

    private const string ResourceName = "Joinery.Dovetail.Build.Tasks.SchemaStore.appsettings.json";

    /// <summary>
    ///     The schema, ready to embed under <c>definitions/SchemaStoreAppSettings</c>: without <c>$schema</c>, <c>$id</c> and
    ///     <c>title</c> (it is no longer a document of its own), and with every local <c>$ref</c> rewritten to point into that
    ///     definition.
    /// </summary>
    public static JsonObject Load()
    {
        using var stream = typeof(SchemaStoreAppSettings).Assembly.GetManifestResourceStream(ResourceName)
         ?? throw new InvalidOperationException($"The embedded resource {ResourceName} is missing.");
        var schema = JsonNode.Parse(stream)?.AsObject() ?? throw new InvalidOperationException($"The embedded resource {ResourceName} is empty.");
        schema.Remove("$schema");
        schema.Remove("$id");
        schema.Remove("title");
        RelocateReferences(schema);
        return schema;
    }

    private static void RelocateReferences(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject value:
                if (value["$ref"] is JsonValue reference
                 && reference.TryGetValue<string>(out var target)
                 && target.StartsWith("#", StringComparison.Ordinal))
                {
                    value["$ref"] = Reference + target.Substring(1);
                }

                foreach (var property in value)
                {
                    if (property.Key != "$ref") RelocateReferences(property.Value);
                }

                break;
            case JsonArray array:
                foreach (var item in array)
                {
                    RelocateReferences(item);
                }

                break;
            default:
                break;
        }
    }
}
