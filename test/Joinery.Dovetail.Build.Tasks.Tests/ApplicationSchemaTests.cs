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

        SchemaAssert.ShouldBeWellFormed(json);
        await Verify(json, "json");
    }

    [Test]
    public void PropertyNamesPattern_AcceptsAnyCasingAndRejectsTypos()
    {
        var document = SchemaAssert.ShouldBeWellFormed(Merge(GreetingsFragment()));
        var propertyNames = SchemaAssert.Section(document, "Greetings")["propertyNames"]!;
        var pattern = new Regex(propertyNames["pattern"]!.GetValue<string>(), RegexOptions.ECMAScript);

        pattern.IsMatch("Message").ShouldBeTrue();
        pattern.IsMatch("message").ShouldBeTrue();
        pattern.IsMatch("MESSAGE").ShouldBeTrue();
        pattern.IsMatch("mEsSaGe").ShouldBeTrue();
        pattern.IsMatch("Endpoint").ShouldBeTrue();
        pattern.IsMatch("Mesage").ShouldBeFalse();
        pattern.IsMatch("Message2").ShouldBeFalse();
        pattern.IsMatch("").ShouldBeFalse();
        propertyNames["patternErrorMessage"]!.GetValue<string>()
                                             .ShouldBe("Unknown key in section \"Greetings\". Known keys (case-insensitive): Message, Count, Endpoint, Headers, Servers.");
        propertyNames["doNotSuggest"]!.GetValue<bool>().ShouldBeTrue();
    }

    [Test]
    public void DefinitionsAreClosed_WhileTheRootWrappersAndDictionariesStayOpen()
    {
        var document = SchemaAssert.ShouldBeWellFormed(Merge(GreetingsFragment()));
        var dovetail = SchemaAssert.DovetailPart(document);
        var greetings = SchemaAssert.Section(document, "Greetings");
        var endpoint = document["definitions"]!["Greetings.Endpoint"]!;

        dovetail.ContainsKey("propertyNames").ShouldBeFalse();
        greetings["properties"]!["Endpoint"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        greetings["properties"]!["Endpoint"]!["allOf"]![0]!["$ref"]!.GetValue<string>().ShouldBe("#/definitions/Greetings.Endpoint");
        greetings["properties"]!["Headers"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        greetings["properties"]!["Servers"]!["items"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        endpoint["propertyNames"]!["patternErrorMessage"]!.GetValue<string>()
                                                          .ShouldBe("Unknown key in \"Endpoint\" (section \"Greetings\"). Known keys (case-insensitive): Url, Timeout.");
        endpoint.AsObject().ContainsKey("type").ShouldBeFalse();
        document["definitions"]!["Greetings.Server"]!["propertyNames"].ShouldNotBeNull();
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

        var document = SchemaAssert.ShouldBeWellFormed(json);
        SchemaAssert.DovetailDefinitions(document).Select(z => z.Key).ShouldBe(["Grpc", "Grpc.Retry", "Http", "Http.Retry", "Tree", "Tree.Node"]);
        document["definitions"]!["Tree.Node"]!["properties"]!["Children"]!["items"]!["allOf"]![0]!["$ref"]!.GetValue<string>().ShouldBe("#/definitions/Tree.Node");
        SchemaAssert.Objects(SchemaAssert.DovetailPart(document)["properties"]).ShouldAllBe(z => !z.ContainsKey("definitions"));
        await Verify(json, "json");
    }

    [Test]
    public async Task OverlappingSections_AreAnAllOfOfOpenBranches_ClosedOnceWithEveryKey()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Timeout", Integer()), ("BaseAddress", String()))), source: "Lib1"),
            Parse("shared", Section("shared", Properties(("Retries", Integer()), ("Backoff", String()))), source: "Lib2")
        );

        var shared = SchemaAssert.Section(SchemaAssert.ShouldBeWellFormed(json), "Shared");
        shared["type"]!.GetValue<string>().ShouldBe("object");
        shared["allOf"]!.AsArray().ShouldAllBe(z => !z!.AsObject().ContainsKey("propertyNames"));
        shared["propertyNames"]!["patternErrorMessage"]!.GetValue<string>()
                                                        .ShouldBe("Unknown key in section \"Shared\". Known keys (case-insensitive): Timeout, BaseAddress, Retries, Backoff.");
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

        var document = SchemaAssert.ShouldBeWellFormed(json);
        var retry = SchemaAssert.Section(document, "Shared", "Retry");
        retry["propertyNames"]!["patternErrorMessage"]!.GetValue<string>()
                                                       .ShouldBe("Unknown key in section \"Shared:Retry\". Known keys (case-insensitive): Attempts, Backoff.");
        document["definitions"]!["Shared.Retry"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        document["definitions"]!["Shared.Retry.2"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        await Verify(json, "json");
    }

    [Test]
    public void OverlappingSections_WithTheSameTypeForAKey_KeepItsDefinitionsClosed()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Timeout", Integer()), ("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib1"),
            Parse("Shared", Section("Shared", Properties(("Enabled", Boolean()), ("Retry", Uses("Retry", description: "How to retry."))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib2")
        );

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document["definitions"]!["Shared.Retry"]!["propertyNames"].ShouldNotBeNull();
        document["definitions"]!["Shared.Retry.2"]!["propertyNames"].ShouldNotBeNull();
        SchemaAssert.Section(document, "Shared", "Retry")["allOf"]!.AsArray().Count.ShouldBe(2);
    }

    [Test]
    public async Task ASectionThatIsAlsoAPropertyOfItsParent_IsMergedWithIt()
    {
        var json = Merge(
            Parse("Shared", Section("Shared", Properties(("Http", Uses("Http"))), Definitions(("Http", Definition(Properties(("Timeout", Integer())))))), source: "Lib1"),
            Parse("Shared:Http", Section("Shared:Http", Properties(("Retries", Integer()))), source: "Lib2")
        );

        var document = SchemaAssert.ShouldBeWellFormed(json);
        var http = SchemaAssert.Section(document, "Shared", "Http");
        http["propertyNames"]!["patternErrorMessage"]!.GetValue<string>()
                                                      .ShouldBe("Unknown key in section \"Shared:Http\". Known keys (case-insensitive): Timeout, Retries.");
        document["definitions"]!["Shared.Http"]!.AsObject().ContainsKey("propertyNames").ShouldBeFalse();
        await Verify(json, "json");
    }

    [Test]
    public async Task SharedParents_AndTheRoot_StayOpen()
    {
        var json = Merge(
            Parse("Shared:Http", Section("Shared:Http", Properties(("Timeout", Integer())))),
            Parse("Shared:Grpc", Section("Shared:Grpc", Properties(("Port", Integer()))))
        );

        var document = SchemaAssert.ShouldBeWellFormed(json);
        SchemaAssert.DovetailPart(document).ContainsKey("propertyNames").ShouldBeFalse();
        SchemaAssert.Section(document, "Shared").ContainsKey("propertyNames").ShouldBeFalse();
        SchemaAssert.Section(document, "Shared", "Http")["propertyNames"].ShouldNotBeNull();
        await Verify(json, "json");
    }

    [Test]
    public void IdenticalFragments_AreMergedOnce()
    {
        var json = Merge(
            Parse("Greetings", GreetingsSection(), source: "Lib1"),
            Parse("Greetings", GreetingsSection(), source: "Lib2")
        );

        SchemaAssert.ShouldBeWellFormed(json);
        json.ShouldBe(Merge(Parse("Greetings", GreetingsSection(), source: "Lib1")));
    }

    [Test]
    public void TheMergedSection_IsSpelledTheOrdinalSmallestWay()
    {
        var document = SchemaAssert.ShouldBeWellFormed(
            Merge(
                Parse("greetings", Section("greetings", Properties(("message", String()))), source: "Lib1"),
                Parse("Greetings", Section("Greetings", Properties(("Message", String()))), source: "Lib2")
            )
        );

        var properties = SchemaAssert.DovetailPart(document)["properties"]!.AsObject();
        properties.Select(z => z.Key).ShouldBe(["Greetings"]);
        SchemaAssert.Section(document, "Greetings")["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().ShouldEndWith("Known keys (case-insensitive): Message.");
    }

    [Test]
    public void TheOutput_DoesNotDependOnTheOrderOfFragments()
    {
        ConfigurationSchemaFragment[] fragments =
        [
            Parse("Greetings", GreetingsSection(), source: "Lib1"),
            Parse("Shared", Section("Shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer())))))), source: "Lib1"),
            Parse("shared", Section("shared", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Backoff", String())))))), source: "Lib2"),
            Parse("Shared:Http", Section("Shared:Http", Properties(("Timeout", Integer()))), source: "Lib3"),
        ];
        var expected = Merge(fragments);
        SchemaAssert.ShouldBeWellFormed(expected);

        foreach (var permutation in Permutations(fragments))
        {
            Merge(permutation).ShouldBe(expected);
        }
    }

    [Test]
    public void NotStrict_LeavesEverySectionAndDefinitionOpen()
    {
        var document = SchemaAssert.ShouldBeWellFormed(
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

        SchemaAssert.Objects(document).ShouldAllBe(z => !z.ContainsKey("propertyNames"));
        SchemaAssert.DovetailDefinitions(document).ShouldNotBeEmpty();
    }

    [Test]
    public void FragmentTwinsSpelledDifferently_AreStillReplacedWhenTheirKeyIsMerged()
    {
        // A twin pattern that isn't the one Dovetail builds itself is matched against the keys instead.
        var shared = """
                     {"type":"object","properties":{"Http":{"type":"object","properties":{"Timeout":{"type":"integer"}}}},
                      "patternProperties":{"^[Hh][Tt][Tt][Pp]$":{"type":"object","properties":{"Timeout":{"type":"integer"}}}}}
                     """;
        var document = SchemaAssert.ShouldBeWellFormed(
            Merge(Parse("Shared", shared, source: "Lib1"), Parse("Shared:Http", Section("Shared:Http", Properties(("Retries", Integer()))), source: "Lib2"))
        );

        var patternProperties = SchemaAssert.Section(document, "Shared")["patternProperties"]!.AsObject();
        patternProperties.Select(z => z.Key).ShouldBe([CaseInsensitivePattern.For("Http")]);
    }

    [Test]
    public void InlineNestedObjects_AreClosedToo()
    {
        // Hand-written fragments may nest objects inline rather than through definitions.
        var document = SchemaAssert.ShouldBeWellFormed(
            Merge(Parse("Greetings", """{"type":"object","properties":{"Endpoint":{"type":"object","properties":{"Url":{"type":"string"}}}}}"""))
        );

        SchemaAssert.Section(document, "Greetings", "Endpoint")["propertyNames"]!["patternErrorMessage"]!
                    .GetValue<string>()
                    .ShouldBe("Unknown key in section \"Greetings:Endpoint\". Known keys (case-insensitive): Url.");
    }

    [Test]
    public void SchemaStore_IsEmbeddedUnderItsOwnDefinition_WithItsReferencesRelocated()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([GreetingsFragment()], strict: true, includeSchemaStore: true));

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document.ContainsKey("title").ShouldBeFalse();
        document["$schema"]!.GetValue<string>().ShouldBe("http://json-schema.org/draft-07/schema#");
        document["$comment"]!.GetValue<string>().ShouldContain("Apache License 2.0");
        document["allOf"]![0]!.ToJsonString().ShouldBe("""{"$ref":"#/definitions/SchemaStoreAppSettings"}""");
        document["definitions"]!.AsObject().Select(z => z.Key).ShouldBe(["SchemaStoreAppSettings", "Greetings", "Greetings.Endpoint", "Greetings.Server"]);

        var embedded = document["definitions"]!["SchemaStoreAppSettings"]!.AsObject();
        embedded.ContainsKey("$schema").ShouldBeFalse();
        embedded.ContainsKey("$id").ShouldBeFalse();
        embedded.ContainsKey("title").ShouldBeFalse();
        embedded["properties"]!["Logging"].ShouldNotBeNull();

        // The same references as the pinned copy of SchemaStore's file, each pointing into the definition.
        using var pinned = typeof(SchemaStoreAppSettings).Assembly.GetManifestResourceStream("Joinery.Dovetail.Build.Tasks.SchemaStore.appsettings.json")!;
        var original = JsonNode.Parse(pinned)!;
        SchemaAssert.References(embedded).ShouldBe(SchemaAssert.References(original).Select(z => "#/definitions/SchemaStoreAppSettings" + z[1..]));
    }

    [Test]
    public async Task WithoutFragments_TheSchemaIsSchemaStoreAndAnEmptyDovetailPart()
    {
        var json = ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([], strict: true, includeSchemaStore: true));

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document["allOf"]![1]!.ToJsonString().ShouldBe("""{"type":"object","properties":{}}""");
        document["definitions"]!["SchemaStoreAppSettings"] = "{SchemaStore}";
        await Verify(ConfigurationSchemaDocument.Serialize(document), "json");
    }

    [Test]
    public void TheMetaSchemaCheck_RejectsInvalidSchemas()
    {
        SchemaAssert.IsValidDraft07("""{"type":"object","properties":{"Message":{"type":"string"}}}""").ShouldBeTrue();
        SchemaAssert.IsValidDraft07("""{"type":12}""").ShouldBeFalse();
        SchemaAssert.IsValidDraft07("""{"properties":{"Message":"string"}}""").ShouldBeFalse();
        SchemaAssert.IsValidDraft07("""{"required":"Message"}""").ShouldBeFalse();
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
