using System.Reflection;

// The JSON Schema of the "Serilog" configuration section, as Serilog.Settings.Configuration (ReadFrom.Configuration) reads it,
// emitted as the same assembly metadata fragment Dovetail's generator writes for [DovetailConfiguration] types:
// Joinery.Dovetail.Build.Tasks reads it from this assembly's metadata, writes it into the package's ConfigurationSchema.json and merges
// it into the appsettings.schema.json of every application that references this library. It is a hand-written attribute rather
// than a generated one because the section is dynamic (sinks, enrichers and their arguments come from other packages), so no
// options type can describe it. Nothing but the attribute exists at runtime, which keeps it trimming- and AOT-neutral.
//
// The rules are those of the "Serilog" definition in SchemaStore's appsettings.json schema (Apache-2.0), which Dovetail embeds in
// every host schema: the same level names, switch and method-name patterns, value types and list shapes, so that a configuration
// is valid under this fragment exactly when it is under SchemaStore's. Only additions that can't change a verdict are Dovetail's:
// titles and descriptions, the well-known enricher and filter names offered as completions beside the general method-name
// pattern, and a case-insensitive patternProperties twin for each key (which Dovetail's writer emits for every fragment).
// SerilogSchemaTests runs sample configurations through both and asserts that they agree.
//
// The schema is draft-07 and permissive, as every fragment is: the merge closes the objects that list "properties" (the section
// itself, MinimumLevel's object form, and the enricher and filter items) with case-insensitive propertyNames. Using, WriteTo,
// AuditTo, Destructure, LevelSwitches and FilterSwitches are not modelled here: they are listed so they stay accepted, and take
// any value (SchemaStore's schema still checks them where it is part of the host schema).
[assembly: AssemblyMetadata(Joinery.Dovetail.Serilog.SerilogConfigurationSchema.Key, Joinery.Dovetail.Serilog.SerilogConfigurationSchema.Json)]

namespace Joinery.Dovetail.Serilog;

internal static class SerilogConfigurationSchema
{
    /// <summary>The metadata key: the section path after Joinery.Dovetail.Build.Tasks' version prefix.</summary>
    internal const string Key = "Dovetail.ConfigurationSchema.v1:Serilog";

    /// <summary>The fragment: the section's schema with its object types under <c>definitions</c>.</summary>
    internal const string Json = Head + Definitions + Tail;

    private const string Head = """
        {"type":"object","title":"Serilog","description":"Serilog's configuration, as read by Serilog.Settings.Configuration (ReadFrom.Configuration).","allOf":[{"$ref":"#/definitions/Settings"}],"definitions":{
        """;

    private const string Tail = "}}";

    private const string LevelDefinition = """
        "Level":{"type":"string","title":"Log level","description":"A Serilog log level threshold. Like SchemaStore's schema, this lists Off beside Serilog's six levels, and matches names exactly.","enum":["Verbose","Debug","Information","Warning","Error","Fatal","Off"]}
        """;

    private const string LevelSwitchDefinition = """
        "LevelSwitch":{"type":"string","title":"Level switch name","description":"A reference to a level switch declared under LevelSwitches, with or without the leading dollar sign.","pattern":"^\\$?[A-Za-z]+[A-Za-z0-9]*$"}
        """;

    private const string LevelThresholdDefinition = """
        "LevelThreshold":{"type":"string","title":"Log level or switch","description":"A log level, or a reference to a level switch declared under LevelSwitches.","anyOf":[{"$ref":"#/definitions/Level"},{"$ref":"#/definitions/LevelSwitch"}]}
        """;

    private const string MinimumLevelObjectDefinition = """
        "MinimumLevelObject":{"description":"The detailed form of MinimumLevel.","properties":{"Default":{"description":"The minimum level for sources without an override.","allOf":[{"$ref":"#/definitions/LevelThreshold"}]},"ControlledBy":{"description":"A level switch ($name) that sets the default minimum level at runtime.","allOf":[{"$ref":"#/definitions/LevelSwitch"}]},"Override":{"type":"object","title":"Minimum level overrides","description":"Minimum levels per logging source: the key is a source context (a namespace or type name prefix, such as Microsoft.AspNetCore), the value a level or a level switch reference.","additionalProperties":{"$ref":"#/definitions/LevelThreshold"}}},"patternProperties":{"^(?:[Dd][Ee][Ff][Aa][Uu][Ll][Tt])$":{"description":"The minimum level for sources without an override.","allOf":[{"$ref":"#/definitions/LevelThreshold"}]},"^(?:[Cc][Oo][Nn][Tt][Rr][Oo][Ll][Ll][Ee][Dd][Bb][Yy])$":{"description":"A level switch ($name) that sets the default minimum level at runtime.","allOf":[{"$ref":"#/definitions/LevelSwitch"}]},"^(?:[Oo][Vv][Ee][Rr][Rr][Ii][Dd][Ee])$":{"type":"object","title":"Minimum level overrides","description":"Minimum levels per logging source: the key is a source context (a namespace or type name prefix, such as Microsoft.AspNetCore), the value a level or a level switch reference.","additionalProperties":{"$ref":"#/definitions/LevelThreshold"}}}}
        """;

