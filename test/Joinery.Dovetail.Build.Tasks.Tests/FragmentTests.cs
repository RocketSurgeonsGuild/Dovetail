using System.Reflection;
using System.Reflection.Emit;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     Reading fragments: from assembly metadata, without loading the assembly, and skipping (with a message, not an error)
///     everything this version can't use.
/// </summary>
public class FragmentTests
{
    [Test]
    [Arguments("Dovetail.ConfigurationSchema.v2:Greetings", "{}", "unknown version")]
    [Arguments("Dovetail.ConfigurationSchema.v1", "{}", "unknown version")]
    [Arguments("Dovetail.ConfigurationSchema.v1:", "{}", "empty")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Shared::Http", "{}", "empty segment")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", null, "missing")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", "{", "not valid JSON")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","type":"string"}""", "not valid JSON")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", "[1]", "not a JSON object")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", "\"object\"", "not a JSON object")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"allOf":[{"$ref":"#/definitions/Missing"}]}""", "doesn't point at one of its definitions")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"properties":{"A":{"$ref":"#/properties/B"}}}""", "doesn't point at one of its definitions")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"definitions":{"Not valid":{}}}""", "definition name")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"definitions":[]}""", "definitions are not a JSON object")]
    [Arguments("Dovetail.ConfigurationSchema.v1:Greetings", """{"definitions":{"Retry":1}}""", "is not a schema")]
    public async Task UnusableEntries_AreRejectedWithAReason(string key, string? value, string reason)
    {
        await ConfigurationSchemaFragment.TryParse(key, value, "Library", out var problem).Should().BeNull();

        await ( await problem.Should().NotBeNull() ).Should().Contain(reason);
    }

    [Test]
    public async Task AFragment_KnowsItsSectionPathAndSource()
    {
        var fragment = ConfigurationSchemaFragment.TryParse(
            "Dovetail.ConfigurationSchema.v1:Shared:Http",
            """{"type":"object","properties":{"Retry":{"type":"object","allOf":[{"$ref":"#/definitions/Retry"}]}},"definitions":{"Retry":{"properties":{}}}}""",
            "Library",
            out var problem
        );

        await problem.Should().BeNull();
        await fragment.Should().NotBeNull();
        await fragment.Path.Should().BeEquivalentTo(["Shared", "Http"], CollectionOrdering.Matching);
        await fragment.Source.Should().BeEqualTo("Library");
    }

    [Test]
    public async Task BooleanSchemas_AreFragmentsToo() =>
        await ConfigurationSchemaFragment.TryParse("Dovetail.ConfigurationSchema.v1:Anything", "true", "Library", out _).Should().NotBeNull();

    [Test]
    public async Task Fragments_AreReadFromAssemblyMetadata_AndUnusableOnesSkippedWithAMessage()
    {
        using var directory = new TemporaryDirectory();
        var assembly = directory.Assembly(
            "Lib1",
            ("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","properties":{"Message":{"type":"string"}}}"""),
            ("Dovetail.ConfigurationSchema.v1:Shared:Http", """{"type":"object","properties":{"Timeout":{"type":"integer"}}}"""),
            ("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","properties":{"Count":{"type":"integer"}}}"""),
            ("Dovetail.ConfigurationSchema.v2:Future", "{}"),
            ("Dovetail.ConfigurationSchema.v1:Broken", "{"),
            ("Some.Other.Key", "ignored")
        );
        var messages = new List<string>();

        var fragments = ConfigurationSchemaFragments.Read([assembly, assembly], messages.Add);

        await fragments.Select(z => string.Join(":", z.Path)).Should().BeEquivalentTo(["Greetings", "Shared:Http", "Greetings"], CollectionOrdering.Matching);
        await fragments.Should().All(z => z.Source == "Lib1");
        await messages.Count(z => z.Contains("skipped", StringComparison.Ordinal)).Should().BeEqualTo(2);
        await messages.Should().Contain(z => z.Contains("Dovetail.ConfigurationSchema.v2:Future", StringComparison.Ordinal) && z.Contains("unknown version", StringComparison.Ordinal));
        await messages.Should().Contain(z => z.Contains("Dovetail.ConfigurationSchema.v1:Broken", StringComparison.Ordinal) && z.Contains("not valid JSON", StringComparison.Ordinal));
        await messages.Should().All(z => !z.Contains("Some.Other.Key", StringComparison.Ordinal));
    }

    [Test]
    public async Task FilesThatArentAssemblies_AreSkippedWithAMessage()
    {
        using var directory = new TemporaryDirectory();
        var notAnAssembly = Path.Combine(directory.Path, "NotAnAssembly.dll");
        File.WriteAllText(notAnAssembly, "not a PE file");
        var missing = Path.Combine(directory.Path, "Missing.dll");
        var messages = new List<string>();

        var fragments = ConfigurationSchemaFragments.Read([notAnAssembly, missing, directory.Assembly("Lib1")], messages.Add);

        await fragments.Should().BeEmpty();
        await messages.Count.Should().BeEqualTo(2);
        await messages.Should().All(z => z.StartsWith("Dovetail: skipped ", StringComparison.Ordinal));
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        public string Path { get; } = Directory.CreateTempSubdirectory("dovetail-build-tasks-").FullName;

        /// <summary>
        ///     Saves an assembly carrying <c>[assembly: AssemblyMetadata(key, value)]</c> for each entry.
        /// </summary>
        public string Assembly(string name, params (string Key, string? Value)[] metadata)
        {
            var builder = new PersistedAssemblyBuilder(new(name), typeof(object).Assembly);
            builder.DefineDynamicModule(name);
            var constructor = typeof(AssemblyMetadataAttribute).GetConstructor([typeof(string), typeof(string)])!;
            foreach (var (key, value) in metadata)
            {
                builder.SetCustomAttribute(new(constructor, [key, value]));
            }

            var path = System.IO.Path.Combine(Path, name + ".dll");
            builder.Save(path);
            return path;
        }

        public void Dispose() => Directory.Delete(Path, recursive: true);
    }
}
