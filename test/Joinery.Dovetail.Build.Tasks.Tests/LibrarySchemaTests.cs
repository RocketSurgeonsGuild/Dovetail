using static Joinery.Dovetail.Build.Tasks.Tests.Fragments;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The ConfigurationSchema.json a library writes and packs: its own fragments, permissive, rooted at the configuration root.
/// </summary>
public class LibrarySchemaTests
{
    [Test]
    public async Task TheSegment_IsRootedAtTheConfigurationRoot_AndPermissive()
    {
        var json = ConfigurationSchemaDocument.Serialize(
            ConfigurationSchemaDocument.CreateLibrarySchema(
                [
                    ApplicationSchemaTests.GreetingsFragment(),
                    Parse("Shared:Http", Section("Shared:Http", Properties(("Retry", Uses("Retry"))), Definitions(("Retry", Definition(Properties(("Attempts", Integer()))))))),
                    Parse("Shared:Http", Section("Shared:Http", Properties(("Timeout", Integer())))),
                ]
            )!
        );

        var document = await SchemaAssert.ShouldBeWellFormed(json);
        await document["type"]!.GetValue<string>().Should().BeEqualTo("object");
        await document["properties"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(["Greetings", "Shared"], CollectionOrdering.Matching);
        await document["definitions"]!.AsObject().Select(z => z.Key).Should().BeEquivalentTo(["Greetings.Endpoint", "Greetings.Server", "Shared.Http.Retry"], CollectionOrdering.Matching);
        await SchemaAssert.Objects(document).Should().All(z => !z.ContainsKey("propertyNames"));
        await Verify(json, "json");
    }

    [Test]
    public async Task ALibraryWithoutSections_HasNoSegment() => await ConfigurationSchemaDocument.CreateLibrarySchema([]).Should().BeNull();
}
