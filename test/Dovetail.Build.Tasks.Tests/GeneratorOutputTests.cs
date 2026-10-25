using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static Dovetail.Build.Tasks.Tests.Fragments;

namespace Dovetail.Build.Tasks.Tests;

/// <summary>
///     The merge against what Dovetail's generator really emits: the fragments in <c>generator-output/</c> are copies of the
///     snapshots in Dovetail.Analyzers.Tests (sections as <c>## Path</c> headings followed by the fragment, then
///     <c>## Diagnostics</c>), covering definitions with dots in their names, recursive types, dictionaries with key patterns,
///     constructor-bound and required members.
/// </summary>
public class GeneratorOutputTests
{
    public static IEnumerable<string> Files() =>
        Directory.EnumerateFiles(Path.Combine(AppContext.BaseDirectory, "generator-output"), "*.txt").Select(Path.GetFileName).OrderBy(z => z, StringComparer.Ordinal)!;

    [Test]
    [MethodDataSource(nameof(Files))]
    public void EachFile_MergesIntoAWellFormedApplicationSchema_AndAPermissiveLibrarySegment(string file)
    {
        var fragments = Read(file);
        fragments.ShouldNotBeEmpty();

        var application = SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(fragments, strict: true, includeSchemaStore: true))
        );
        var library = SchemaAssert.ShouldBeWellFormed(ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateLibrarySchema(fragments)!));

        // Every Dovetail object that lists properties is closed, however it is reached: sections and hoisted definitions.
        foreach (var definition in SchemaAssert.DovetailDefinitions(application))
        {
            foreach (var schema in SchemaAssert.Objects(definition.Value).Where(z => z["properties"] is JsonObject properties && z["additionalProperties"] is null && !IsSharedParent(properties)))
            {
                schema["propertyNames"].ShouldNotBeNull($"{definition.Key} has an open object with properties: {schema.ToJsonString()}");
            }
        }

        SchemaAssert.Objects(library).ShouldAllBe(z => !z.ContainsKey("propertyNames") || z["propertyNames"]!["pattern"] is JsonValue, "only dictionary key patterns");
        library["properties"]!.AsObject().Select(z => z.Key).ShouldBe(fragments.Select(z => z.Path[0]).Distinct(StringComparer.OrdinalIgnoreCase), ignoreOrder: true);
    }

    [Test]
    public void AllFilesTogether_Merge_AndTheDefinitionsStayUnique()
    {
        var fragments = Files().SelectMany(Read).ToArray();

        var document = SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(fragments, strict: true, includeSchemaStore: true))
        );

        var names = document["definitions"]!.AsObject().Select(z => z.Key).ToArray();
        names.Distinct(StringComparer.OrdinalIgnoreCase).Count().ShouldBe(names.Length);
        // The recursive type (Reports_problems.txt's Tree) refers to itself under its new name.
        document["definitions"]!.AsObject().Where(z => z.Key.StartsWith("Tree.", StringComparison.Ordinal))
                                .ShouldContain(z => SchemaAssert.References(z.Value!).Contains("#/definitions/" + z.Key));
    }

    [Test]
    public async Task InterfacesAndAbstractClasses_AreDefinitionsLikeAnyOther()
    {
        // As the generator describes ShapesOptions { IShape Shape; Base Item }: their declared members, as definitions.
        var shapes = Section(
            "Shapes",
            Properties(("Shape", Uses("Sample.IShape")), ("Item", Uses("Sample.Base"))),
            Definitions(
                ("Sample.IShape", Definition(Properties(("Sides", Integer())), "A shape.")),
                ("Sample.Base", Definition(Properties(("Name", String()))))
            )
        );
        var other = Section("Other", Properties(("Shape", Uses("Sample.IShape"))), Definitions(("Sample.IShape", Definition(Properties(("Sides", Integer()), ("Fill", String()))))));

        var json = ConfigurationSchemaDocument.Serialize(
            ConfigurationSchemaDocument.CreateApplicationSchema([Parse("Shapes", shapes), Parse("Other", other)], strict: true, includeSchemaStore: false)
        );

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document["definitions"]!["Shapes.Sample.IShape"]!["propertyNames"]!["patternErrorMessage"]!.GetValue<string>()
            .ShouldBe("Unknown key in \"Sample.IShape\" (section \"Shapes\"). Known keys (case-insensitive): Sides.");
        document["definitions"]!["Other.Sample.IShape"]!["propertyNames"]!["pattern"]!.GetValue<string>().ShouldContain("[Ff][Ii][Ll][Ll]");
        await Verify(json, "json");
    }

    [Test]
    public void AnObjectWithoutBindableMembers_IsClosedToo()
    {
        // An interface or abstract class with nothing to bind has no valid keys.
        var section = Section("Empty", Properties(("Marker", Uses("Sample.IMarker"))), Definitions(("Sample.IMarker", Definition(Properties()))));

        var document = SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([Parse("Empty", section)], strict: true, includeSchemaStore: false))
        );

        var pattern = new Regex(document["definitions"]!["Empty.Sample.IMarker"]!["propertyNames"]!["pattern"]!.GetValue<string>(), RegexOptions.ECMAScript);
        pattern.IsMatch("Anything").ShouldBeFalse();
    }

    // A section that only exists because of deeper ones (Shared for Shared:A) stays open: its properties are bare references.
    private static bool IsSharedParent(JsonObject properties) =>
        properties.Count > 0 && properties.All(z => z.Value is JsonObject { Count: 1, } value && value["$ref"] is not null);

    private static ConfigurationSchemaFragment[] Read(string file)
    {
        var fragments = new List<ConfigurationSchemaFragment>();
        string? section = null;
        var json = new List<string>();

        void Flush()
        {
            if (section is not null and not "Diagnostics") fragments.Add(Parse(section, string.Join('\n', json)));
            json.Clear();
        }

        foreach (var line in File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "generator-output", file)))
        {
            if (line.TrimStart('﻿').StartsWith("## ", StringComparison.Ordinal))
            {
                Flush();
                section = line.TrimStart('﻿')[3..];
            }
            else
            {
                json.Add(line);
            }
        }

        Flush();
        return [.. fragments];
    }
}
