using System.Text.Json.Nodes;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     How the files are written: the same bytes for the same document on every machine, and untouched when nothing changed.
/// </summary>
public class SchemaFileTests
{
    [Test]
    public void Serialize_UsesTwoSpacesLineFeedsAndNoNeedlessEscapes()
    {
        var text = ConfigurationSchemaDocument.Serialize(new JsonObject { ["description"] = "Größe <b> & 'quotes' \"too\" + more", ["nested"] = new JsonObject { ["a"] = 1, }, });

        text.ShouldBe("{\n  \"description\": \"Größe <b> & 'quotes' \\\"too\\\" + more\",\n  \"nested\": {\n    \"a\": 1\n  }\n}\n");
    }

    [Test]
    public void Write_LeavesAnUnchangedFileAlone()
    {
        var path = Path.Combine(Directory.CreateTempSubdirectory("dovetail-build-tasks-").FullName, "obj", "appsettings.schema.json");
        try
        {
            ConfigurationSchemaDocument.Write(path, "{}\n").ShouldBeTrue();
            var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            File.SetLastWriteTimeUtc(path, written);

            ConfigurationSchemaDocument.Write(path, "{}\n").ShouldBeFalse();
            File.GetLastWriteTimeUtc(path).ShouldBe(written);

            ConfigurationSchemaDocument.Write(path, "{ }\n").ShouldBeTrue();
            File.ReadAllBytes(path).ShouldBe("{ }\n"u8.ToArray());
        }
        finally
        {
            Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path))!, recursive: true);
        }
    }
}