    private const string EnricherNameDefinition = """
        "EnricherName":{"type":"string","minLength":1,"description":"The name of the enricher method (an extension method on Serilog's configuration), e.g. FromLogContext. Any C# method name is accepted, since libraries add their own.","anyOf":[{"enum":["FromLogContext","WithMachineName","WithEnvironmentName","WithEnvironmentUserName","WithThreadId","WithThreadName","WithProcessId","WithProcessName","WithProperty","WithExceptionDetails","WithSpan","WithCorrelationId","WithClientIp","WithClientAgent","WithDemystifiedStackTraces","WithAssemblyName","WithAssemblyVersion","WithMemoryUsage","AtLevel","When","With"]},{"pattern":"^[a-zA-Z_]\\w*$"}]}
        """;

    private const string EnricherCallDefinition = """
        "EnricherCall":{"description":"An enricher with arguments.","properties":{"Name":{"allOf":[{"$ref":"#/definitions/EnricherName"}]},"Args":{"type":"object","description":"The arguments of the method, by parameter name. Their shape depends on the method, so any values are accepted.","patternProperties":{"^[a-zA-Z_]\\w*$":{}},"additionalProperties":false}},"patternProperties":{"^(?:[Nn][Aa][Mm][Ee])$":{"allOf":[{"$ref":"#/definitions/EnricherName"}]},"^(?:[Aa][Rr][Gg][Ss])$":{"type":"object","description":"The arguments of the method, by parameter name. Their shape depends on the method, so any values are accepted.","patternProperties":{"^[a-zA-Z_]\\w*$":{}},"additionalProperties":false}},"required":["Name"]}
        """;

    private const string EnricherItemDefinition = """
        "EnricherItem":{"title":"Enricher","oneOf":[{"$ref":"#/definitions/EnricherName"},{"type":"object","allOf":[{"$ref":"#/definitions/EnricherCall"}]}]}
        """;

    private const string EnricherListDefinition = """
        "EnricherList":{"anyOf":[{"type":"array","minItems":1,"uniqueItems":true,"items":{"$ref":"#/definitions/EnricherItem"}},{"$ref":"#/definitions/EnricherName"},{"type":"object","description":"Items by name: the keys are free (so environment variables can override one item), the values the items.","additionalProperties":{"$ref":"#/definitions/EnricherItem"}}]}
        """;

    private const string FilterNameDefinition = """
        "FilterName":{"type":"string","minLength":1,"description":"The name of the filter method (an extension method on Serilog's configuration), e.g. ByExcluding. Any C# method name is accepted, since libraries add their own.","anyOf":[{"enum":["ByExcluding","ByIncludingOnly","ControlledBy","With"]},{"pattern":"^[a-zA-Z_]\\w*$"}]}
        """;

    private const string FilterCallDefinition = """
        "FilterCall":{"description":"A filter with arguments.","properties":{"Name":{"allOf":[{"$ref":"#/definitions/FilterName"}]},"Args":{"type":"object","description":"The arguments of the method, by parameter name. Their shape depends on the method, so any values are accepted.","patternProperties":{"^[a-zA-Z_]\\w*$":{}},"additionalProperties":false}},"patternProperties":{"^(?:[Nn][Aa][Mm][Ee])$":{"allOf":[{"$ref":"#/definitions/FilterName"}]},"^(?:[Aa][Rr][Gg][Ss])$":{"type":"object","description":"The arguments of the method, by parameter name. Their shape depends on the method, so any values are accepted.","patternProperties":{"^[a-zA-Z_]\\w*$":{}},"additionalProperties":false}},"required":["Name"]}
        """;

    private const string FilterItemDefinition = """
        "FilterItem":{"title":"Filter","oneOf":[{"$ref":"#/definitions/FilterName"},{"type":"object","allOf":[{"$ref":"#/definitions/FilterCall"}]}]}
        """;

    private const string FilterListDefinition = """
        "FilterList":{"anyOf":[{"type":"array","minItems":1,"uniqueItems":true,"items":{"$ref":"#/definitions/FilterItem"}},{"$ref":"#/definitions/FilterName"},{"type":"object","description":"Items by name: the keys are free (so environment variables can override one item), the values the items.","additionalProperties":{"$ref":"#/definitions/FilterItem"}}]}
        """;

