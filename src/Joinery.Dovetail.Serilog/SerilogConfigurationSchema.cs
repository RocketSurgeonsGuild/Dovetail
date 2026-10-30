using System.Reflection;

// The JSON Schema of the "Serilog" configuration section, as Joinery.Dovetail.Serilog reads it (SerilogMinimumLevelConfiguration): the
// minimum level, as a level name or as an object with Default and Override. It is emitted as the same assembly metadata fragment
// Dovetail's generator writes for [DovetailConfiguration] types: Joinery.Dovetail.Build.Tasks reads it from this assembly's metadata,
// writes it into the package's ConfigurationSchema.json and merges it into the appsettings.schema.json of every application that
// references this library. It is a hand-written attribute rather than a generated one because the section is read by hand, with no
// options type behind it. Nothing but the attribute exists at runtime, which keeps it trimming- and AOT-neutral.
//
// Sinks, enrichers, filters and the rest of the format of Serilog.Settings.Configuration are not read (that package finds them by
// reflection), so they are not described either: the merge closes the section with the one key it lists, and flags the others.
//
// The level rules are those of the "Serilog" definition in SchemaStore's appsettings.json schema (Apache-2.0), which Dovetail embeds in
// every host schema, narrowed to what is read: the six level names (SchemaStore also lists Off, which is not a Serilog level) and no
// level switches (SchemaStore's ControlledBy and $name references). Dovetail adds titles and descriptions, and a case-insensitive
// patternProperties twin for each key (which Dovetail's writer emits for every fragment). SerilogSchemaTests runs sample
// configurations through both and asserts where they agree and where they differ.
//
// The schema is draft-07 and permissive, as every fragment is: the merge closes the objects that list "properties" (the section
// itself and MinimumLevel's object form) with case-insensitive propertyNames.
[assembly: AssemblyMetadata(Joinery.Dovetail.Serilog.SerilogConfigurationSchema.Key, Joinery.Dovetail.Serilog.SerilogConfigurationSchema.Json)]

namespace Joinery.Dovetail.Serilog;

internal static class SerilogConfigurationSchema
{
    /// <summary>The metadata key: the section path after Joinery.Dovetail.Build.Tasks' version prefix.</summary>
    internal const string Key = "Dovetail.ConfigurationSchema.v1:Serilog";

    /// <summary>The fragment: the section's schema with its object types under <c>definitions</c>.</summary>
    internal const string Json = Head + Definitions + Tail;

    private const string Head = """
        {"type":"object","title":"Serilog","description":"Serilog's configuration. Joinery.Dovetail.Serilog reads the minimum level and its overrides from it; sinks and enrichers are configured by Serilog joints in code.","allOf":[{"$ref":"#/definitions/Settings"}],"definitions":{
        """;

    private const string Tail = "}}";

    private const string LevelDefinition = """
        "Level":{"type":"string","title":"Log level","description":"A Serilog log level threshold: one of Serilog's six levels, matched by name.","enum":["Verbose","Debug","Information","Warning","Error","Fatal"]}
        """;

    private const string MinimumLevelObjectDefinition = """
        "MinimumLevelObject":{"description":"The detailed form of MinimumLevel.","properties":{"Default":{"description":"The minimum level for sources without an override.","allOf":[{"$ref":"#/definitions/Level"}]},"Override":{"type":"object","title":"Minimum level overrides","description":"Minimum levels per logging source: the key is a source context (a namespace or type name prefix, such as Microsoft.AspNetCore), the value a level.","additionalProperties":{"$ref":"#/definitions/Level"}}},"patternProperties":{"^(?:[Dd][Ee][Ff][Aa][Uu][Ll][Tt])$":{"description":"The minimum level for sources without an override.","allOf":[{"$ref":"#/definitions/Level"}]},"^(?:[Oo][Vv][Ee][Rr][Rr][Ii][Dd][Ee])$":{"type":"object","title":"Minimum level overrides","description":"Minimum levels per logging source: the key is a source context (a namespace or type name prefix, such as Microsoft.AspNetCore), the value a level.","additionalProperties":{"$ref":"#/definitions/Level"}}}}
        """;

    private const string SettingsDefinition = """
        "Settings":{"properties":{"MinimumLevel":{"title":"Minimum level","description":"The minimum level of events to emit: a level name, or an object with Default and Override.","oneOf":[{"$ref":"#/definitions/Level"},{"type":"object","allOf":[{"$ref":"#/definitions/MinimumLevelObject"}]}]}},"patternProperties":{"^(?:[Mm][Ii][Nn][Ii][Mm][Uu][Mm][Ll][Ee][Vv][Ee][Ll])$":{"title":"Minimum level","description":"The minimum level of events to emit: a level name, or an object with Default and Override.","oneOf":[{"$ref":"#/definitions/Level"},{"type":"object","allOf":[{"$ref":"#/definitions/MinimumLevelObject"}]}]}}}
        """;

    private const string Definitions =
        LevelDefinition + "," +
        MinimumLevelObjectDefinition + "," +
        SettingsDefinition;
}
