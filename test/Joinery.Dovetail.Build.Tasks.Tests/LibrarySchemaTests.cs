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

        var document = SchemaAssert.ShouldBeWellFormed(json);
        document["type"]!.GetValue<string>().ShouldBe("object");
        document["properties"]!.AsObject().Select(z => z.Key).ShouldBe(["Greetings", "Shared"]);
        document["definitions"]!.AsObject().Select(z => z.Key).ShouldBe(["Greetings.Endpoint", "Greetings.Server", "Shared.Http.Retry"]);
        SchemaAssert.Objects(document).ShouldAllBe(z => !z.ContainsKey("propertyNames"));
        await Verify(json, "json");
    }

    [Test]
    public void ALibraryWithoutSections_HasNoSegment() => ConfigurationSchemaDocument.CreateLibrarySchema([]).ShouldBeNull();
}
