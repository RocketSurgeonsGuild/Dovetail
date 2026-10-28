using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
/// Covers the MSBuild switches that shape what the generator emits: <c>IsTestProject</c> / <c>DovetailHostType</c> decide
/// whether the generated Imports pins a host type, and <c>ImportDovetailAssembly</c> / <c>ExportDovetailAssembly</c>
/// decide whether Imports is generated and whether the assembly advertises its joints to referencing assemblies.
/// </summary>
public class AssemblyRoleTests() : GeneratorTest()
{
    private const string ExportedJoint = """
        using Joinery.Dovetail;
        using Joinery.Dovetail.Attributes;
        using Joinery.Dovetail.Infrastructure;

        namespace Joinery.Dovetail.Tests
        {
            [DovetailExport]
            internal class Contrib : IDovetailJoint { }
        }
        """;

    [Test]
    [Arguments(null, null, "properties ?? new Dictionary<object, object>(), categories ?? []")]
    [Arguments(null, "Undefined", "properties ?? new Dictionary<object, object>(), categories ?? []")]
    [Arguments(null, "NotAHostType", "properties ?? new Dictionary<object, object>(), categories ?? []")]
    [Arguments(null, "1", "properties ?? new Dictionary<object, object>(), categories ?? []")]
    [Arguments(null, "live", "DovetailHostType.Live, categories ?? []")]
    [Arguments(null, "UnitTest", "DovetailHostType.UnitTest, categories ?? []")]
    [Arguments("true", null, "DovetailHostType.UnitTest, categories ?? []")]
    [Arguments("true", "Undefined", "DovetailHostType.UnitTest, categories ?? []")]
    [Arguments("true", "Live", "DovetailHostType.Live, categories ?? []")]
    public async Task Imports_Pins_HostType_Only_When_The_Build_Says_So(string? isTestProject, string? hostType, string expected)
    {
        var builder = WithSharedDeps().AddSources(ExportedJoint);
        if (isTestProject is not null) builder = builder.AddGlobalOption("build_property.IsTestProject", isTestProject);
        if (hostType is not null) builder = builder.AddGlobalOption("build_property.DovetailHostType", hostType);

        var result = await builder.Build().GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, "Imported_Assembly_Dovetails.g.cs").ShouldContain(expected);
    }

    [Test]
    [Arguments(null, true)]
    [Arguments("", true)]
    [Arguments("true", true)]
    [Arguments("false", false)]
    public async Task Imports_Is_Generated_Unless_ImportDovetailAssembly_Is_False(string? importDovetailAssembly, bool generated)
    {
        var builder = WithSharedDeps().AddSources(ExportedJoint);
        if (importDovetailAssembly is not null) builder = builder.AddGlobalOption("build_property.ImportDovetailAssembly", importDovetailAssembly);

        var result = await builder.Build().GenerateAsync(TestContext.CancellationToken);

        HasGeneratedSource(result, "Imported_Assembly_Dovetails.g.cs").ShouldBe(generated);
    }

    [Test]
    [Arguments(null, true)]
    [Arguments("true", true)]
    [Arguments("false", false)]
    public async Task Exports_Are_Advertised_Unless_ExportDovetailAssembly_Is_False(string? exportDovetailAssembly, bool advertised)
    {
        var builder = WithSharedDeps().AddSources(ExportedJoint);
        if (exportDovetailAssembly is not null) builder = builder.AddGlobalOption("build_property.ExportDovetailAssembly", exportDovetailAssembly);

        var result = await builder.Build().GenerateAsync(TestContext.CancellationToken);

        // The Exports class is always emitted, so this assembly's own Imports keeps picking up its joints...
        var exports = GeneratedSource(result, "Exported_Dovetails.g.cs");
        exports.ShouldContain("class Exports");
        GeneratedSource(result, "Imported_Assembly_Dovetails.g.cs").ShouldContain("Exports.Rivet()");
        // ...but only the Dovetail.Export.* metadata makes referencing assemblies import them.
        exports.Contains("\"Dovetail.Export.ClassName\"", StringComparison.Ordinal).ShouldBe(advertised);
    }

    [Test]
    [Arguments(LanguageVersion.CSharp11, false)]
    [Arguments(LanguageVersion.CSharp12, true)]
    [Arguments(LanguageVersion.Preview, true)]
    public async Task Generates_Nothing_Below_CSharp12_And_Says_Why(LanguageVersion languageVersion, bool generated)
    {
        var result = await WithSharedDeps()
                          .WithLanguageVersion(languageVersion)
                          .AddSources(ExportedJoint)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        HasGeneratedSource(result, "Imported_Assembly_Dovetails.g.cs").ShouldBe(generated);
        HasGeneratedSource(result, "Exported_Dovetails.g.cs").ShouldBe(generated);
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.Any(z => z.Id == "DT0006").ShouldBe(!generated);
        // Either way the project still compiles: what's generated from C# 12 on is valid there, and nothing is generated before it.
        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
    }

    private static bool HasGeneratedSource(GeneratorTestResults result, string hintName) =>
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Any(z => z.FilePath.EndsWith(hintName, StringComparison.Ordinal));

    private static string GeneratedSource(GeneratorTestResults result, string hintName) =>
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Single(z => z.FilePath.EndsWith(hintName, StringComparison.Ordinal)).ToString();
}
