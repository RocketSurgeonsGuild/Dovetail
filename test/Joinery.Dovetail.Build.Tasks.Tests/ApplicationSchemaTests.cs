using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static Joinery.Dovetail.Build.Tasks.Tests.Fragments;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The application schema MergeConfigurationSchema writes: the merge tree, strictness, overlaps, definition hoisting and the
///     SchemaStore base. Every output is checked against the draft-07 meta-schema.
/// </summary>
public class ApplicationSchemaTests
{
    [Test]
    public async Task Section_IsClosedWithCaseInsensitivePropertyNames()
    {
        var json = Merge(GreetingsFragment());

        await SchemaAssert.ShouldBeWellFormed(json);
        await Verify(json, "json");
    }

    [Test]
    public async Task PropertyNamesPattern_AcceptsAnyCasingAndRejectsTypos()
    {
        var document = await SchemaAssert.ShouldBeWellFormed(Merge(GreetingsFragment()));
        var propertyNames = SchemaAssert.Section(document, "Greetings")["propertyNames"]!;
        var pattern = new Regex(propertyNames["pattern"]!.GetValue<string>(), RegexOptions.ECMAScript);

        await pattern.IsMatch("Message").Should().BeTrue();
        await pattern.IsMatch("message").Should().BeTrue();
        await pattern.IsMatch("MESSAGE").Should().BeTrue();
        await pattern.IsMatch("mEsSaGe").Should().BeTrue();
        await pattern.IsMatch("Endpoint").Should().BeTrue();
        await pattern.IsMatch("Mesage").Should().BeFalse();
        await pattern.IsMatch("Message2").Should().BeFalse();
        await pattern.IsMatch("").Should().BeFalse();
        await propertyNames["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in section \"Greetings\". Known keys (case-insensitive): Message, Count, Endpoint, Headers, Servers.");
        await propertyNames["doNotSuggest"]!.GetValue<bool>().Should().BeTrue();
    }

    [Test]
    public async Task DefinitionsAreClosed_WhileTheRootWrappersAndDictionariesStayOpen()
    {
        var document = await SchemaAssert.ShouldBeWellFormed(Merge(GreetingsFragment()));
        var dovetail = SchemaAssert.DovetailPart(document);
        var greetings = SchemaAssert.Section(document, "Greetings");
        var endpoint = document["definitions"]!["Greetings.Endpoint"]!;

        await dovetail.ContainsKey("propertyNames").Should().BeFalse();
        await greetings["properties"]!["Endpoint"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await greetings["properties"]!["Endpoint"]!["allOf"]![0]!["$ref"]!.GetValue<string>().Should().BeEqualTo("#/definitions/Greetings.Endpoint");
        await greetings["properties"]!["Headers"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await greetings["properties"]!["Servers"]!["items"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await endpoint["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in \"Endpoint\" (section \"Greetings\"). Known keys (case-insensitive): Url, Timeout.");
        await endpoint.AsObject().ContainsKey("type").Should().BeFalse();
        await document["definitions"]!["Greetings.Server"]!["propertyNames"].Should().NotBeNull();
    }

    [Test]
    public async Task Definitions_AreHoistedUnderUniqueNames_AndTheirReferencesRewritten()
    {
        // Two fragments define a "Retry" of their own, and a third a type that refers to itself.
        var json = Merge(
            Parse("Http", Section("Http", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer()))))))),
            Parse("Grpc", Section("Grpc", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Backoff", String()))))))),
            Parse("Tree", Section("Tree", Properties(("Root", Uses("Node"))), Definitions(("Node", Definition(Properties(("Name", String()), ("Children", ArrayOf(Uses("Node")))))))))
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await SchemaAssert.DovetailDefinitions(document).Select(z => z.Key).Should().BeEquivalentTo(["Grpc", "Grpc.Retry", "Http", "Http.Retry", "Tree", "Tree.Node"], CollectionOrdering.Matching);
        await document["definitions"]!["Tree.Node"]!["properties"]!["Children"]!["items"]!["allOf"]![0]!["$ref"]!.GetValue<string>().Should().BeEqualTo("#/definitions/Tree.Node");
        await SchemaAssert.Objects(SchemaAssert.DovetailPart(document)["properties"]).Should().All(z => !z.ContainsKey("definitions"));
        await Verify(json, "json");
    }

    [Test]
    public async Task OverlappingSections_AreAnAllOfOfOpenBranches_ClosedOnceWithEveryKey()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Timeout", Integer()), ("BaseAddress", String()))), source: "Lib1"),
            Parse("shared", Section("shared", Properties(("Retries", Integer()), ("Backoff", String()))), source: "Lib2")
        );

