using System.Text;
using System.Text.Json;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The .vscode/settings.json editor: it adds or completes Dovetail's own mappings and leaves every other byte alone.
/// </summary>
public class VsCodeSettingsEditorTests
{
    private const string OtherUrl = "./src/Other/obj/appsettings.schema.json";

    private static readonly EditorSettingsMappings Host = Mappings("src/Host");

    private static EditorSettingsMappings Mappings(string project)
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo");
        return EditorSettingsMappings.TryCreate(root, Path.Combine(root, project), Path.Combine(root, project, "obj", "appsettings.schema.json"), out var mappings, out var error)
            ? mappings!
            : throw new InvalidOperationException(error);
    }

    private static byte[] Bytes(string text, bool bom = false) => bom ? [0xEF, 0xBB, 0xBF, .. new UTF8Encoding(false).GetBytes(text)] : new UTF8Encoding(false).GetBytes(text);

    /// <summary>
    ///     Applies the editor, asserts that it updated the file, and returns the new text after checking it is valid JSONC.
    /// </summary>
    private static string Edit(string text, EditorSettingsMappings? mappings = null)
    {
        var result = VsCodeSettingsEditor.Apply(Bytes(text), mappings ?? Host);
        if (result.Status != EditorSettingsStatus.Updated) throw new InvalidOperationException($"Expected {EditorSettingsStatus.Updated} but was {result.Status}: {result.Reason}");
        var updated = new UTF8Encoding(false).GetString(result.Content);
        Parse(updated);
        return updated;
    }

    private static JsonDocument Parse(string text) => JsonDocument.Parse(text, new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });

    private static async Task ShouldBeUnchanged(string text, EditorSettingsMappings? mappings = null)
    {
        var original = Bytes(text);
        var result = VsCodeSettingsEditor.Apply(original, mappings ?? Host);
        await result.Status.Should().BeEqualTo(EditorSettingsStatus.Unchanged).Because(result.Reason);
        await result.Content.Should().BeEquivalentTo(original, CollectionOrdering.Matching);
    }

    private static async Task ShouldBeUnsafe(string text, string reason)
    {
        var result = VsCodeSettingsEditor.Apply(Bytes(text), Host);
        await result.Status.Should().BeEqualTo(EditorSettingsStatus.Unsafe);
        await result.Reason.Should().Contain(reason);
        await result.Content.Should().BeEmpty();
    }

    /// <summary>
    ///     Asserts that the document maps the host once in each of the three settings.
    /// </summary>
    private static async Task ShouldMap(string text, EditorSettingsMappings mappings)
    {
        using var document = Parse(text);
        var root = document.RootElement;
        await root.GetProperty("json.schemas").EnumerateArray()
            .Where(z => z.ValueKind == JsonValueKind.Object)
            .Count(z => z.GetProperty("url").GetString() == mappings.SchemaUrl && z.GetProperty("fileMatch").EnumerateArray().Any(f => f.GetString() == mappings.JsonFileMatch)).Should().BeEqualTo(1);
        var yaml = root.GetProperty("yaml.schemas").GetProperty(mappings.SchemaUrl).EnumerateArray().Select(z => z.GetString()).ToArray();
        await yaml.Should().BeEquivalentTo(mappings.YamlGlobs);
        await root.GetProperty("evenBetterToml.schema.associations").GetProperty(mappings.TomlPattern).GetString().Should().BeEqualTo(mappings.SchemaUrl);
    }

    private const string HostEntries = """
        {
          "json.schemas": [
            {
              "fileMatch": ["/src/Host/appsettings*.json"],
              "url": "./src/Host/obj/appsettings.schema.json"
            }
          ],
          "yaml.schemas": {
            "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"]
          },
          "evenBetterToml.schema.associations": {
            "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json"
          }
        }

        """;

    [Test]
    public async Task MissingFile_IsCreatedWithTheThreeMappings()
    {
        var result = VsCodeSettingsEditor.Apply(null, Host);

        await result.Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        await new UTF8Encoding(false).GetString(result.Content).Should().BeEqualTo(HostEntries);
    }

    [Test]
    [Arguments("")]
    [Arguments("  \n\n")]
    [Arguments("\t")]
    public async Task EmptyFile_GetsTheThreeMappings(string text) => await Edit(text).Should().BeEqualTo(HostEntries);

    [Test]
    public async Task EmptyFileWithCrLf_GetsCrLf()
    {
        var updated = Edit("\r\n");

        await updated.Should().BeEqualTo(HostEntries.Replace("\n", "\r\n"));
    }

    [Test]
    public async Task EmptyFileWithABom_KeepsTheBom()
    {
        var result = VsCodeSettingsEditor.Apply(Bytes("", bom: true), Host);

        await result.Content.Take(3).Should().BeEquivalentTo(new byte[] { 0xEF, 0xBB, 0xBF }, CollectionOrdering.Matching);
        await new UTF8Encoding(false).GetString(result.Content[3..]).Should().BeEqualTo(HostEntries);
    }

    [Test]
    public async Task FileWithOnlyComments_KeepsThemAndAddsTheDocument()
    {
        await Edit("// my settings\n").Should().BeEqualTo("// my settings\n" + HostEntries);
        await Edit("/* none yet */").Should().BeEqualTo("/* none yet */\n" + HostEntries);
    }

    [Test]
    public async Task EmptyObject_GetsTheMappings() => await Edit("{}").Should().BeEqualTo(HostEntries.TrimEnd('\n'));

    [Test]
    public async Task ExistingSettings_ComeFirstAndKeepEverything()
    {
        const string original = """
            {
              // The editor
              "editor.fontSize": 14, /* big */
              "files.exclude": {
                "**/bin": true, // not interesting
              },
              "other": [1, 2, 3],
            }
            """;

        var updated = Edit(original);

        await updated.Should().BeEqualTo("""
            {
              // The editor
              "editor.fontSize": 14, /* big */
              "files.exclude": {
                "**/bin": true, // not interesting
              },
              "other": [1, 2, 3],
              "json.schemas": [
                {
                  "fileMatch": ["/src/Host/appsettings*.json"],
                  "url": "./src/Host/obj/appsettings.schema.json"
                }
              ],
              "yaml.schemas": {
                "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"]
              },
              "evenBetterToml.schema.associations": {
                "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json"
              },
            }
            """);
    }

    [Test]
    public async Task LastSettingWithoutTrailingComma_GetsOneAndKeepsItsTrailingComment()
    {
        var updated = Edit("{\n  \"a\": 1 // keep me\n}\n");

        await updated.Should().StartWith("{\n  \"a\": 1, // keep me\n  \"json.schemas\": [");
        await updated.Should().EndWith("\n  }\n}\n");
        await ShouldMap(updated, Host);
    }

    [Test]
    public async Task FourSpaceAndTabIndentation_IsFollowed()
    {
        var spaces = Edit("{\n    \"a\": 1\n}\n");
        await spaces.Should().Contain("\n    \"json.schemas\": [\n        {\n            \"fileMatch\"");

        var tabs = Edit("{\n\t\"a\": 1\n}\n");
        await tabs.Should().Contain("\n\t\"json.schemas\": [\n\t\t{\n\t\t\t\"fileMatch\"");
        await tabs.Should().NotContain("  ");
    }

    [Test]
    public async Task CrLfLineEndings_AreKept()
    {
        var original = "{\r\n  // comment\r\n  \"a\": 1,\r\n}\r\n";

        var updated = Edit(original);

        await updated.Should().StartWith("{\r\n  // comment\r\n  \"a\": 1,\r\n  \"json.schemas\"");
        await updated.Replace("\r\n", "").Should().NotContain("\n");
        await ShouldMap(updated, Host);
    }

    [Test]
    public async Task ByteOrderMark_IsKept()
    {
        var result = VsCodeSettingsEditor.Apply(Bytes("{\n  \"a\": 1\n}\n", bom: true), Host);

        await result.Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        await result.Content.Take(3).Should().BeEquivalentTo(new byte[] { 0xEF, 0xBB, 0xBF }, CollectionOrdering.Matching);
        await result.Content.Skip(3).Take(3).SequenceEqual(new byte[] { 0xEF, 0xBB, 0xBF }).Should().BeFalse();
        await ShouldMap(new UTF8Encoding(false).GetString(result.Content[3..]), Host);
    }

    [Test]
    public async Task SingleLineFile_StaysCompact()
    {
        var updated = Edit("""{ "a": 1 }""");

        await updated.Should().BeEqualTo("""{ "a": 1, "json.schemas": [{ "fileMatch": ["/src/Host/appsettings*.json"], "url": "./src/Host/obj/appsettings.schema.json" }], "yaml.schemas": { "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"] }, "evenBetterToml.schema.associations": { "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json" } }""");
    }

    [Test]
    public async Task ExistingJsonSchemasWithOtherEntries_GetOneMoreAndKeepThem()
    {
        const string original = """
            {
              "json.schemas": [
                // somebody else's
                {
                  "fileMatch": ["/package.json"],
                  "url": "https://json.schemastore.org/package.json"
                }, // trailing
              ],
              "yaml.schemas": {},
              "evenBetterToml.schema.associations": { "^x$": "./x.json" }
            }
            """;

        var updated = Edit(original);

        await updated.Should().Contain("""
                // somebody else's
                {
                  "fileMatch": ["/package.json"],
                  "url": "https://json.schemastore.org/package.json"
                }, // trailing
                {
                  "fileMatch": ["/src/Host/appsettings*.json"],
                  "url": "./src/Host/obj/appsettings.schema.json"
                },
              ],
            """);
        await updated.Should().Contain("  \"yaml.schemas\": {\n"
          + "    \"./src/Host/obj/appsettings.schema.json\": [\"src/Host/appsettings*.yaml\", \"src/Host/appsettings*.yml\"]\n"
          + "  },\n");
        await updated.Should().Contain("""{ "^x$": "./x.json", "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json" }""");
        await ShouldMap(updated, Host);
    }

    [Test]
    public async Task AlreadyMapped_IsLeftByteForByte()
    {
        await ShouldBeUnchanged(HostEntries);
        await ShouldBeUnchanged("""
            // tweaked by hand
            {
                "json.schemas": [ { "url": "./src/Host/obj/appsettings.schema.json", "fileMatch": [ "/other/*.json", "/src/Host/appsettings*.json" ] } ],
                "yaml.schemas": { "src/Host/obj/appsettings.schema.json": [ "src/Host/appsettings*.yml", "src/Host/appsettings*.yaml", "extra/*.yaml" ], },
                "evenBetterToml.schema.associations": {
                    "^.+/src/Host/appsettings.*\\.toml$": "src/Host/obj/appsettings.schema.json", // fine
                },
            }
            """);
    }

    [Test]
    public async Task ExistingDovetailEntries_AreCompletedInPlace()
    {
        const string original = """
            {
              "json.schemas": [
                {
                  "url": "./src/Host/obj/appsettings.schema.json",
                  "fileMatch": ["/mine/*.json"] // mine too
                }
              ],
              "yaml.schemas": {
                "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "extra/*.yaml"]
              },
              "evenBetterToml.schema.associations": {
                "^.+/src/Host/appsettings.*\\.toml$": "./old/appsettings.schema.json"
              }
            }
            """;

        var updated = Edit(original);

        await updated.Should().BeEqualTo("""
            {
              "json.schemas": [
                {
                  "url": "./src/Host/obj/appsettings.schema.json",
                  "fileMatch": ["/mine/*.json", "/src/Host/appsettings*.json"] // mine too
                }
              ],
              "yaml.schemas": {
                "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "extra/*.yaml", "src/Host/appsettings*.yml"]
              },
              "evenBetterToml.schema.associations": {
                "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json"
              }
            }
            """);
    }

    [Test]
    public async Task DovetailEntryWithoutFileMatch_GetsOne()
    {
        var updated = Edit("""
            {
              "json.schemas": [
                {
                  "url": "./src/Host/obj/appsettings.schema.json"
                }
              ],
            }
            """);

        await updated.Should().Contain("""
                  "url": "./src/Host/obj/appsettings.schema.json",
                  "fileMatch": ["/src/Host/appsettings*.json"]
            """);
        await ShouldMap(updated, Host);
    }

    [Test]
    public async Task YamlEntryAsASingleString_BecomesAnArrayWithTheStringKept()
    {
        var updated = Edit("""{ "yaml.schemas": { "./src/Host/obj/appsettings.schema.json": "src/Host/appsettings*.yaml" } }""");

        await updated.Should().Contain("""["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"]""");
        await ShouldMap(updated, Host);
    }

    [Test]
    public async Task EmptyArraysAndObjects_AreFilled()
    {
        var updated = Edit("""
            {
              "json.schemas": [],
              "yaml.schemas": {
              },
              "evenBetterToml.schema.associations": { /* none */ }
            }
            """);

        await ShouldMap(updated, Host);
        await updated.Should().Contain("/* none */");
    }

    [Test]
    public async Task AnotherHostsEntries_AreNeverTouched()
    {
        var other = Mappings("src/Other");
        var first = Edit("""
            // two hosts, one file
            {
              "editor.tabSize": 2,
            }
            """, other);

        var both = Edit(first);

        await both.Should().StartWith("// two hosts, one file\n{\n  \"editor.tabSize\": 2,\n  \"json.schemas\": [\n    {\n      \"fileMatch\": [\"/src/Other/appsettings*.json\"],\n      \"url\": \"./src/Other/obj/appsettings.schema.json\"\n    },\n");
        await both.Should().Contain("    \"./src/Other/obj/appsettings.schema.json\": [\"src/Other/appsettings*.yaml\", \"src/Other/appsettings*.yml\"],\n");
        await ShouldMap(both, other);
        await ShouldMap(both, Host);
        using var document = Parse(both);
        await document.RootElement.GetProperty("json.schemas").GetArrayLength().Should().BeEqualTo(2);
        await document.RootElement.GetProperty("yaml.schemas").EnumerateObject().Count().Should().BeEqualTo(2);
        await document.RootElement.GetProperty("yaml.schemas").GetProperty(OtherUrl).GetArrayLength().Should().BeEqualTo(2);
        await document.RootElement.GetProperty("evenBetterToml.schema.associations").EnumerateObject().Count().Should().BeEqualTo(2);

        // Each host keeps finding only its own entries.
        await ShouldBeUnchanged(both, other);
        await ShouldBeUnchanged(both);
    }

    [Test]
    public async Task ProjectAtTheSettingsDirectory_MapsTheFilesNextToIt()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo");
        await EditorSettingsMappings.TryCreate(root, root, Path.Combine(root, "obj", "appsettings.schema.json"), out var mappings, out _).Should().BeTrue();

        await mappings!.SchemaUrl.Should().BeEqualTo("./obj/appsettings.schema.json");
        await mappings.JsonFileMatch.Should().BeEqualTo("/appsettings*.json");
        await mappings.YamlGlobs.Should().BeEquivalentTo(["appsettings*.yaml", "appsettings*.yml"], CollectionOrdering.Matching);
        await mappings.TomlPattern.Should().BeEqualTo("^.+/appsettings.*\\.toml$");
    }

    [Test]
    public async Task DotsAndMetacharactersInTheProjectPath_AreEscapedInTheTomlPattern()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo");
        await EditorSettingsMappings.TryCreate(root, Path.Combine(root, "My.App+x"), Path.Combine(root, "My.App+x", "obj", "appsettings.schema.json"), out var mappings, out _).Should().BeTrue();

        await mappings!.TomlPattern.Should().BeEqualTo("^.+/My\\.App\\+x/appsettings.*\\.toml$");
    }

    [Test]
    public async Task PathsOutsideTheSettingsDirectory_CantBeMapped()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo", "nested");
        var outside = Path.Combine(Path.GetTempPath(), "dovetail-repo", "project");

        await EditorSettingsMappings.TryCreate(root, outside, Path.Combine(outside, "obj", "appsettings.schema.json"), out var mappings, out var error).Should().BeFalse();

        await mappings.Should().BeNull();
        await error.Should().Contain("isn't below");
    }

    [Test]
    [Arguments("{ \"a\": ", "end of text")]
    [Arguments("{ 'a': 1 }", "property name")]
    [Arguments("{ a: 1 }", "property name")]
    [Arguments("{ \"a\": 1,, }", "property name")]
    [Arguments("{ \"a\": 1 } x", "after the root")]
    [Arguments("{ \"a\": 1 /* open }", "block comment")]
    [Arguments("{ \"a\": tru }", "invalid value")]
    [Arguments("[1, 2]", "root isn't an object")]
    [Arguments("42", "root isn't an object")]
    [Arguments("{ \"json.schemas\": {} }", "isn't an array")]
    [Arguments("{ \"json.schemas\": null }", "isn't an array")]
    [Arguments("{ \"yaml.schemas\": [] }", "isn't an object")]
    [Arguments("{ \"evenBetterToml.schema.associations\": \"x\" }", "isn't an object")]
    [Arguments("{ \"json.schemas\": [], \"json.schemas\": [] }", "more than once")]
    [Arguments("{ \"yaml.schemas\": { \"./src/Host/obj/appsettings.schema.json\": 5 } }", "unexpected value")]
    [Arguments("{ \"evenBetterToml.schema.associations\": { \"^.+/src/Host/appsettings.*\\\\.toml$\": [] } }", "unexpected value")]
    [Arguments("{ \"json.schemas\": [ { \"url\": \"./src/Host/obj/appsettings.schema.json\", \"fileMatch\": \"/x\" } ] }", "unexpected \"fileMatch\"")]
    public async Task UnparsableOrUnexpectedFile_IsRejectedWithoutContent(string text, string reason) => await ShouldBeUnsafe(text, reason);

    [Test]
    public async Task NonUtf8File_IsRejected()
    {
        var utf16 = new byte[] { 0xFF, 0xFE, (byte)'{', 0, (byte)'}', 0 };
        await VsCodeSettingsEditor.Apply(utf16, Host).Status.Should().BeEqualTo(EditorSettingsStatus.Unsafe);

        var invalid = new byte[] { (byte)'{', 0xC3, 0x28, (byte)'}' };
        await VsCodeSettingsEditor.Apply(invalid, Host).Status.Should().BeEqualTo(EditorSettingsStatus.Unsafe);
    }

    [Test]
    public async Task DeeplyNestedFile_IsRejectedRatherThanOverflowing() =>
        await ShouldBeUnsafe(new string('[', 5000) + new string(']', 5000), "nested too deeply");

    [Test]
    public async Task StringsWithEscapesAndUnicode_AreUnderstood()
    {
        var updated = Edit("""{ "a": "line\nbreak é \" } ] // not a comment", "b": -1.5e+3 }""");

        await updated.Should().StartWith("""{ "a": "line\nbreak é \" } ] // not a comment", "b": -1.5e+3, "json.schemas": """);
        await ShouldMap(updated, Host);
    }

    [Test]
    [Arguments("")]
    [Arguments("{}")]
    [Arguments("{ \"a\": 1 }")]
    [Arguments("{\n  \"a\": 1\n}")]
    [Arguments("{\n  \"a\": 1, // x\n}\n")]
    [Arguments("{\r\n\t\"a\": [1, 2,],\r\n\t\"yaml.schemas\": {\r\n\t},\r\n}\r\n")]
    [Arguments("{ \"json.schemas\": [ ], \"yaml.schemas\": { \"./src/Host/obj/appsettings.schema.json\": \"src/Host/appsettings*.yml\" } }")]
    [Arguments("// c\n{\n  /* c */ \"json.schemas\": [\n    1,\n    \"two\"\n  ]\n}\n")]
    public async Task ApplyingTwice_ChangesNothingTheSecondTime(string text)
    {
        var first = Edit(text);

        await ShouldMap(first, Host);
        await ShouldBeUnchanged(first);
    }

    [Test]
    public async Task Snippet_IsTheDocumentThatWouldBeCreated() => await Host.ToSnippet().Should().BeEqualTo(HostEntries);
}
