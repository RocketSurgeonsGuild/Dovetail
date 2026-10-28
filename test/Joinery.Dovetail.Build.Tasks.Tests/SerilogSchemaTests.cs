using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Joinery.Dovetail.Serilog;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The schema Joinery.Dovetail.Serilog ships for the <c>Serilog</c> configuration section (Serilog.Settings.Configuration's format):
///     read from the real assembly's metadata like the build does, merged into an application schema, and checked against
///     SchemaStore's <c>Serilog</c> definition, which every host schema embeds and whose rules the fragment follows. MinimumLevel,
///     Enrich, Filter and Properties are modelled; Using, WriteTo, AuditTo, Destructure, LevelSwitches and FilterSwitches are only
///     permitted, with any value.
/// </summary>
public class SerilogSchemaTests
{
    private static readonly string[] NotModelledSections = ["Using", "WriteTo", "AuditTo", "Destructure", "LevelSwitches", "FilterSwitches"];

    private static readonly string[] Levels = ["Verbose", "Debug", "Information", "Warning", "Error", "Fatal"];

    private static readonly Regex NamedGroup = new(@"\(\?<[A-Za-z]+>", RegexOptions.CultureInvariant);

    /// <summary>The fragments in the Joinery.Dovetail.Serilog assembly, read as the build task reads them.</summary>
    private static IReadOnlyList<ConfigurationSchemaFragment> ReadFragments() =>
        ConfigurationSchemaFragments.Read([typeof(SerilogConfigurationJoint).Assembly.Location], _ => { });

    private static JsonObject ApplicationSchema(bool strict = true, bool includeSchemaStore = false) =>
        SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict, includeSchemaStore))
        );

    /// <summary>Our fragment alone, through the application schema without SchemaStore.</summary>
    private static SchemaInstanceValidator Validator(bool strict = true) => new(ApplicationSchema(strict));

    /// <summary>SchemaStore's appsettings schema alone. Its patterns use .NET-style named groups, which ECMA-262 lacks.</summary>
    private static SchemaInstanceValidator SchemaStoreValidator()
    {
        var document = new JsonObject
        {
            ["definitions"] = new JsonObject { [SchemaStoreAppSettings.DefinitionName] = SchemaStoreAppSettings.Load(), },
            ["$ref"] = SchemaStoreAppSettings.Reference,
        };
        return new(JsonNode.Parse(NamedGroup.Replace(document.ToJsonString(), "(?:"))!);
    }

    /// <summary>The host schema as built by default: SchemaStore's and ours must both pass.</summary>
    private static SchemaInstanceValidator HostValidator() =>
        new(
            JsonNode.Parse(
                NamedGroup.Replace(
                    ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict: true, includeSchemaStore: true)),
                    "(?:"
                )
            )!
        );

    private static JsonObject SchemaStoreSerilog() => SchemaStoreAppSettings.Load()["definitions"]!["Serilog"]!["definitions"]!.AsObject();

    private static string Config(string serilogSection) => "{\"Serilog\": {" + serilogSection + "}}";

    [Test]
    public void TheAssembly_DeclaresExactlyTheSerilogFragment()
    {
        var fragments = ReadFragments();

        fragments.Count.ShouldBe(1);
        fragments[0].Path.ShouldBe(["Serilog"]);
        fragments[0].Source.ShouldBe("Joinery.Dovetail.Serilog");
    }

    [Test]
    public async Task TheLibrarySegment_IsWellFormed_AndPermissive()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateLibrarySchema(ReadFragments())!);

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document["properties"]!.AsObject().Select(z => z.Key).ShouldBe(["Serilog"]);
        SchemaAssert.Objects(document).ShouldAllBe(z => !z.ContainsKey("propertyNames"));
        await Verify(json, "json");
    }

    [Test]
    public async Task TheApplicationSchema_ClosesTheSectionAndItsObjects_WithCaseInsensitiveKeys()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict: true, includeSchemaStore: false));

        var document = SchemaAssert.ShouldBeWellFormed(json);
        var closed = SchemaAssert.DovetailDefinitions(document).Where(z => z.Value!["propertyNames"] is not null).Select(z => z.Key).ToArray();

        // The section's own keys: everything Serilog.Settings.Configuration reads, modelled or not, and $schema like SchemaStore's.
        var section = SchemaAssert.DovetailDefinitions(document).Single(z => z.Key == "Serilog.Settings").Value!.AsObject();
        section["properties"]!.AsObject().Select(z => z.Key).ShouldBe(["$schema", "Using", "LevelSwitches", "FilterSwitches", "MinimumLevel", "Properties", "Enrich", "Filter", "WriteTo", "AuditTo", "Destructure"]);
        section["propertyNames"].ShouldNotBeNull();
        closed.ShouldContain("Serilog.MinimumLevelObject");
        closed.ShouldContain("Serilog.EnricherCall");
        closed.ShouldContain("Serilog.FilterCall");
        await Verify(json, "json");
    }

    [Test]
    public void TheEveryKeyHasACaseInsensitiveTwin_BuiltTheWayTheWriterBuildsIt()
    {
        var definitions = ReadFragments().Single().Schema["definitions"]!.AsObject();

        foreach (var definition in definitions.Where(z => z.Value!["properties"] is JsonObject))
        {
            var keys = definition.Value!["properties"]!.AsObject().Select(z => z.Key).ToArray();
            definition.Value["patternProperties"]!.AsObject().Select(z => z.Key).ShouldBe(keys.Select(CaseInsensitivePattern.For), $"{definition.Key}'s patternProperties");
        }
    }

    [Test]
    public void TheSharedDefinitions_AreSchemaStoresDefinitions()
    {
        var ours = ReadFragments().Single().Schema["definitions"]!.AsObject();
        var theirs = SchemaStoreSerilog();

        // Level names, the method-name pattern, the Name and Args rules and the list limits, as SchemaStore states them.
        ours["Level"]!["enum"]!.ToJsonString().ShouldBe(theirs["SerilogLogEventLevel"]!["enum"]!.ToJsonString());
        ours["Level"]!["type"]!.ToJsonString().ShouldBe(theirs["SerilogLogEventLevel"]!["type"]!.ToJsonString());
        Unnamed(theirs["CSharpMethodName"]!["pattern"]!.GetValue<string>()).ShouldBe(ours["EnricherName"]!["anyOf"]![1]!["pattern"]!.GetValue<string>());
        Unnamed(theirs["CSharpMethodName"]!["pattern"]!.GetValue<string>()).ShouldBe(ours["FilterName"]!["anyOf"]![1]!["pattern"]!.GetValue<string>());
        theirs["CSharpMethodName"]!["minLength"]!.GetValue<int>().ShouldBe(ours["EnricherName"]!["minLength"]!.GetValue<int>());
        Unnamed(theirs["ComplexMethodCallReference"]!["properties"]!["Args"]!["patternProperties"]!.AsObject().Single().Key)
           .ShouldBe(ours["EnricherCall"]!["properties"]!["Args"]!["patternProperties"]!.AsObject().Single().Key);
        theirs["ComplexMethodCallReference"]!["required"]!.ToJsonString().ShouldBe(ours["EnricherCall"]!["required"]!.ToJsonString());
        var list = theirs["MethodCallReference"]!["anyOf"]![0]!;
        ours["EnricherList"]!["anyOf"]![0]!["minItems"]!.ToJsonString().ShouldBe(list["minItems"]!.ToJsonString());
        ours["EnricherList"]!["anyOf"]![0]!["uniqueItems"]!.ToJsonString().ShouldBe(list["uniqueItems"]!.ToJsonString());
    }

    /// <summary>A SchemaStore pattern of the form <c>^(?&lt;name&gt;…)$</c> without the group.</summary>
    private static string Unnamed(string pattern) => Regex.Replace(pattern, @"^\^\(\?<[A-Za-z]+>(.*)\)\$$", "^$1$", RegexOptions.CultureInvariant);

    [Test]
    public void TheSectionIsPartOfTheHostSchema_WithOrWithoutSchemaStore()
    {
        foreach (var includeSchemaStore in new[] { false, true, })
        {
            var document = ApplicationSchema(includeSchemaStore: includeSchemaStore);

            SchemaAssert.DovetailPart(document)["properties"]!.AsObject().Select(z => z.Key).ShouldContain("Serilog");
            SchemaAssert.DovetailDefinitions(document).Single(z => z.Key == "Serilog.Settings").Value!["properties"]!["MinimumLevel"]!["oneOf"].ShouldNotBeNull();
        }
    }

    /// <summary>
    ///     Configurations of the modelled keys and whether they are valid. SchemaStore's schema, ours and the host schema that
    ///     contains both must all give that verdict.
    /// </summary>
    public static IEnumerable<(string Name, string Section, bool Valid)> AgreementCases()
    {
        // The README's example.
        yield return (
            "README",
            """
            "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
            "MinimumLevel": "Debug",
            "WriteTo": [ { "Name": "Console" }, { "Name": "File", "Args": { "path": "Logs/log.txt" } } ],
            "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
            "Destructure": [ { "Name": "ToMaximumDepth", "Args": { "maximumDestructuringDepth": 4 } } ],
            "Properties": { "Application": "Sample" }
            """,
            true
        );
        yield return ("an empty section", "", true);
        yield return ("$schema", """ "$schema": "https://example.com/serilog.json" """, true);

        foreach (var level in Levels.Append("Off"))
        {
            yield return ($"MinimumLevel {level}", $$""" "MinimumLevel": "{{level}}" """, true);
            yield return ($"MinimumLevel object with {level}", $$""" "MinimumLevel": { "Default": "{{level}}", "Override": { "System": "{{level}}" } } """, true);
        }

        yield return ("MinimumLevel with only a default", """ "MinimumLevel": { "Default": "Information" } """, true);
        yield return (
            "MinimumLevel with overrides",
            """ "MinimumLevel": { "Default": "Information", "Override": { "Microsoft": "Warning", "Microsoft.Hosting.Lifetime": "Information", "System": "Warning", "MyApp.Something.Tricky": "Verbose" } } """,
            true
        );
        yield return ("MinimumLevel controlled by a switch", """ "MinimumLevel": { "ControlledBy": "$controlSwitch", "Override": { "Microsoft": "Warning" } } """, true);
        yield return ("MinimumLevel with an override switch", """ "MinimumLevel": { "Default": "Debug", "Override": { "Microsoft": "$microsoftSwitch" } } """, true);
        yield return ("MinimumLevel with empty overrides", """ "MinimumLevel": { "Default": "Warning", "Override": {} } """, true);
        yield return ("MinimumLevel with an empty object", """ "MinimumLevel": {} """, true);

        // SchemaStore's switch-name pattern ($ optional, then letters and digits) also matches plain words.
        yield return ("a default that is a plain word", """ "MinimumLevel": { "Default": "Chatty" } """, true);
        yield return ("ControlledBy without a dollar sign", """ "MinimumLevel": { "ControlledBy": "controlSwitch" } """, true);
        yield return ("an override that is a plain word", """ "MinimumLevel": { "Override": { "Microsoft": "Quiet" } } """, true);

        yield return ("MinimumLevel of a number", """ "MinimumLevel": 2 """, false);
        yield return ("MinimumLevel of a boolean", """ "MinimumLevel": true """, false);
        yield return ("MinimumLevel of an array", """ "MinimumLevel": [ "Debug" ] """, false);
        yield return ("an unknown MinimumLevel", """ "MinimumLevel": "Loud" """, false);
        yield return ("a MinimumLevel in other casing", """ "MinimumLevel": "debug" """, false);
        yield return ("an empty MinimumLevel", """ "MinimumLevel": "" """, false);
        yield return ("a numeric default level", """ "MinimumLevel": { "Default": 2 } """, false);
        yield return ("a default with a space", """ "MinimumLevel": { "Default": "Not a level" } """, false);
        yield return ("a default that starts with a digit", """ "MinimumLevel": { "Default": "1Debug" } """, false);
        yield return ("a misspelled MinimumLevel key", """ "MinimumLevel": { "Defualt": "Information" } """, false);
        yield return ("an unknown MinimumLevel key", """ "MinimumLevel": { "Default": "Information", "Minimum": "Debug" } """, false);
        yield return ("an override that is a number", """ "MinimumLevel": { "Override": { "Microsoft": 3 } } """, false);
        yield return ("an override map that is an array", """ "MinimumLevel": { "Override": [ "Microsoft" ] } """, false);
        yield return ("ControlledBy that is a number", """ "MinimumLevel": { "ControlledBy": 1 } """, false);
        yield return ("ControlledBy with a double dollar sign", """ "MinimumLevel": { "ControlledBy": "$$controlSwitch" } """, false);
        yield return ("a misspelled section key", """ "MinimumLevl": "Debug" """, false);
        yield return ("an unknown section key", """ "Minimum": "Debug" """, false);

        yield return (
            "Enrich",
            """
            "Enrich": [
              "FromLogContext",
              "WithThreadId",
              "WithSomethingElse",
              "_with_underscores_1",
              { "Name": "WithProperty", "Args": { "name": "Version", "value": "1.2.3" } },
              { "Name": "AtLevel", "Args": { "enrichFromLevel": "Error", "configureEnricher": [ "WithThreadName" ] } },
              { "Name": "WithCustomEnricher" }
            ]
            """,
            true
        );
        yield return ("Enrich by name", """ "Enrich": { "Context": "FromLogContext", "Machine": { "Name": "WithMachineName" } } """, true);
        yield return ("Enrich as a single name", """ "Enrich": "FromLogContext" """, true);
        yield return ("Enrich with empty Args", """ "Enrich": [ { "Name": "WithMachineName", "Args": {} } ] """, true);
        yield return (
            "Filter",
            """ "Filter": [ { "Name": "ByExcluding", "Args": { "expression": "RequestPath like '/health%'" } }, { "Name": "ControlledBy", "Args": { "switch": "$filterSwitch" } } ] """,
            true
        );
        yield return ("Properties", """ "Properties": { "Application": "Sample", "Instance": "3" } """, true);

        yield return ("an enricher that is a number", """ "Enrich": [ 1 ] """, false);
        yield return ("an empty enricher name", """ "Enrich": [ "" ] """, false);
        yield return ("an enricher that is null", """ "Enrich": [ null ] """, false);
        yield return ("an enricher name with a dot", """ "Enrich": [ "Some.Namespace.WithThing" ] """, false);
        yield return ("an enricher name with a space", """ "Enrich": [ "With Thing" ] """, false);
        yield return ("an enricher name that starts with a digit", """ "Enrich": [ "1WithThing" ] """, false);
        yield return ("a numeric enricher name", """ "Enrich": [ { "Name": 4 } ] """, false);
        yield return ("an enricher object name with a dot", """ "Enrich": [ { "Name": "Some.Thing" } ] """, false);
        yield return ("an enricher without a name", """ "Enrich": [ { "Args": {} } ] """, false);
        yield return ("an enricher with a misspelled key", """ "Enrich": [ { "Name": "FromLogContext", "Arg": {} } ] """, false);
        yield return ("enricher arguments that are a string", """ "Enrich": [ { "Name": "WithProperty", "Args": "name" } ] """, false);
        yield return ("enricher arguments named with a dot", """ "Enrich": [ { "Name": "WithProperty", "Args": { "a.b": 1 } } ] """, false);
        yield return ("an empty Enrich list", """ "Enrich": [] """, false);
        yield return ("a repeated enricher", """ "Enrich": [ "FromLogContext", "FromLogContext" ] """, false);
        yield return ("Enrich as a number", """ "Enrich": 3 """, false);
        yield return ("an empty Enrich name", """ "Enrich": "" """, false);
        yield return ("a Filter object with a misspelled key", """ "Filter": [ { "Name": "ByExcluding", "Args": {}, "Expression": "x" } ] """, false);
        yield return ("a Filter that is a boolean", """ "Filter": [ true ] """, false);
        yield return ("a Filter item with the item's keys directly", """ "Filter": { "Name": "ByExcluding", "Args": { "expression": "x" } } """, false);
        yield return ("a property that is an object", """ "Properties": { "Application": { "Name": "Sample" } } """, false);
        yield return ("a property that is a number", """ "Properties": { "Instance": 3 } """, false);
        yield return ("a property that is a boolean", """ "Properties": { "Debug": true } """, false);
        yield return ("properties that are an array", """ "Properties": [ "Application" ] """, false);
    }

    [Test]
    [MethodDataSource(nameof(AgreementCases))]
    public void SchemaStoresRulesAndOurs_GiveEachConfigurationTheSameVerdict(string name, string section, bool valid)
    {
        var instance = Config(section);

        var schemaStore = SchemaStoreValidator().Validate(instance);
        var ours = Validator().Validate(instance);
        var host = HostValidator().Validate(instance);

        ( schemaStore.Count == 0 ).ShouldBe(valid, $"{name}, SchemaStore's schema: {string.Join("; ", schemaStore)}");
        ( ours.Count == 0 ).ShouldBe(valid, $"{name}, Dovetail's fragment: {string.Join("; ", ours)}");
        ( host.Count == 0 ).ShouldBe(valid, $"{name}, the host schema: {string.Join("; ", host)}");
    }

    [Test]
    public void NamesAndSwitchReferences_AreMatchedLikeSchemaStoreMatchesThem()
    {
        string[] probes = ["a", "A", "_", "_a", "a1", "a_1", "1a", "a-b", "a.b", "a b", "", "$a", "$", "$$a", "$1a", "a$", "é", "aé"];
        var schemaStore = SchemaStoreValidator();
        var ours = Validator();
        var host = HostValidator();

        foreach (var probe in probes)
        {
            var escaped = JsonValue.Create(probe)!.ToJsonString();
            foreach (var section in new[]
                     {
                         $$""" "Enrich": [ {{escaped}} ] """,
                         $$""" "Filter": [ { "Name": {{escaped}} } ] """,
                         $$""" "MinimumLevel": { "ControlledBy": {{escaped}} } """,
                         $$""" "MinimumLevel": { "Default": {{escaped}} } """,
                         $$""" "MinimumLevel": { "Override": { "Microsoft": {{escaped}} } } """,
                     })
            {
                var instance = Config(section);
                var expected = schemaStore.IsValid(instance);

                ours.IsValid(instance).ShouldBe(expected, $"{section}: Dovetail's fragment against SchemaStore's");
                host.IsValid(instance).ShouldBe(expected, $"{section}: the host schema against SchemaStore's");
            }
        }
    }

    [Test]
    public void TheKnownNames_AreAllAcceptedBySchemaStore()
    {
        var definitions = ReadFragments().Single().Schema["definitions"]!.AsObject();
        var schemaStore = SchemaStoreValidator();

        foreach (var kind in new[] { "Enricher", "Filter", })
        {
            foreach (var name in definitions[kind + "Name"]!["anyOf"]![0]!["enum"]!.AsArray().Select(z => z!.GetValue<string>()))
            {
                schemaStore.Validate(Config($$""" "{{( kind == "Enricher" ? "Enrich" : "Filter" )}}": [ "{{name}}", { "Name": "{{name}}" } ] """)).ShouldBeEmpty(name);
            }
        }
    }

    [Test]
    public void KeyCasing_IsTheOneDifferenceFromSchemaStore_AndTheHostSchemaStillFollowsIt()
    {
        // Configuration keys ignore case, so the fragment accepts any casing of its keys (a twin for each); SchemaStore's schema
        // lists them case-sensitively with additionalProperties: false. Both apply in the host schema, so SchemaStore decides.
        var instance = Config(""" "minimumlevel": { "default": "Debug", "OVERRIDE": { "System": "Error" } }, "enrich": [ { "Name": "FromLogContext", "args": {} } ] """);
        var wrongValue = Config(""" "minimumlevel": 2 """);

        Validator().IsValid(instance).ShouldBeTrue();
        SchemaStoreValidator().IsValid(instance).ShouldBeFalse();
        HostValidator().IsValid(instance).ShouldBeFalse();
        Validator().IsValid(wrongValue).ShouldBeFalse();
    }

    [Test]
    public void FlatKeys_AreAcceptedBySchemaStore_ButClosedOutByDovetailsStrictness()
    {
        // "WriteTo:Async" and "Enrich:0" are documented Serilog syntax and SchemaStore lists patterns for them. The strict
        // merge closes the section with the names it knows, so they are flagged unless strictness is off.
        var instance = Config(""" "WriteTo:Async": { "Name": "Async" }, "Enrich:0": "FromLogContext", "Enrich:Machine": { "Name": "WithMachineName" } """);

        SchemaStoreValidator().IsValid(instance).ShouldBeTrue();
        Validator().IsValid(instance).ShouldBeFalse();
        HostValidator().IsValid(instance).ShouldBeFalse();
        Validator(strict: false).IsValid(instance).ShouldBeTrue();
    }

    [Test]
    public void TheSectionsThatAreNotModelled_StillValidate_WhateverTheirValue()
    {
        var validator = Validator();
        string[] values =
        [
            "[ \"Console\", { \"Name\": \"File\", \"Args\": { \"path\": \"Logs/log.txt\", \"rollingInterval\": \"Day\" } } ]",
            "{ \"Console\": \"Console\", \"File\": { \"Name\": \"File\" } }",
            "\"Console\"",
            "{ \"controlSwitch\": \"Verbose\", \"$other\": \"Debug\" }",
            "{ \"filterSwitch\": \"Application = 'Sample'\" }",
            "[ { \"Name\": \"With\", \"Args\": { \"policy\": \"Sample.CustomPolicy, Sample\" } } ]",
            "42",
            "null",
            "true",
        ];

        foreach (var section in NotModelledSections)
        {
            foreach (var value in values)
            {
                validator.Validate(Config($"\"{section}\": {value}")).ShouldBeEmpty($"{section}: {value}");
                // Whatever the casing of the key.
                validator.Validate(Config($"\"{section.ToUpperInvariant()}\": {value}")).ShouldBeEmpty($"{section.ToUpperInvariant()}: {value}");
            }
        }
    }

    [Test]
    public void TheNotModelledSections_AreLeftToSchemaStore_InTheHostSchema()
    {
        // Ours accepts anything there, so SchemaStore's rules are the ones that apply when it is part of the host schema.
        var host = HostValidator();

        host.IsValid(Config(""" "WriteTo": [ { "Name": "Console" } ], "Using": [ "Serilog.Sinks.Console" ], "AuditTo": [ "Console" ] """)).ShouldBeTrue();
        host.IsValid(Config(""" "WriteTo": 42 """)).ShouldBeFalse();
        Validator().IsValid(Config(""" "WriteTo": 42 """)).ShouldBeTrue();
    }

    [Test]
    public void AConfigurationUsingTheWholeFormat_Validates_AndTheSectionIsStillClosed()
    {
        var everything = """
                         "Using": [ "Serilog.Sinks.Console" ],
                         "LevelSwitches": { "controlSwitch": "Verbose" },
                         "FilterSwitches": { "filterSwitch": "Application = 'Sample'" },
                         "MinimumLevel": { "ControlledBy": "$controlSwitch", "Override": { "Microsoft": "Warning" } },
                         "WriteTo": [ { "Name": "Console" } ],
                         "AuditTo": [ { "Name": "File", "Args": { "path": "audit.txt" } } ],
                         "Enrich": [ "FromLogContext" ],
                         "Destructure": [ { "Name": "ToMaximumDepth", "Args": { "maximumDestructuringDepth": 4 } } ],
                         "Filter": [ { "Name": "ByExcluding", "Args": { "expression": "x = 1" } } ],
                         "Properties": { "Application": "Sample" }
                         """;

        foreach (var validator in new[] { Validator(), HostValidator(), SchemaStoreValidator(), })
        {
            validator.Validate(Config(everything)).ShouldBeEmpty();
            validator.IsValid(Config(everything + ", \"WriteToo\": []")).ShouldBeFalse();
        }
    }

    [Test]
    public void TheSchemaIsOpenWhenStrictnessIsOff_ExceptForTheValuesItModels()
    {
        // Without strictness no key is closed (typos pass), but a wrong value is still wrong.
        var validator = Validator(strict: false);

        validator.IsValid(Config(""" "Minimum": "Debug" """)).ShouldBeTrue();
        validator.IsValid(Config(""" "MinimumLevel": { "Defualt": "Debug" } """)).ShouldBeTrue();
        validator.IsValid(Config(""" "MinimumLevel": 2 """)).ShouldBeFalse();
        validator.IsValid(Config(""" "MinimumLevel": { "Default": "Not a level" } """)).ShouldBeFalse();
    }
}
