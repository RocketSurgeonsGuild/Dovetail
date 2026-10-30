using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Joinery.Dovetail.Serilog;
using Serilog.Events;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The schema Joinery.Dovetail.Serilog ships for the <c>Serilog</c> configuration section, which it reads for the minimum level
///     (<c>MinimumLevel</c>, as a level name or as an object with <c>Default</c> and <c>Override</c>) and nothing else: read from the real
///     assembly's metadata like the build does, merged into an application schema, and checked against SchemaStore's <c>Serilog</c>
///     definition, which every host schema embeds and whose level rules the fragment follows, narrowed to what is read. The tests say where
///     the two agree and where the fragment deliberately says no (Off, level switches, and the keys that are not read).
/// </summary>
public class SerilogSchemaTests
{
    private static readonly string[] Levels = ["Verbose", "Debug", "Information", "Warning", "Error", "Fatal"];

    /// <summary>The keys of Serilog.Settings.Configuration's format that Joinery.Dovetail.Serilog does not read, with a value each.</summary>
    private static readonly (string Key, string Value)[] KeysThatAreNotRead =
    [
        ("Using", """[ "Serilog.Sinks.Console" ]"""),
        ("LevelSwitches", """{ "controlSwitch": "Verbose" }"""),
        ("FilterSwitches", """{ "filterSwitch": "Application = 'Sample'" }"""),
        ("Properties", """{ "Application": "Sample" }"""),
        ("Enrich", """[ "FromLogContext", { "Name": "WithProperty", "Args": { "name": "Version", "value": "1.2.3" } } ]"""),
        ("Filter", """[ { "Name": "ByExcluding", "Args": { "expression": "RequestPath like '/health%'" } } ]"""),
        ("WriteTo", """[ { "Name": "Console" }, { "Name": "File", "Args": { "path": "Logs/log.txt" } } ]"""),
        ("AuditTo", """[ "Console" ]"""),
        ("Destructure", """[ { "Name": "ToMaximumDepth", "Args": { "maximumDestructuringDepth": 4 } } ]"""),
        ("$schema", "\"https://example.com/serilog.json\""),
    ];

    private static readonly Regex NamedGroup = new(@"\(\?<[A-Za-z]+>", RegexOptions.CultureInvariant);

    /// <summary>The fragments in the Joinery.Dovetail.Serilog assembly, read as the build task reads them.</summary>
    private static IReadOnlyList<ConfigurationSchemaFragment> ReadFragments() =>
        ConfigurationSchemaFragments.Read([typeof(SerilogJoint).Assembly.Location], _ => { });

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