    private const string SettingsDefinition = """
        "Settings":{"properties":{"$schema":{"type":"string","title":"Schema","description":"Pointer to the schema against which this document should be validated."},"Using":{"title":"Using","description":"The assemblies that hold the configuration methods, e.g. Serilog.Sinks.Console. Its shape is not modelled here: any value is accepted."},"LevelSwitches":{"title":"LevelSwitches","description":"Named level switches, referenced as $name. Its shape is not modelled here: any value is accepted."},"FilterSwitches":{"title":"FilterSwitches","description":"Named filter switches, referenced as $name. Its shape is not modelled here: any value is accepted."},"MinimumLevel":{"title":"Minimum level","description":"The minimum level of events to emit: a level name, or an object with Default, Override and ControlledBy.","oneOf":[{"$ref":"#/definitions/Level"},{"type":"object","allOf":[{"$ref":"#/definitions/MinimumLevelObject"}]}]},"Properties":{"type":"object","title":"Properties","description":"Static properties added to every log event.","additionalProperties":{"type":"string"}},"Enrich":{"title":"Enrich","description":"The enrichers that add properties to log events.","allOf":[{"$ref":"#/definitions/EnricherList"}]},"Filter":{"title":"Filter","description":"The filters that decide which log events are emitted.","allOf":[{"$ref":"#/definitions/FilterList"}]},"WriteTo":{"title":"WriteTo","description":"The sinks that log events are written to. Its shape is not modelled here: any value is accepted."},"AuditTo":{"title":"AuditTo","description":"The sinks that log events are audited to. Its shape is not modelled here: any value is accepted."},"Destructure":{"title":"Destructure","description":"The destructuring policies. Its shape is not modelled here: any value is accepted."}},"patternProperties":{"^(?:\\$[Ss][Cc][Hh][Ee][Mm][Aa])$":{"type":"string","title":"Schema","description":"Pointer to the schema against which this document should be validated."},"^(?:[Uu][Ss][Ii][Nn][Gg])$":{"title":"Using","description":"The assemblies that hold the configuration methods, e.g. Serilog.Sinks.Console. Its shape is not modelled here: any value is accepted."},"^(?:[Ll][Ee][Vv][Ee][Ll][Ss][Ww][Ii][Tt][Cc][Hh][Ee][Ss])$":{"title":"LevelSwitches","description":"Named level switches, referenced as $name. Its shape is not modelled here: any value is accepted."},"^(?:[Ff][Ii][Ll][Tt][Ee][Rr][Ss][Ww][Ii][Tt][Cc][Hh][Ee][Ss])$":{"title":"FilterSwitches","description":"Named filter switches, referenced as $name. Its shape is not modelled here: any value is accepted."},"^(?:[Mm][Ii][Nn][Ii][Mm][Uu][Mm][Ll][Ee][Vv][Ee][Ll])$":{"title":"Minimum level","description":"The minimum level of events to emit: a level name, or an object with Default, Override and ControlledBy.","oneOf":[{"$ref":"#/definitions/Level"},{"type":"object","allOf":[{"$ref":"#/definitions/MinimumLevelObject"}]}]},"^(?:[Pp][Rr][Oo][Pp][Ee][Rr][Tt][Ii][Ee][Ss])$":{"type":"object","title":"Properties","description":"Static properties added to every log event.","additionalProperties":{"type":"string"}},"^(?:[Ee][Nn][Rr][Ii][Cc][Hh])$":{"title":"Enrich","description":"The enrichers that add properties to log events.","allOf":[{"$ref":"#/definitions/EnricherList"}]},"^(?:[Ff][Ii][Ll][Tt][Ee][Rr])$":{"title":"Filter","description":"The filters that decide which log events are emitted.","allOf":[{"$ref":"#/definitions/FilterList"}]},"^(?:[Ww][Rr][Ii][Tt][Ee][Tt][Oo])$":{"title":"WriteTo","description":"The sinks that log events are written to. Its shape is not modelled here: any value is accepted."},"^(?:[Aa][Uu][Dd][Ii][Tt][Tt][Oo])$":{"title":"AuditTo","description":"The sinks that log events are audited to. Its shape is not modelled here: any value is accepted."},"^(?:[Dd][Ee][Ss][Tt][Rr][Uu][Cc][Tt][Uu][Rr][Ee])$":{"title":"Destructure","description":"The destructuring policies. Its shape is not modelled here: any value is accepted."}}}
        """;

    private const string Definitions =
        LevelDefinition + "," +
        LevelSwitchDefinition + "," +
        LevelThresholdDefinition + "," +
        MinimumLevelObjectDefinition + "," +
        EnricherNameDefinition + "," +
        EnricherCallDefinition + "," +
        EnricherItemDefinition + "," +
        EnricherListDefinition + "," +
        FilterNameDefinition + "," +
        FilterCallDefinition + "," +
        FilterItemDefinition + "," +
        FilterListDefinition + "," +
        SettingsDefinition;
}
