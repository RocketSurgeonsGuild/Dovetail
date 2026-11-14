using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static Joinery.Dovetail.Build.Tasks.Tests.Fragments;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The merge against what Dovetail's generator really emits: the fragments in <c>generator-output/</c> are copies of the
///     snapshots in Joinery.Dovetail.Analyzers.Tests (sections as <c>## Path</c> headings followed by the fragment, then
///     <c>## Diagnostics</c>), covering definitions with dots in their names, recursive types, dictionaries with key patterns,
///     constructor-bound and required members.
/// </summary>
public class GeneratorOutputTests
{
    public static IEnumerable<string> Files() =>
        Directory.EnumerateFiles(Path.Combine(AppContext.BaseDirectory, "generator-output"), "*.txt").Select(Path.GetFileName).OrderBy(z => z, StringComparer.Ordinal)!;

    [Test]
    [MethodDataSource(nameof(Files))]
    public async Task EachFile_MergesIntoAWellFormedApplicationSchema_AndAPermissiveLibrarySegment(string file)
    {
        var fragments = Read(file);
        await fragments.Should().NotBeEmpty();

        var application = await SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(fragments, strict: true, includeSchemaStore: true))
        );
        var library = await SchemaAssert.ShouldBeWellFormed(ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateLibrarySchema(fragments)!));

        // Every Dovetail object that lists properties is closed, however it is reached: sections and hoisted definitions.
        foreach (var definition in SchemaAssert.DovetailDefinitions(application))
        {
            foreach (var schema in SchemaAssert.Objects(definition.Value))
            {
                if (schema["properties"] is not JsonObject properties || schema["additionalProperties"] is not null || IsSharedParent(properties)) continue;

                await schema["propertyNames"].Should().NotBeNull().Because($"{definition.Key} has an open object with properties: {schema.ToJsonString()}");
            }
        }

        await SchemaAssert.Objects(library).Should().All(z => !z.ContainsKey("propertyNames") || z["propertyNames"]!["pattern"] is JsonValue).Because("only dictionary key patterns");
        await library["properties"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(fragments.Select(z => z.Path[0]).Distinct(StringComparer.OrdinalIgnoreCase));
    }

    [Test]
    public async Task AllFilesTogether_Merge_AndTheDefinitionsStayUnique()
    {
        var fragments = Files().SelectMany(Read).ToArray();

        var document = await SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema(fragments, strict: true, includeSchemaStore: true))
        );

        var names = document["definitions"]!.AsObject().Select(z => z.Key).ToArray();
        await names.Distinct(StringComparer.OrdinalIgnoreCase).Count().Should().BeEqualTo(names.Length);
        // The recursive type (Reports_problems.txt's Tree) refers to itself under its new name.
        await document["definitions"]!.AsObject().Where(z => z.Key.StartsWith("Tree.", StringComparison.Ordinal)).Should().Contain(z => SchemaAssert.References(z.Value!).Contains("#/definitions/" + z.Key));
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

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document["definitions"]!["Shapes.Sample.IShape"]!["propertyNames"]!["patternErrorMessage"]!.GetValue<string>().Should().BeEqualTo("Unknown key in \"Sample.IShape\" (section \"Shapes\"). Known keys (case-insensitive): Sides.");
        await document["definitions"]!["Other.Sample.IShape"]!["propertyNames"]!["pattern"]!.GetValue<string>().Should().Contain("[Ff][Ii][Ll][Ll]");
        await Verify(json, "json");
    }

    [Test]
    public async Task AnObjectWithoutBindableMembers_IsClosedToo()
    {
        // An interface or abstract class with nothing to bind has no valid keys.
        var section = Section("Empty", Properties(("Marker", Uses("Sample.IMarker"))), Definitions(("Sample.IMarker", Definition(Properties()))));

        var document = await SchemaAssert.ShouldBeWellFormed(
            ConfigurationSchemaDocument.Serialize(ConfigurationSchemaDocument.CreateApplicationSchema([Parse("Empty", section)], strict: true, includeSchemaStore: false))
        );

        var pattern = new Regex(document["definitions"]!["Empty.Sample.IMarker"]!["propertyNames"]!["pattern"]!.GetValue<string>(), RegexOptions.ECMAScript);
        await pattern.IsMatch("Anything").Should().BeFalse();
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