        var shared = SchemaAssert.Section(await SchemaAssert.ShouldBeWellFormed(json), "Shared");
        await shared["type"]!.GetValue<string>().Should().BeEqualTo("object");
        await shared["allOf"]!.AsArray().Should().All(z => !z!.AsObject().ContainsKey("propertyNames"));
        await shared["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in section \"Shared\". Known keys (case-insensitive): Timeout, BaseAddress, Retries, Backoff.");
        await Verify(json, "json");
    }

    [Test]
    public async Task OverlappingSections_WithDifferentTypesForAKey_MergeThatKeyAndLeaveItsDefinitionsOpen()
    {
        // Both define a "Retry" type, with different keys, for the same "Retry" key.
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib1"),
            Parse("Shared", Section("Shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Backoff", String())))))), source: "Lib2")
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        var retry = SchemaAssert.Section(document, "Shared", "Retry");
        await retry["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in section \"Shared:Retry\". Known keys (case-insensitive): Attempts, Backoff.");
        await document["definitions"]!["Shared.Retry"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await document["definitions"]!["Shared.Retry.2"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await Verify(json, "json");
    }

    [Test]
    public async Task OverlappingSections_WithTheSameTypeForAKey_KeepItsDefinitionsClosed()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Timeout", Integer()), ("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib1"),
            Parse("Shared", Section("Shared", Properties(("Enabled", Boolean()), ("Retry", Uses("Retry", description: "How to retry."))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib2")
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document["definitions"]!["Shared.Retry"]!["propertyNames"].Should().NotBeNull();
        await document["definitions"]!["Shared.Retry.2"]!["propertyNames"].Should().NotBeNull();
        await SchemaAssert.Section(document, "Shared", "Retry")["allOf"]!.AsArray().Count.Should().BeEqualTo(2);
    }

    [Test]
    public async Task ASectionThatIsAlsoAPropertyOfItsParent_IsMergedWithIt()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Http", Uses("Http"))), Definitions(("Http", Definition(Properties(("Timeout", Integer())))))), source: "Lib1"),
            Parse("Shared:Http", Section("Shared:Http", Properties(("Retries", Integer()))), source: "Lib2")
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        var http = SchemaAssert.Section(document, "Shared", "Http");
        await http["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in section \"Shared:Http\". Known keys (case-insensitive): Timeout, Retries.");
        await document["definitions"]!["Shared.Http"]!.AsObject().ContainsKey("propertyNames").Should().BeFalse();
        await Verify(json, "json");
    }

    [Test]
    public async Task SharedParents_AndTheRoot_StayOpen()
    {
        var json = Merge(
            Parse("Shared:Http", Section("Shared:Http", Properties(("Timeout", Integer())))),
            Parse("Shared:Grpc", Section("Shared:Grpc", Properties(("Port", Integer()))))
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await SchemaAssert.DovetailPart(document).ContainsKey("propertyNames").Should().BeFalse();
        await SchemaAssert.Section(document, "Shared").ContainsKey("propertyNames").Should().BeFalse();
        await SchemaAssert.Section(document, "Shared", "Http")["propertyNames"].Should().NotBeNull();
        await Verify(json, "json");
    }

    [Test]
    public async Task IdenticalFragments_AreMergedOnce()
    {
        var json = Merge(
            Parse("Greetings", GreetingsSection(), source: "Lib1"),
            Parse("Greetings", GreetingsSection(), source: "Lib2")
        );

        await SchemaAssert.ShouldBeWellFormed(json);
        await json.Should().BeEqualTo(Merge(Parse("Greetings", GreetingsSection(), source: "Lib1")));
    }

    [Test]
    public async Task TheMergedSection_IsSpelledTheOrdinalSmallestWay()
    {
        var document = await SchemaAssert.ShouldBeWellFormed(
            Merge(
                Parse("greetings", Section("greetings", Properties(("message", String()))), source: "Lib1"),
                Parse("Greetings", Section("Greetings", Properties(("Message", String()))), source: "Lib2")
            )
        );

        var properties = SchemaAssert.DovetailPart(document)["properties"]!.AsObject();
        await properties.Select(z => z.Key).Should().BeEquivalentTo(["Greetings"], CollectionOrdering.Matching);
        await SchemaAssert.Section(document, "Greetings")["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().EndWith("Known keys (case-insensitive): Message.");
    }

    [Test]
    public async Task TheOutput_DoesNotDependOnTheOrderOfFragments()
    {
        ConfigurationSchemaFragment[] fragments =
        [
            Parse("Greetings", GreetingsSection(), source: "Lib1"),
            Parse("Shared", Section("Shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib1"),
            Parse("shared", Section("shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Backoff", String())))))), source: "Lib2"),
            Parse("Shared:Http", Section("Shared:Http", Properties(("Timeout", Integer()))), source: "Lib3"),
        ];
        var expected = Merge(fragments);
        await SchemaAssert.ShouldBeWellFormed(expected);

        foreach (var permutation in Permutations(fragments))
        {
            await Merge(permutation).Should().BeEqualTo(expected);
        }
    }

    [Test]
    public async Task NotStrict_LeavesEverySectionAndDefinitionOpen()
    {
        var document = await SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(
                ConfigurationSchemaDocument.CreateApplicationSchema(
                    [
                        GreetingsFragment(),
                        Parse("Shared", Section("Shared", Properties(("Timeout", Integer()))), source: "Lib1"),
                        Parse("Shared", Section("Shared", Properties(("Retries", Integer()))), source: "Lib2"),
                    ],
                    strict: false,
                    includeSchemaStore: false
                )
            )
        );

        await SchemaAssert.Objects(document).Should().All(z => !z.ContainsKey("propertyNames"));
        await SchemaAssert.DovetailDefinitions(document).Should().NotBeEmpty();
    }

    [Test]
    public async Task FragmentTwinsSpelledDifferently_AreStillReplacedWhenTheirKeyIsMerged()
    {
        // A twin pattern that isn't the one Dovetail builds itself is matched against the keys instead.
        var shared = """
                     {"type":"object","properties":{"Http":{"type":"object","properties":{"Timeout":{"type":"integer"}}}},
                      "patternProperties":{"^[Hh][Tt][Tt][Pp]$":{"type":"object","properties":{"Timeout":{"type":"integer"}}}}}
                     """;
        var document = await SchemaAssert.ShouldBeWellFormed(
            Merge(Parse("Shared", shared, source: "Lib1"), Parse("Shared:Http", Section("Shared:Http", Properties(("Retries", Integer()))), source: "Lib2"))
        );

        var patternProperties = SchemaAssert.Section(document, "Shared")["patternProperties"]!.AsObject();
        await patternProperties.Select(z => z.Key).Should().BeEquivalentTo([CaseInsensitivePattern.For("Http")], CollectionOrdering.Matching);
    }

    [Test]
    public async Task InlineNestedObjects_AreClosedToo()
    {
        // Hand-written fragments may nest objects inline rather than through definitions.
        var document = await SchemaAssert.ShouldBeWellFormed(
            Merge(Parse("Greetings", """{"type":"object","properties":{"Endpoint":{"type":"object","properties":{"Url":{"type":"string"}}}}}"""))
        );

        await SchemaAssert.Section(document, "Greetings", "Endpoint")["propertyNames"]!["patternErrorMessage"]!
                    .GetValue<string>().Should().BeEqualTo("Unknown key in section \"Greetings:Endpoint\". Known keys (case-insensitive): Url.");
    }

    [Test]
    public async Task SchemaStore_IsEmbeddedUnderItsOwnDefinition_WithItsReferencesRelocated()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([GreetingsFragment()], strict: true, includeSchemaStore: true));

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document.ContainsKey("title").Should().BeFalse();
        await document["$schema"]!.GetValue<string>().Should().BeEqualTo("http://json-schema.org/draft-07/schema#");
        await document["$comment"]!.GetValue<string>().Should().Contain("Apache License 2.0");
        await document["allOf"]![0]!.ToJsonString().Should().BeEqualTo("""{"$ref":"#/definitions/SchemaStoreAppSettings"}""");
        await document["definitions"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(["SchemaStoreAppSettings", "Greetings", "Greetings.Endpoint", "Greetings.Server"], CollectionOrdering.Matching);

        var embedded = document["definitions"]!["SchemaStoreAppSettings"]!.AsObject();
        await embedded.ContainsKey("$schema").Should().BeFalse();
        await embedded.ContainsKey("$id").Should().BeFalse();
        await embedded.ContainsKey("title").Should().BeFalse();
        await embedded["properties"]!["Logging"].Should().NotBeNull();

        // The same references as the pinned copy of SchemaStore's file, each pointing into the definition.
        using var pinned = typeof(SchemaStoreAppSettings).Assembly.GetManifestResourceStream("Joinery.Dovetail.Build.Tasks.SchemaStore.appsettings.json")!;
        var original = JsonNode.Parse(pinned)!;
        await SchemaAssert.References(embedded).Should().BeEquivalentTo(SchemaAssert.References(original).Select(z => "#/definitions/SchemaStoreAppSettings" + z[1..]), CollectionOrdering.Matching);
    }

    [Test]
    public async Task WithoutFragments_TheSchemaIsSchemaStoreAndAnEmptyDovetailPart()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([], strict: true, includeSchemaStore: true));

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document["allOf"]![1]!.ToJsonString().Should().BeEqualTo("""{"type":"object","properties":{}}""");
        document["definitions"]!["SchemaStoreAppSettings"] = "{SchemaStore}";
        await Verify(ConfigurationSchemaDocument.Serialize(document), "json");
    }

    [Test]
    public async Task TheMetaSchemaCheck_RejectsInvalidSchemas()
    {
        await SchemaAssert.IsValidDraft07("""{"type":"object","properties":{"Message":{"type":"string"}}}""").Should().BeTrue();
        await SchemaAssert.IsValidDraft07("""{"type":12}""").Should().BeFalse();
        await SchemaAssert.IsValidDraft07("""{"properties":{"Message":"string"}}""").Should().BeFalse();
        await SchemaAssert.IsValidDraft07("""{"required":"Message"}""").Should().BeFalse();
    }

    internal static ConfigurationSchemaFragment GreetingsFragment() => Parse("Greetings", GreetingsSection(), source: "Greetings");

    internal static JsonObject GreetingsSection() =>
        Section(
            "Greetings",
            Properties(
                ("Message", String("Text placed before the name.")),
                ("Count", Integer()),
                ("Endpoint", Uses("Endpoint", nullable: true)),
                ("Headers", DictionaryOf(String())),
                ("Servers", ArrayOf(Uses("Server")))
            ),
            Definitions(
                ("Endpoint", Definition(Properties(("Url", String()), ("Timeout", Integer())), "Where to send greetings.")),
                ("Server", Definition(Properties(("Host", String()), ("Port", Integer()))))
            ),
            "Options of the greeter."
        );

    private static string Merge(params IReadOnlyList<ConfigurationSchemaFragment> fragments) =>
        ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(fragments, strict: true, includeSchemaStore: false));

    private static IEnumerable<IReadOnlyList<T>> Permutations<T>(IReadOnlyList<T> items) =>
        items.Count <= 1
            ? [items]
            : items.SelectMany((item, index) => Permutations([.. items.Where((_, other) => other != index)]).Select(rest => (IReadOnlyList<T>)[item, .. rest]));
}
