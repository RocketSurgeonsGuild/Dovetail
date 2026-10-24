using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Dovetail.Build.Tasks;

/// <summary>
///     One configuration section's schema, as Dovetail's generator embeds it:
///     <c>[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:&lt;Section path&gt;", "&lt;draft-07 schema of the value&gt;")]</c>.
/// </summary>
/// <remarks>
///     The section's own object is inline. Object types used inside it live in the fragment's root <c>definitions</c> (names
///     matching <c>^[A-Za-z0-9._-]+$</c>, no <c>type</c> keyword) and are used through small wrappers,
///     <c>{"type": "object", "allOf": [{"$ref": "#/definitions/&lt;Name&gt;"}]}</c>; definitions may reference each other and
///     themselves. Fragments are permissive: strictness is the merge's job.
/// </remarks>
internal sealed class ConfigurationSchemaFragment
{
    /// <summary>
    ///     The prefix of every configuration schema metadata key, whatever its version.
    /// </summary>
    public const string KeyPrefix = "Dovetail.ConfigurationSchema.";

    /// <summary>
    ///     The prefix of the keys this version understands; the section path follows it.
    /// </summary>
    public const string VersionPrefix = KeyPrefix + "v1:";

    /// <summary>
    ///     The prefix of a reference to one of the fragment's definitions.
    /// </summary>
    public const string DefinitionReferencePrefix = "#/definitions/";

    private static readonly Regex DefinitionName = new("^[A-Za-z0-9._-]+$", RegexOptions.CultureInvariant);

    private ConfigurationSchemaFragment(IReadOnlyList<string> path, JsonNode schema, string json, string source)
    {
        Path = path;
        Schema = schema;
        Json = json;
        Source = source;
    }

    /// <summary>
    ///     The section path, split on <c>:</c>.
    /// </summary>
    public IReadOnlyList<string> Path { get; }

    /// <summary>
    ///     The schema of the section's value, including its <c>definitions</c>.
    /// </summary>
    public JsonNode Schema { get; }

    /// <summary>
    ///     The schema, re-serialized compactly: identical declarations compare equal.
    /// </summary>
    public string Json { get; }

    /// <summary>
    ///     The name of the assembly that declared the section.
    /// </summary>
    public string Source { get; }

    /// <summary>
    ///     Parses one metadata entry. Returns <see langword="null" /> and explains why in <paramref name="problem" /> when the key
    ///     belongs to another version or the entry is malformed.
    /// </summary>
    public static ConfigurationSchemaFragment? TryParse(string key, string? value, string source, out string? problem)
    {
        if (!key.StartsWith(VersionPrefix, StringComparison.Ordinal))
        {
            problem = $"unknown version (this Dovetail reads \"{VersionPrefix}\" keys)";
            return null;
        }

        var path = key.Substring(VersionPrefix.Length).Split(':');
        if (path.Any(string.IsNullOrEmpty))
        {
            problem = "the section path is empty or has an empty segment";
            return null;
        }

        if (value is null)
        {
            problem = "the schema is missing";
            return null;
        }

        JsonNode? schema;
        try
        {
            schema = JsonNode.Parse(value, documentOptions: new() { MaxDepth = 512, });
            problem = schema switch
            {
                JsonObject node => FindProblem(node),
                JsonValue node when node.GetValueKind() is JsonValueKind.True or JsonValueKind.False => null,
                _ => "the schema is not a JSON object",
            };
        }
        catch (Exception exception) when (exception is JsonException or ArgumentException or InvalidOperationException)
        {
            // ArgumentException/InvalidOperationException: JsonObject rejects duplicate property names when it materializes them.
            problem = $"the schema is not valid JSON ({exception.Message})";
            return null;
        }

        if (problem is not null || schema is null)
        {
            problem ??= "the schema is not a JSON object";
            return null;
        }

        return new(path, schema, schema.ToJsonString(), source);
    }

    /// <summary>
    ///     Validates the definitions and that every local <c>$ref</c> points at one of them: the merge moves them into another
    ///     document, so nothing else can be resolved there. Walking the whole schema also makes <see cref="JsonObject" />
    ///     materialize (and so validate) every property.
    /// </summary>
    private static string? FindProblem(JsonObject schema)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        if (schema.TryGetPropertyValue("definitions", out var definitions))
        {
            if (definitions is not JsonObject definitionObject) return "its definitions are not a JSON object";
            foreach (var definition in definitionObject)
            {
                if (!DefinitionName.IsMatch(definition.Key)) return $"the definition name \"{definition.Key}\" isn't of the form {DefinitionName}";
                if (definition.Value is not JsonObject && definition.Value?.GetValueKind() is not ( JsonValueKind.True or JsonValueKind.False ))
                {
                    return $"the definition \"{definition.Key}\" is not a schema";
                }

                names.Add(definition.Key);
            }
        }

        return FindReferenceProblem(schema, names);
    }

    private static string? FindReferenceProblem(JsonNode? node, HashSet<string> definitions)
    {
        switch (node)
        {
            case JsonObject value:
                foreach (var property in value)
                {
                    if (property is { Key: "$ref", Value: JsonValue reference, }
                     && reference.TryGetValue<string>(out var target)
                     && target.StartsWith("#", StringComparison.Ordinal)
                     && !( target.StartsWith(DefinitionReferencePrefix, StringComparison.Ordinal)
                         && definitions.Contains(target.Substring(DefinitionReferencePrefix.Length)) ))
                    {
                        return $"the $ref \"{target}\" doesn't point at one of its definitions";
                    }

                    if (FindReferenceProblem(property.Value, definitions) is { } problem) return problem;
                }

                return null;
            case JsonArray array:
                return array.Select(z => FindReferenceProblem(z, definitions)).FirstOrDefault(z => z is not null);
            default:
                return null;
        }
    }
}
