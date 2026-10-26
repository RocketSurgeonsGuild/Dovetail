using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Dovetail.Build.Tasks.Tests;
using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Dovetail.Serilog ships the schema of the <c>Serilog</c> section as an assembly metadata fragment
/// (<c>SerilogConfigurationSchema.cs</c>). These build a real application that references the packed library and check that
/// <c>Dovetail.ConfigurationSchema.targets</c> merges the fragment into its <c>obj/appsettings.schema.json</c> during a normal
/// <c>dotnet build</c>, next to the SchemaStore schema every host schema embeds. Dovetail.Build.Tasks.Tests covers the fragment
/// against the build task in-process; this is the same through the package and MSBuild.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaSerilogTests(string targetFramework)
{
    private const string HostSchema = "host/obj/appsettings.schema.json";

    private const string SchemaStoreDefinition = "SchemaStoreAppSettings";

    private static readonly string[] ModelledKeys = ["MinimumLevel", "Enrich", "Filter", "Properties"];

    private static readonly string[] PermittedKeys = ["Using", "WriteTo", "AuditTo", "Destructure", "LevelSwitches", "FilterSwitches"];

    private static readonly Regex NamedGroup = new(@"\(\?<[A-Za-z]+>", RegexOptions.CultureInvariant);

    /// <summary>
    /// The way a Serilog section is usually written: the detailed MinimumLevel, sinks with arguments, enrichers and properties.
    /// </summary>
    private const string RealisticConfiguration = """
        {
          "Serilog": {
            "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
            "MinimumLevel": { "Default": "Information", "Override": { "Microsoft": "Warning", "System": "Warning" } },
            "WriteTo": [ { "Name": "Console" }, { "Name": "File", "Args": { "path": "Logs/log.txt", "rollingInterval": "Day" } } ],
            "Enrich": [ "FromLogContext", "WithMachineName", { "Name": "WithProperty", "Args": { "name": "Version", "value": "1.2.3" } } ],
            "Properties": { "Application": "Sample" }
          }
        }
        """;

    /// <summary>
    /// Each of these breaks one rule of the fragment (and of SchemaStore's schema, which the host schema also applies).
    /// </summary>
    private static readonly (string Name, string Configuration)[] InvalidConfigurations =
    [
        ("a numeric MinimumLevel", """{ "Serilog": { "MinimumLevel": 2 } }"""),
        ("a numeric default level", """{ "Serilog": { "MinimumLevel": { "Default": 2 } } }"""),
        ("an unknown level", """{ "Serilog": { "MinimumLevel": "Loud" } }"""),
        ("a numeric override", """{ "Serilog": { "MinimumLevel": { "Override": { "Microsoft": 3 } } } }"""),
        ("an enricher name with a dot", """{ "Serilog": { "Enrich": [ "Some.Dotted.Name" ] } }"""),
        ("a property that is an object", """{ "Serilog": { "Properties": { "Application": { "Name": "Sample" } } } }"""),
        ("a misspelled section key", """{ "Serilog": { "MinimumLevl": "Debug" } }"""),
    ];

    private SdkTestProject CreateHost(bool referenceSerilog)
    {
        // Packed from the repo's sources and restored from the scaffold's feed (see SdkTestProject / Config.NugetArtifactsDirectory).
        // Dovetail.Serilog brings Dovetail, and with it the build assets that merge the schema; the control host gets the same
        // through Dovetail.Hosting, so only the Serilog library differs between them.
        var package = referenceSerilog ? "Dovetail.Serilog" : "Dovetail.Hosting";
        var host = ProjectCreator
                  .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                  .ItemInclude("PackageReference", include: package, metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion(package) });

        var project = new SdkTestProject();
        project
           .AddProject("host/host.csproj", host)
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", ConfigurationSchemaTests.AppFragment);
        return project;
    }

    private async Task<SdkTestProject> BuildHost(bool referenceSerilog)
    {
        var project = CreateHost(referenceSerilog);
        await project.BuildProject("host");
        return project;
    }

    private static JsonObject ReadSchema(SdkTestProject project) => JsonNode.Parse(project.ReadFile(HostSchema))!.AsObject();

    /// <summary>
    /// The Dovetail part of an application schema: the second <c>allOf</c> member of the root (the first references SchemaStore's).
    /// </summary>
    private static JsonObject DovetailPart(JsonObject schema) => schema["allOf"]!.AsArray()[1]!.AsObject();

    /// <summary>
    /// Every key the schema at <paramref name="node" /> declares: its <c>properties</c>, following <c>$ref</c> and <c>allOf</c>.
    /// </summary>
    private static IReadOnlyList<string> Keys(JsonObject schema, JsonObject node) =>
    [
        .. ( node["properties"] as JsonObject )?.Select(z => z.Key) ?? [],
        .. node["$ref"] is JsonValue reference ? Keys(schema, SchemaInstanceValidator.Resolve(schema, reference.GetValue<string>())!.AsObject()) : [],
        .. ( node["allOf"] as JsonArray )?.SelectMany(z => Keys(schema, z!.AsObject())) ?? [],
    ];

    private static JsonObject SerilogSection(JsonObject schema) => DovetailPart(schema)["properties"]!["Serilog"]!.AsObject();

    /// <summary>
    /// The schema SchemaStore's part of the host schema wouldn't decide: the Dovetail part alone, so that a verdict can only come
    /// from the merged fragment.
    /// </summary>
    private static SchemaInstanceValidator FragmentOnly(JsonObject schema)
    {
        var copy = JsonNode.Parse(schema.ToJsonString())!.AsObject();
        copy["definitions"]!.AsObject().Remove(SchemaStoreDefinition);
        copy["allOf"] = new JsonArray(DovetailPart(copy).DeepClone());
        return new(copy);
    }

    /// <summary>
    /// The host schema as it is: SchemaStore's and the fragments' rules both apply. SchemaStore's patterns use .NET-style named
    /// groups, which ECMA-262 (what the validator speaks) lacks.
    /// </summary>
    private static SchemaInstanceValidator WholeSchema(JsonObject schema) =>
        new(JsonNode.Parse(NamedGroup.Replace(schema.ToJsonString(new() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, }), "(?:"))!);

    [Test]
    public async Task HostReferencingSerilog_BuildsWithoutWarnings_AndMergesTheSerilogSection()
    {
        using var project = CreateHost(referenceSerilog: true);

        await project.VerifyBuildWarnings("host");

        var schema = ReadSchema(project);
        var section = SerilogSection(schema);
        section["$ref"].ShouldNotBeNull();
        var keys = Keys(schema, section);
        foreach (var key in ModelledKeys.Concat(PermittedKeys))
        {
            keys.ShouldContain(key);
        }

        // The fragment's definitions are in the host schema, SchemaStore's next to them, and the host's own section is still there.
        schema["definitions"]!.AsObject().Select(z => z.Key).ShouldContain(SchemaStoreDefinition);
        schema["definitions"]!.AsObject().Select(z => z.Key).ShouldContain("Serilog.MinimumLevelObject");
        DovetailPart(schema)["properties"]!.AsObject().Select(z => z.Key).ShouldBe(["App", "Serilog"], ignoreOrder: true);
        project.ReadFile($"host/bin/Debug/{targetFramework}/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    [Test]
    public async Task TheHostSchemaWithSerilog_IsWellFormed()
    {
        using var project = await BuildHost(referenceSerilog: true);

        var schema = ReadSchema(project);

        schema["$schema"]!.GetValue<string>().ShouldBe("http://json-schema.org/draft-07/schema#");
        // Every reference is local and resolves (a dangling one in the merge would silently accept anything).
        var references = Objects(schema).Select(z => z["$ref"]).OfType<JsonValue>().Select(z => z.GetValue<string>()).ToArray();
        references.ShouldNotBeEmpty();
        foreach (var reference in references)
        {
            reference.ShouldStartWith("#/", customMessage: $"{reference} is not a local reference");
            SchemaInstanceValidator.Resolve(schema, reference).ShouldNotBeNull($"{reference} doesn't resolve");
        }

        // Dovetail's patterns (SchemaStore's are checked by its own tests) are ECMA-262.
        var dovetail = new JsonObject { ["part"] = DovetailPart(schema).DeepClone() };
        foreach (var definition in schema["definitions"]!.AsObject().Where(z => z.Key != SchemaStoreDefinition))
        {
            dovetail[definition.Key] = definition.Value!.DeepClone();
        }

        var patterns = Objects(dovetail).SelectMany(Patterns).ToArray();
        patterns.ShouldNotBeEmpty();
        foreach (var pattern in patterns)
        {
            Should.NotThrow(() => new Regex(pattern, RegexOptions.ECMAScript), $"{pattern} isn't an ECMA-262 pattern");
        }
    }

    [Test]
    public async Task TheHostSchemaWithSerilog_AcceptsARealisticConfiguration_AndRejectsBadOnes()
    {
        using var project = await BuildHost(referenceSerilog: true);

        var schema = ReadSchema(project);
        var whole = WholeSchema(schema);
        var fragment = FragmentOnly(schema);

        whole.Validate(RealisticConfiguration).ShouldBeEmpty();
        fragment.Validate(RealisticConfiguration).ShouldBeEmpty();
        foreach (var (name, configuration) in InvalidConfigurations)
        {
            whole.IsValid(configuration).ShouldBeFalse($"{name}, in the host schema");
            // Without SchemaStore's part: the merged fragment is what rejects it.
            fragment.IsValid(configuration).ShouldBeFalse($"{name}, in the merged fragment");
        }
    }

    [Test]
    public async Task HostNotReferencingSerilog_HasNoSerilogSection()
    {
        using var project = await BuildHost(referenceSerilog: false);

        var schema = ReadSchema(project);

        // The host's own section is there, so the schema is built from the fragments; Serilog's isn't one of them. (SchemaStore's
        // part has a Serilog definition of its own, which is not what is looked at.)
        var dovetail = DovetailPart(schema)["properties"]!.AsObject().Select(z => z.Key).ToArray();
        dovetail.ShouldBe(["App"]);
        schema["definitions"]!.AsObject().Select(z => z.Key).Where(z => z.StartsWith("Serilog", StringComparison.OrdinalIgnoreCase)).ShouldBeEmpty();
        project.ReadFile(HostSchema).ShouldNotContain("Dovetail.Serilog");
        // Without the library's fragment, a configuration only SchemaStore's rules cover is left to them.
        FragmentOnly(schema).IsValid("""{ "Serilog": { "MinimumLevel": 2 } }""").ShouldBeTrue();
    }

    private static IEnumerable<JsonObject> Objects(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject value:
                yield return value;
                foreach (var child in value.SelectMany(z => Objects(z.Value)))
                {
                    yield return child;
                }

                break;
            case JsonArray array:
                foreach (var child in array.SelectMany(Objects))
                {
                    yield return child;
                }

                break;
            default:
                break;
        }
    }

    private static IEnumerable<string> Patterns(JsonObject schema) =>
        ( schema["patternProperties"] is JsonObject patternProperties ? patternProperties.Select(z => z.Key) : [] )
       .Concat(schema["pattern"] is JsonValue pattern && pattern.TryGetValue<string>(out var text) ? [text] : [])
       .Concat(schema["propertyNames"] is JsonObject { } names && names["pattern"] is JsonValue namesPattern ? [namesPattern.GetValue<string>()] : []);
}
