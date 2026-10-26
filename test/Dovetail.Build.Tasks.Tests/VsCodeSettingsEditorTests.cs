using System.Text;
using System.Text.Json;

namespace Dovetail.Build.Tasks.Tests;

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
        EditorSettingsMappings.TryCreate(root, Path.Combine(root, project), Path.Combine(root, project, "obj", "appsettings.schema.json"), out var mappings, out var error).ShouldBeTrue(error);
        return mappings!;
    }

    private static byte[] Bytes(string text, bool bom = false) => bom ? [0xEF, 0xBB, 0xBF, .. new UTF8Encoding(false).GetBytes(text)] : new UTF8Encoding(false).GetBytes(text);

    /// <summary>
    ///     Applies the editor, asserts that it updated the file, and returns the new text after checking it is valid JSONC.
    /// </summary>
    private static string Edit(string text, EditorSettingsMappings? mappings = null)
    {
        var result = VsCodeSettingsEditor.Apply(Bytes(text), mappings ?? Host);
        result.Status.ShouldBe(EditorSettingsStatus.Updated, result.Reason);
        var updated = new UTF8Encoding(false).GetString(result.Content);
        Parse(updated);
        return updated;
    }

    private static JsonDocument Parse(string text) => JsonDocument.Parse(text, new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });

    private static void ShouldBeUnchanged(string text, EditorSettingsMappings? mappings = null)
    {
        var original = Bytes(text);
        var result = VsCodeSettingsEditor.Apply(original, mappings ?? Host);
        result.Status.ShouldBe(EditorSettingsStatus.Unchanged, result.Reason);
        result.Content.ShouldBe(original);
    }

    private static void ShouldBeUnsafe(string text, string reason)
    {
        var result = VsCodeSettingsEditor.Apply(Bytes(text), Host);
        result.Status.ShouldBe(EditorSettingsStatus.Unsafe);
        result.Reason.ShouldContain(reason);
        result.Content.ShouldBeEmpty();
    }

    /// <summary>
    ///     Asserts that the document maps the host once in each of the three settings.
    /// </summary>
    private static void ShouldMap(string text, EditorSettingsMappings mappings)
    {
        using var document = Parse(text);
        var root = document.RootElement;
        root.GetProperty("json.schemas").EnumerateArray()
            .Where(z => z.ValueKind == JsonValueKind.Object)
            .Count(z => z.GetProperty("url").GetString() == mappings.SchemaUrl && z.GetProperty("fileMatch").EnumerateArray().Any(f => f.GetString() == mappings.JsonFileMatch))
            .ShouldBe(1);
        var yaml = root.GetProperty("yaml.schemas").GetProperty(mappings.SchemaUrl).EnumerateArray().Select(z => z.GetString()).ToArray();
        yaml.ShouldBe(mappings.YamlGlobs, ignoreOrder: true);
        root.GetProperty("evenBetterToml.schema.associations").GetProperty(mappings.TomlPattern).GetString().ShouldBe(mappings.SchemaUrl);
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
    public void MissingFile_IsCreatedWithTheThreeMappings()
    {
        var result = VsCodeSettingsEditor.Apply(null, Host);

        result.Status.ShouldBe(EditorSettingsStatus.Updated);
        new UTF8Encoding(false).GetString(result.Content).ShouldBe(HostEntries);
    }

    [Test]
    [Arguments("")]
    [Arguments("  \n\n")]
    [Arguments("\t")]
    public void EmptyFile_GetsTheThreeMappings(string text) => Edit(text).ShouldBe(HostEntries);

    [Test]
    public void EmptyFileWithCrLf_GetsCrLf()
    {
        var updated = Edit("\r\n");

        updated.ShouldBe(HostEntries.Replace("\n", "\r\n"));
    }

    [Test]
    public void EmptyFileWithABom_KeepsTheBom()
    {
        var result = VsCodeSettingsEditor.Apply(Bytes("", bom: true), Host);

        result.Content.Take(3).ShouldBe(new byte[] { 0xEF, 0xBB, 0xBF });
        new UTF8Encoding(false).GetString(result.Content[3..]).ShouldBe(HostEntries);
    }

    [Test]
    public void FileWithOnlyComments_KeepsThemAndAddsTheDocument()
    {
        Edit("// my settings\n").ShouldBe("// my settings\n" + HostEntries);
        Edit("/* none yet */").ShouldBe("/* none yet */\n" + HostEntries);
    }

    [Test]
    public void EmptyObject_GetsTheMappings() => Edit("{}").ShouldBe(HostEntries.TrimEnd('\n'));

    [Test]
    public void ExistingSettings_ComeFirstAndKeepEverything()
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

        updated.ShouldBe(
            """
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
            """
        );
    }

    [Test]
    public void LastSettingWithoutTrailingComma_GetsOneAndKeepsItsTrailingComment()
    {
        var updated = Edit("{\n  \"a\": 1 // keep me\n}\n");

        updated.ShouldStartWith("{\n  \"a\": 1, // keep me\n  \"json.schemas\": [");
        updated.ShouldEndWith("\n  }\n}\n");
        ShouldMap(updated, Host);
    }

    [Test]
    public void FourSpaceAndTabIndentation_IsFollowed()
    {
        var spaces = Edit("{\n    \"a\": 1\n}\n");
        spaces.ShouldContain("\n    \"json.schemas\": [\n        {\n            \"fileMatch\"");

        var tabs = Edit("{\n\t\"a\": 1\n}\n");
        tabs.ShouldContain("\n\t\"json.schemas\": [\n\t\t{\n\t\t\t\"fileMatch\"");
        tabs.ShouldNotContain("  ");
    }

    [Test]
    public void CrLfLineEndings_AreKept()
    {
        var original = "{\r\n  // comment\r\n  \"a\": 1,\r\n}\r\n";

        var updated = Edit(original);

        updated.ShouldStartWith("{\r\n  // comment\r\n  \"a\": 1,\r\n  \"json.schemas\"");
        updated.Replace("\r\n", "").ShouldNotContain("\n");
        ShouldMap(updated, Host);
    }

    [Test]
    public void ByteOrderMark_IsKept()
    {
        var result = VsCodeSettingsEditor.Apply(Bytes("{\n  \"a\": 1\n}\n", bom: true), Host);

        result.Status.ShouldBe(EditorSettingsStatus.Updated);
        result.Content.Take(3).ShouldBe(new byte[] { 0xEF, 0xBB, 0xBF });
        result.Content.Skip(3).Take(3).ShouldNotBe(new byte[] { 0xEF, 0xBB, 0xBF });
        ShouldMap(new UTF8Encoding(false).GetString(result.Content[3..]), Host);
    }

    [Test]
    public void SingleLineFile_StaysCompact()
    {
        var updated = Edit("""{ "a": 1 }""");

        updated.ShouldBe("""{ "a": 1, "json.schemas": [{ "fileMatch": ["/src/Host/appsettings*.json"], "url": "./src/Host/obj/appsettings.schema.json" }], "yaml.schemas": { "./src/Host/obj/appsettings.schema.json": ["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"] }, "evenBetterToml.schema.associations": { "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json" } }""");
    }

    [Test]
    public void ExistingJsonSchemasWithOtherEntries_GetOneMoreAndKeepThem()
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

        updated.ShouldContain("""
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
        updated.ShouldContain(
            "  \"yaml.schemas\": {\n"
          + "    \"./src/Host/obj/appsettings.schema.json\": [\"src/Host/appsettings*.yaml\", \"src/Host/appsettings*.yml\"]\n"
          + "  },\n"
        );
        updated.ShouldContain("""{ "^x$": "./x.json", "^.+/src/Host/appsettings.*\\.toml$": "./src/Host/obj/appsettings.schema.json" }""");
        ShouldMap(updated, Host);
    }

    [Test]
    public void AlreadyMapped_IsLeftByteForByte()
    {
        ShouldBeUnchanged(HostEntries);
        ShouldBeUnchanged("""
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
    public void ExistingDovetailEntries_AreCompletedInPlace()
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

        updated.ShouldBe(
            """
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
            """
        );
    }

    [Test]
    public void DovetailEntryWithoutFileMatch_GetsOne()
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

        updated.ShouldContain("""
                  "url": "./src/Host/obj/appsettings.schema.json",
                  "fileMatch": ["/src/Host/appsettings*.json"]
            """);
        ShouldMap(updated, Host);
    }

    [Test]
    public void YamlEntryAsASingleString_BecomesAnArrayWithTheStringKept()
    {
        var updated = Edit("""{ "yaml.schemas": { "./src/Host/obj/appsettings.schema.json": "src/Host/appsettings*.yaml" } }""");

        updated.ShouldContain("""["src/Host/appsettings*.yaml", "src/Host/appsettings*.yml"]""");
        ShouldMap(updated, Host);
    }

    [Test]
    public void EmptyArraysAndObjects_AreFilled()
    {
        var updated = Edit("""
            {
              "json.schemas": [],
              "yaml.schemas": {
              },
              "evenBetterToml.schema.associations": { /* none */ }
            }
            """);

        ShouldMap(updated, Host);
        updated.ShouldContain("/* none */");
    }

    [Test]
    public void AnotherHostsEntries_AreNeverTouched()
    {
        var other = Mappings("src/Other");
        var first = Edit("""
            // two hosts, one file
            {
              "editor.tabSize": 2,
            }
            """, other);

        var both = Edit(first);

        both.ShouldStartWith("// two hosts, one file\n{\n  \"editor.tabSize\": 2,\n  \"json.schemas\": [\n    {\n      \"fileMatch\": [\"/src/Other/appsettings*.json\"],\n      \"url\": \"./src/Other/obj/appsettings.schema.json\"\n    },\n");
        both.ShouldContain("    \"./src/Other/obj/appsettings.schema.json\": [\"src/Other/appsettings*.yaml\", \"src/Other/appsettings*.yml\"],\n");
        ShouldMap(both, other);
        ShouldMap(both, Host);
        using var document = Parse(both);
        document.RootElement.GetProperty("json.schemas").GetArrayLength().ShouldBe(2);
        document.RootElement.GetProperty("yaml.schemas").EnumerateObject().Count().ShouldBe(2);
        document.RootElement.GetProperty("yaml.schemas").GetProperty(OtherUrl).GetArrayLength().ShouldBe(2);
        document.RootElement.GetProperty("evenBetterToml.schema.associations").EnumerateObject().Count().ShouldBe(2);

        // Each host keeps finding only its own entries.
        ShouldBeUnchanged(both, other);
        ShouldBeUnchanged(both);
    }

    [Test]
    public void ProjectAtTheSettingsDirectory_MapsTheFilesNextToIt()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo");
        EditorSettingsMappings.TryCreate(root, root, Path.Combine(root, "obj", "appsettings.schema.json"), out var mappings, out _).ShouldBeTrue();

        mappings!.SchemaUrl.ShouldBe("./obj/appsettings.schema.json");
        mappings.JsonFileMatch.ShouldBe("/appsettings*.json");
        mappings.YamlGlobs.ShouldBe(["appsettings*.yaml", "appsettings*.yml"]);
        mappings.TomlPattern.ShouldBe("^.+/appsettings.*\\.toml$");
    }

    [Test]
    public void DotsAndMetacharactersInTheProjectPath_AreEscapedInTheTomlPattern()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo");
        EditorSettingsMappings.TryCreate(root, Path.Combine(root, "My.App+x"), Path.Combine(root, "My.App+x", "obj", "appsettings.schema.json"), out var mappings, out _).ShouldBeTrue();

        mappings!.TomlPattern.ShouldBe("^.+/My\\.App\\+x/appsettings.*\\.toml$");
    }

    [Test]
    public void PathsOutsideTheSettingsDirectory_CantBeMapped()
    {
        var root = Path.Combine(Path.GetTempPath(), "dovetail-repo", "nested");
        var outside = Path.Combine(Path.GetTempPath(), "dovetail-repo", "project");

        EditorSettingsMappings.TryCreate(root, outside, Path.Combine(outside, "obj", "appsettings.schema.json"), out var mappings, out var error).ShouldBeFalse();

        mappings.ShouldBeNull();
        error.ShouldContain("isn't below");
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
    public void UnparsableOrUnexpectedFile_IsRejectedWithoutContent(string text, string reason) => ShouldBeUnsafe(text, reason);

    [Test]
    public void NonUtf8File_IsRejected()
    {
        var utf16 = new byte[] { 0xFF, 0xFE, (byte)'{', 0, (byte)'}', 0 };
        VsCodeSettingsEditor.Apply(utf16, Host).Status.ShouldBe(EditorSettingsStatus.Unsafe);

        var invalid = new byte[] { (byte)'{', 0xC3, 0x28, (byte)'}' };
        VsCodeSettingsEditor.Apply(invalid, Host).Status.ShouldBe(EditorSettingsStatus.Unsafe);
    }

    [Test]
    public void DeeplyNestedFile_IsRejectedRatherThanOverflowing() =>
        ShouldBeUnsafe(new string('[', 5000) + new string(']', 5000), "nested too deeply");

    [Test]
    public void StringsWithEscapesAndUnicode_AreUnderstood()
    {
        var updated = Edit("""{ "a": "line\nbreak é \" } ] // not a comment", "b": -1.5e+3 }""");

        updated.ShouldStartWith("""{ "a": "line\nbreak é \" } ] // not a comment", "b": -1.5e+3, "json.schemas": """);
        ShouldMap(updated, Host);
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
    public void ApplyingTwice_ChangesNothingTheSecondTime(string text)
    {
        var first = Edit(text);

        ShouldMap(first, Host);
        ShouldBeUnchanged(first);
    }

    [Test]
    public void Snippet_IsTheDocumentThatWouldBeCreated() => Host.ToSnippet().ShouldBe(HostEntries);
}
