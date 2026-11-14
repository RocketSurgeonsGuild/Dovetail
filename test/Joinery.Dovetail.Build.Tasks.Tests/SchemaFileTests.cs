using System.Text.Json.Nodes;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     How the files are written: the same bytes for the same document on every machine, and untouched when nothing changed.
/// </summary>
public class SchemaFileTests
{
    [Test]
    public async Task Serialize_UsesTwoSpacesLineFeedsAndNoNeedlessEscapes()
    {
        var text = ConfigurationSchemaDocument.Serialize(new JsonObject { ["description"] = "Größe <b> & 'quotes' \"too\" + more", ["nested"] = new JsonObject { ["a"] = 1, }, });

        await text.Should().BeEqualTo("{\n  \"description\": \"Größe <b> & 'quotes' \\\"too\\\" + more\",\n  \"nested\": {\n    \"a\": 1\n  }\n}\n");
    }

    [Test]
    public async Task Write_LeavesAnUnchangedFileAlone()
    {
        var path = Path.Combine(Directory.CreateTempSubdirectory("dovetail-build-tasks-").FullName, "obj", "appsettings.schema.json");
        try
        {
            await ConfigurationSchemaDocument.Write(path, "{}\n").Should().BeTrue();
            var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            File.SetLastWriteTimeUtc(path, written);

            await ConfigurationSchemaDocument.Write(path, "{}\n").Should().BeFalse();
            await File.GetLastWriteTimeUtc(path).Should().BeEqualTo(written);

            await ConfigurationSchemaDocument.Write(path, "{ }\n").Should().BeTrue();
            await File.ReadAllBytes(path).Should().BeEquivalentTo("{ }\n"u8.ToArray(), CollectionOrdering.Matching);
        }
        finally
        {
            Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path))!, recursive: true);
        }
    }
}