        // The section's own keys: the one it reads.
        var section = SchemaAssert.DovetailDefinitions(document).Single(z => z.Key == "Serilog.Settings").Value!.AsObject();
        section["properties"]!.AsObject().Select(z => z.Key).ShouldBe(["MinimumLevel"]);
        section["propertyNames"].ShouldNotBeNull();
        closed.ShouldContain("Serilog.MinimumLevelObject");
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
    public void TheLevels_AreSerilogsLevels_AndSchemaStoresLevelsWithoutOff()
    {
        var ours = ReadFragments().Single().Schema["definitions"]!.AsObject();
        var theirs = SchemaStoreSerilog();
        var oursLevels = ours["Level"]!["enum"]!.AsArray().Select(z => z!.GetValue<string>()).ToArray();

        // The names of LogEventLevel, which is what the section is parsed to. SchemaStore also lists Off, which Serilog has no level for.
        oursLevels.ShouldBe(Enum.GetNames<LogEventLevel>());
        oursLevels.ShouldBe(theirs["SerilogLogEventLevel"]!["enum"]!.AsArray().Select(z => z!.GetValue<string>()).Where(z => z != "Off"));
        ours["Level"]!["type"]!.ToJsonString().ShouldBe(theirs["SerilogLogEventLevel"]!["type"]!.ToJsonString());
    }

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
    ///     Configurations of the keys that are read and whether they are valid. SchemaStore's schema, ours and the host schema that
    ///     contains both must all give that verdict.
    /// </summary>
    public static IEnumerable<(string Name, string Section, bool Valid)> AgreementCases()
    {
        yield return ("an empty section", "", true);

        foreach (var level in Levels)
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
        yield return ("MinimumLevel with only overrides", """ "MinimumLevel": { "Override": { "Microsoft": "Warning" } } """, true);
        yield return ("MinimumLevel with empty overrides", """ "MinimumLevel": { "Default": "Warning", "Override": {} } """, true);
        yield return ("MinimumLevel with an empty object", """ "MinimumLevel": {} """, true);

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
        yield return ("a misspelled section key", """ "MinimumLevl": "Debug" """, false);
        yield return ("an unknown section key", """ "Minimum": "Debug" """, false);
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

    /// <summary>
    ///     Configurations that SchemaStore's schema accepts (they are Serilog.Settings.Configuration's format) and that Joinery.Dovetail.Serilog
    ///     does not read, so the fragment says no, and so does the host schema that contains both.
    /// </summary>
    public static IEnumerable<(string Name, string Section)> NotReadCases()
    {
        yield return ("the level Off, which is not a Serilog level", """ "MinimumLevel": "Off" """);
        yield return ("a switch to control the minimum level", """ "MinimumLevel": { "ControlledBy": "$controlSwitch", "Override": { "Microsoft": "Warning" } } """);
        yield return ("an override that is a switch reference", """ "MinimumLevel": { "Default": "Debug", "Override": { "Microsoft": "$microsoftSwitch" } } """);
        yield return ("a default that is a plain word, which SchemaStore takes for a switch name", """ "MinimumLevel": { "Default": "Chatty" } """);
        yield return ("ControlledBy without a dollar sign", """ "MinimumLevel": { "ControlledBy": "controlSwitch" } """);
        yield return (
            "the README's example",
            """
            "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
            "MinimumLevel": "Debug",
            "WriteTo": [ { "Name": "Console" }, { "Name": "File", "Args": { "path": "Logs/log.txt" } } ],
            "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
            "Destructure": [ { "Name": "ToMaximumDepth", "Args": { "maximumDestructuringDepth": 4 } } ],
            "Properties": { "Application": "Sample" }
            """
        );

        foreach (var (key, value) in KeysThatAreNotRead.Where(z => z.Key != "$schema"))
        {
            yield return (key, $$""" "{{key}}": {{value}} """);
        }
    }

    [Test]
    [MethodDataSource(nameof(NotReadCases))]
    public void WhatIsNotRead_IsValidToSchemaStore_ButNotToUs(string name, string section)
    {
        var instance = Config(section);

        SchemaStoreValidator().Validate(instance).ShouldBeEmpty($"{name}, SchemaStore's schema");
        Validator().IsValid(instance).ShouldBeFalse($"{name}, Dovetail's fragment");
        HostValidator().IsValid(instance).ShouldBeFalse($"{name}, the host schema");
    }

    [Test]
    public void KeyCasing_IsTheOneDifferenceFromSchemaStore_AndTheHostSchemaStillFollowsIt()
    {
        // Configuration keys ignore case, so the fragment accepts any casing of its keys (a twin for each); SchemaStore's schema
        // lists them case-sensitively with additionalProperties: false. Both apply in the host schema, so SchemaStore decides.
        var instance = Config(""" "minimumlevel": { "default": "Debug", "OVERRIDE": { "System": "Error" } } """);
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
    public void TheKeysThatAreNotRead_AreFlaggedByTheStrictSchema_AndAcceptedWhenStrictnessIsOff()
    {
        var strict = Validator();
        var open = Validator(strict: false);

        foreach (var (key, value) in KeysThatAreNotRead)
        {
            strict.IsValid(Config($"\"{key}\": {value}")).ShouldBeFalse(key);
            // Whatever the casing of the key.
            strict.IsValid(Config($"\"{key.ToUpperInvariant()}\": {value}")).ShouldBeFalse(key.ToUpperInvariant());
            open.IsValid(Config($"\"{key}\": {value}")).ShouldBeTrue(key);
        }
    }

    [Test]
    public void TheMinimumLevel_IsAcceptedByEverySchema_AndASectionKeyBesidesItIsNot()
    {
        var minimumLevel = """ "MinimumLevel": { "Default": "Debug", "Override": { "Microsoft": "Warning" } } """;

        foreach (var validator in new[] { Validator(), HostValidator(), SchemaStoreValidator(), })
        {
            validator.Validate(Config(minimumLevel)).ShouldBeEmpty();
            validator.IsValid(Config(minimumLevel + ", \"MinimumLevell\": \"Debug\"")).ShouldBeFalse();
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
