using System.Reflection;
using System.Text.Json.Nodes;
using Serilog.Events;
using Draft7Schema = Corvus.Json.JsonSchema.Draft7.Schema;
using ValidationContext = Corvus.Json.ValidationContext;
using ValidationLevel = Corvus.Json.ValidationLevel;

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     The configuration schema fragment Joinery.Dovetail.Serilog writes for the <c>Serilog</c> section (<c>SerilogConfigurationSchema</c>),
///     read from the assembly's metadata the way Optionary's build reads it. How Optionary merges a fragment like it is tested in Optionary;
///     real builds with it are in Joinery.Dovetail.Sdk.Tests (<c>ConfigurationSchemaSerilogTests</c>).
/// </summary>
public class SerilogConfigurationSchemaTests
{
    private const string Key = "Optionary.Schema.v1:Serilog";

    private static string Fragment() =>
        typeof(SerilogJoint).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(z => z.Key == Key).Value!;

    [Test]
    public async Task The_assembly_declares_one_fragment_for_the_Serilog_section()
    {
        var keys = typeof(SerilogJoint).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                                       .Select(z => z.Key)
                                       .Where(z => z.StartsWith("Optionary.Schema.", StringComparison.Ordinal));

        await keys.Should().BeEquivalentTo([Key], CollectionOrdering.Matching);
    }

    [Test]
    public async Task The_fragment_is_a_valid_draft_07_schema()
    {
        var result = Draft7Schema.Parse(Fragment()).Validate(ValidationContext.ValidContext, ValidationLevel.Detailed);

        var failures = result.Results.Where(z => !z.Valid).Select(z => $"{z.Location}: {z.Message}").ToArray();
        await failures.Should().BeEmpty().Because(string.Join(Environment.NewLine, failures));
    }

    [Test]
    public async Task The_levels_are_the_names_of_LogEventLevel()
    {
        // What the section is parsed to (SerilogMinimumLevelConfiguration).
        var levels = JsonNode.Parse(Fragment())!["definitions"]!["Level"]!["enum"]!.AsArray().Select(z => z!.GetValue<string>());

        await levels.Should().BeEquivalentTo(Enum.GetNames<LogEventLevel>(), CollectionOrdering.Matching);
    }
}
