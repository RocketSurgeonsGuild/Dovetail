using Joinery.Dovetail.Infrastructure;
using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
/// The generated export metadata has to gate a joint the same way the runtime reflection path
/// (<c>DovetailContextBuilder.AppendJoint(IDovetailJoint)</c>) does: <c>[LiveJoint]</c> and <c>[UnitTestJoint]</c> first, then
/// <c>[DovetailHostType]</c>, otherwise no restriction. The only generated-path addition is the default for joints without any
/// of those attributes exported from a test project (<c>IsTestProject</c>), which are unit-test only.
/// </summary>
public class ExportedHostTypeTests() : GeneratorTest()
{
    [Test]
    [Arguments("", "Undefined", null)]
    [Arguments("[LiveJoint]", "Live", null)]
    [Arguments("[UnitTestJoint]", "UnitTest", null)]
    [Arguments("[DovetailHostType(DovetailHostType.Live)]", "Live", null)]
    [Arguments("[DovetailHostType(DovetailHostType.UnitTest)]", "UnitTest", null)]
    [Arguments("[DovetailHostType(DovetailHostType.Undefined)]", "Undefined", null)]
    // [LiveJoint] / [UnitTestJoint] are looked at before [DovetailHostType], as on the runtime path.
    [Arguments("[LiveJoint, DovetailHostType(DovetailHostType.UnitTest)]", "Live", null)]
    [Arguments("[UnitTestJoint, DovetailHostType(DovetailHostType.Live)]", "UnitTest", null)]
    // A test project exports unit-test-only joints unless the joint says otherwise.
    [Arguments("", "UnitTest", "true")]
    [Arguments("[LiveJoint]", "Live", "true")]
    [Arguments("[UnitTestJoint]", "UnitTest", "true")]
    [Arguments("[DovetailHostType(DovetailHostType.Live)]", "Live", "true")]
    [Arguments("[DovetailHostType(DovetailHostType.Undefined)]", "Undefined", "true")]
    public async Task Exports_The_Host_Type_The_Joint_Declares(string attributes, string expected, string? isTestProject)
    {
        var result = await Generate($"[DovetailExport] {attributes} internal class Contrib : IDovetailJoint {{ }}", isTestProject);

        ExportedSource(result).ShouldContain($"new Joinery.Dovetail.Tests.Contrib(), DovetailHostType.{expected}, DovetailCategory.Application)");
    }

    [Test]
    public async Task Exports_The_Host_Type_Declared_On_A_Base_Class()
    {
        // The attributes are inherited, so the runtime path sees [LiveJoint] on the base class of an exported joint.
        var result = await Generate(InheritedLiveJoint);

        ExportedSource(result).ShouldContain("new Joinery.Dovetail.Tests.Contrib(), DovetailHostType.Live, DovetailCategory.Application)");
    }

    [Test]
    [Arguments("")]
    [Arguments("[LiveJoint]")]
    [Arguments("[UnitTestJoint]")]
    [Arguments("[DovetailHostType(DovetailHostType.Live)]")]
    [Arguments("[DovetailHostType(DovetailHostType.UnitTest)]")]
    [Arguments("[DovetailHostType(DovetailHostType.Undefined)]")]
    [Arguments("[LiveJoint, DovetailHostType(DovetailHostType.UnitTest)]")]
    [Arguments("[UnitTestJoint, DovetailHostType(DovetailHostType.Live)]")]
    public async Task Generated_Metadata_Applies_The_Same_Joints_As_The_Runtime_Scan(string attributes)
    {
        var result = await Generate($"[DovetailExport] {attributes} internal class Contrib : IDovetailJoint {{ }}");

        await AssertAppliesTheSameJointsAsTheRuntimeScan(result);
    }

    [Test]
    public async Task Generated_Metadata_Applies_The_Same_Joints_As_The_Runtime_Scan_For_An_Inherited_Host_Type()
    {
        var result = await Generate(InheritedLiveJoint);

        await AssertAppliesTheSameJointsAsTheRuntimeScan(result);
    }

    private const string InheritedLiveJoint = """
        [LiveJoint] internal abstract class Base : IDovetailJoint { }
        [DovetailExport] internal class Contrib : Base { }
        """;

    private async Task<GeneratorTestResults> Generate(string declarations, string? isTestProject = null)
    {
        var builder = Builder.AddSources(
            $$"""
              using Joinery.Dovetail;
              using Joinery.Dovetail.Attributes;
              using Joinery.Dovetail.Infrastructure;

              namespace Joinery.Dovetail.Tests
              {
                  {{declarations}}
              }
              """
        );
        if (isTestProject is not null) builder = builder.AddGlobalOption("build_property.IsTestProject", isTestProject);

        return await builder.Build().GenerateAsync(TestContext.CancellationToken);
    }

    private static string ExportedSource(GeneratorTestResults result) =>
        result
           .Results[typeof(DovetailAttributesGenerator)]
           .SyntaxTrees
           .Single(z => z.FilePath.EndsWith("Exported_Dovetails.g.cs", StringComparison.Ordinal))
           .ToString();

    /// <summary>
    /// Builds a context from the generated <c>Exports.Rivet()</c> metadata and another from the same joint through the runtime
    /// reflection path, for each host type, and expects both to apply the same joints.
    /// </summary>
    private async Task AssertAppliesTheSameJointsAsTheRuntimeScan(GeneratorTestResults result)
    {
        var assembly = result.Assembly;
        assembly.ShouldNotBeNull(string.Join(Environment.NewLine, result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error)));

        var metadata = ( (IEnumerable<IDovetailJointMetadata>)assembly.GetType("Exports")!.GetMethod("Rivet")!.Invoke(null, null)! ).ToArray();
        var joint = metadata.Single().Joint;

        foreach (var hostType in Enum.GetValues<DovetailHostType>())
        {
            var generated = DovetailContextBuilder.Create(metadata, new Dictionary<object, object>(), hostType, []);
            var runtime = DovetailContextBuilder.Create([], new Dictionary<object, object>(), hostType, []).AppendJoint(joint);

            var generatedJoints = ( await generated.CreateAsync(TestContext.CancellationToken) ).Joints.Select(z => z.GetType());
            var runtimeJoints = ( await runtime.CreateAsync(TestContext.CancellationToken) ).Joints.Select(z => z.GetType());

            generatedJoints.ShouldBe(runtimeJoints, $"in a {hostType} host");
        }
    }
}
