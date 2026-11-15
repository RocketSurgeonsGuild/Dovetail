using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     A hand-written fragment: a schema a library writes itself (as an <c>AssemblyMetadata</c> attribute) rather than one the
///     generator emits for an options type, merged into an application schema like any other. The fixture is the fragment
///     Joinery.Dovetail.Serilog writes for the <c>Serilog</c> section, which describes only what it reads (the minimum level, as a
///     level name or as an object with <c>Default</c> and <c>Override</c>) and follows SchemaStore's <c>Serilog</c> definition, which
///     every host schema embeds, narrowed to that. The tests check the merge against SchemaStore's definition: where the two agree,
///     and where the fragment deliberately says no (Off, level switches, and the keys it does not describe).
/// </summary>
public class HandWrittenFragmentTests
{
    private static readonly string[] Levels = ["Verbose", "Debug", "Information", "Warning", "Error", "Fatal"];

    /// <summary>The keys of Serilog.Settings.Configuration's format that the fragment does not describe, with a value each.</summary>
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

    /// <summary>The fixture, parsed as the build task parses a fragment it reads from an assembly's metadata.</summary>
    private static IReadOnlyList<ConfigurationSchemaFragment> ReadFragments() =>
        [Fragments.Parse("Serilog", File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fixtures", "Serilog.fragment.json")), "HandWritten")];

    private static async Task<JsonObject> ApplicationSchema(bool strict = true, bool includeSchemaStore = false) =>
        await SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict, includeSchemaStore))
        );

    /// <summary>Our fragment alone, through the application schema without SchemaStore.</summary>
    private static SchemaInstanceValidator Validator(bool strict = true) =>
        new(JsonNode.Parse(ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict, includeSchemaStore: false)))!);

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
    public async Task TheFixture_IsOneFragmentForTheSerilogSection()
    {
        var fragments = ReadFragments();

        await fragments.Count.Should().BeEqualTo(1);
        await fragments[0].Path.Should().BeEquivalentTo(["Serilog"], CollectionOrdering.Matching);
        await fragments[0].Source.Should().BeEqualTo("HandWritten");
    }

    [Test]
    public async Task TheLibrarySegment_IsWellFormed_AndPermissive()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateLibrarySchema(ReadFragments())!);

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document["properties"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(["Serilog"], CollectionOrdering.Matching);
        await SchemaAssert.Objects(document).Should().All(z => !z.ContainsKey("propertyNames"));
        await Verify(json, "json");
    }

    [Test]
    public async Task TheApplicationSchema_ClosesTheSectionAndItsObjects_WithCaseInsensitiveKeys()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(ReadFragments(), strict: true, includeSchemaStore: false));

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        var closed = SchemaAssert.DovetailDefinitions(document).Where(z => z.Value!["propertyNames"] is not null).Select(z => z.Key).ToArray();

        // The section's own keys: the one it reads.
        var section = SchemaAssert.DovetailDefinitions(document).Single(z => z.Key == "Serilog.Settings").Value!.AsObject();
        await section["properties"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(["MinimumLevel"], CollectionOrdering.Matching);
        await section["propertyNames"].Should().NotBeNull();
        await closed.Should().Contain("Serilog.MinimumLevelObject");
        await Verify(json, "json");
    }

    [Test]
    public async Task TheEveryKeyHasACaseInsensitiveTwin_BuiltTheWayTheWriterBuildsIt()
    {
        var definitions = ReadFragments().Single().Schema["definitions"]!.AsObject();

        foreach (var definition in definitions.Where(z => z.Value!["properties"] is JsonObject))
        {
            var patterns = new List<string>();
            foreach (var property in definition.Value!["properties"]!.AsObject())
            {
                patterns.Add(CaseInsensitivePattern.For(property.Key));
            }

            var patternKeys = new List<string>();
            foreach (var patternProperty in definition.Value["patternProperties"]!.AsObject())
            {
                patternKeys.Add(patternProperty.Key);
            }

            await patternKeys.Should().BeEquivalentTo(patterns, CollectionOrdering.Matching).Because($"{definition.Key}'s patternProperties");
        }
    }

    [Test]
    public async Task TheLevels_AreSchemaStoresLevelsWithoutOff()
    {
        var ours = ReadFragments().Single().Schema["definitions"]!.AsObject();
        var theirs = SchemaStoreSerilog();
        var oursLevels = ours["Level"]!["enum"]!.AsArray().Select(z => z!.GetValue<string>()).ToArray();

        // Serilog's six levels (Joinery.Dovetail.Serilog.Tests checks them against LogEventLevel). SchemaStore also lists Off,
        // which Serilog has no level for.
        await oursLevels.Should().BeEquivalentTo(Levels, CollectionOrdering.Matching);
        await oursLevels.Should().BeEquivalentTo(theirs["SerilogLogEventLevel"]!["enum"]!.AsArray().Select(z => z!.GetValue<string>()).Where(z => z != "Off"), CollectionOrdering.Matching);
        await ours["Level"]!["type"]!.ToJsonString().Should().BeEqualTo(theirs["SerilogLogEventLevel"]!["type"]!.ToJsonString());
    }

    [Test]
    public async Task TheSectionIsPartOfTheHostSchema_WithOrWithoutSchemaStore()
    {
        foreach (var includeSchemaStore in new[] { false, true, })
        {
            var document = await ApplicationSchema(includeSchemaStore: includeSchemaStore);

            await SchemaAssert.DovetailPart(document)["properties"]!.AsObject().ContainsKey("Serilog").Should().BeTrue();
            var settings = document["definitions"]!["Serilog.Settings"];
            await settings.Should().NotBeNull();
            await settings!["properties"]!["MinimumLevel"]!["oneOf"].Should().NotBeNull();
        }
    }

    /// <summary>
    ///     Configurations of the keys the fragment describes and whether they are valid. SchemaStore's schema, ours and the host schema that
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
    public async Task SchemaStoresRulesAndOurs_GiveEachConfigurationTheSameVerdict(string name, string section, bool valid)
    {
        var instance = Config(section);

        var schemaStore = SchemaStoreValidator().Validate(instance);
        var ours = Validator().Validate(instance);
        var host = HostValidator().Validate(instance);

        await ( schemaStore.Count == 0 ).Should().BeEqualTo(valid).Because($"{name}, SchemaStore's schema: {string.Join("; ", schemaStore)}");
        await ( ours.Count == 0 ).Should().BeEqualTo(valid).Because($"{name}, Dovetail's fragment: {string.Join("; ", ours)}");
        await ( host.Count == 0 ).Should().BeEqualTo(valid).Because($"{name}, the host schema: {string.Join("; ", host)}");
    }

    /// <summary>
    ///     Configurations that SchemaStore's schema accepts (they are Serilog.Settings.Configuration's format) and that the fragment
    ///     does not describe, so the fragment says no, and so does the host schema that contains both.
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
    public async Task WhatIsNotRead_IsValidToSchemaStore_ButNotToUs(string name, string section)
    {
        var instance = Config(section);

        await SchemaStoreValidator().Validate(instance).Should().BeEmpty().Because($"{name}, SchemaStore's schema");
        await Validator().IsValid(instance).Should().BeFalse().Because($"{name}, Dovetail's fragment");
        await HostValidator().IsValid(instance).Should().BeFalse().Because($"{name}, the host schema");
    }

    [Test]
    public async Task KeyCasing_IsTheOneDifferenceFromSchemaStore_AndTheHostSchemaStillFollowsIt()
    {
        // Configuration keys ignore case, so the fragment accepts any casing of its keys (a twin for each); SchemaStore's schema
        // lists them case-sensitively with additionalProperties: false. Both apply in the host schema, so SchemaStore decides.
        var instance = Config(""" "minimumlevel": { "default": "Debug", "OVERRIDE": { "System": "Error" } } """);
        var wrongValue = Config(""" "minimumlevel": 2 """);

        await Validator().IsValid(instance).Should().BeTrue();
        await SchemaStoreValidator().IsValid(instance).Should().BeFalse();
        await HostValidator().IsValid(instance).Should().BeFalse();
        await Validator().IsValid(wrongValue).Should().BeFalse();
    }

    [Test]
    public async Task FlatKeys_AreAcceptedBySchemaStore_ButClosedOutByDovetailsStrictness()
    {
        // "WriteTo:Async" and "Enrich:0" are documented Serilog syntax and SchemaStore lists patterns for them. The strict
        // merge closes the section with the names it knows, so they are flagged unless strictness is off.
        var instance = Config(""" "WriteTo:Async": { "Name": "Async" }, "Enrich:0": "FromLogContext", "Enrich:Machine": { "Name": "WithMachineName" } """);

        await SchemaStoreValidator().IsValid(instance).Should().BeTrue();
        await Validator().IsValid(instance).Should().BeFalse();
        await HostValidator().IsValid(instance).Should().BeFalse();
        await Validator(strict: false).IsValid(instance).Should().BeTrue();
    }

    [Test]
    public async Task TheKeysThatAreNotRead_AreFlaggedByTheStrictSchema_AndAcceptedWhenStrictnessIsOff()
    {
        var strict = Validator();
        var open = Validator(strict: false);

        foreach (var (key, value) in KeysThatAreNotRead)
        {
            await strict.IsValid(Config($"\"{key}\": {value}")).Should().BeFalse().Because(key);
            // Whatever the casing of the key.
            await strict.IsValid(Config($"\"{key.ToUpperInvariant()}\": {value}")).Should().BeFalse().Because(key.ToUpperInvariant());
            await open.IsValid(Config($"\"{key}\": {value}")).Should().BeTrue().Because(key);
        }
    }

    [Test]
    public async Task TheMinimumLevel_IsAcceptedByEverySchema_AndASectionKeyBesidesItIsNot()
    {
        var minimumLevel = """ "MinimumLevel": { "Default": "Debug", "Override": { "Microsoft": "Warning" } } """;

        foreach (var validator in new[] { Validator(), HostValidator(), SchemaStoreValidator(), })
        {
            await validator.Validate(Config(minimumLevel)).Should().BeEmpty();
            await validator.IsValid(Config(minimumLevel + ", \"MinimumLevell\": \"Debug\"")).Should().BeFalse();
        }
    }

    [Test]
    public async Task TheSchemaIsOpenWhenStrictnessIsOff_ExceptForTheValuesItModels()
    {
        // Without strictness no key is closed (typos pass), but a wrong value is still wrong.
        var validator = Validator(strict: false);

        await validator.IsValid(Config(""" "Minimum": "Debug" """)).Should().BeTrue();
        await validator.IsValid(Config(""" "MinimumLevel": { "Defualt": "Debug" } """)).Should().BeTrue();
        await validator.IsValid(Config(""" "MinimumLevel": 2 """)).Should().BeFalse();
        await validator.IsValid(Config(""" "MinimumLevel": { "Default": "Not a level" } """)).Should().BeFalse();
    }
}
