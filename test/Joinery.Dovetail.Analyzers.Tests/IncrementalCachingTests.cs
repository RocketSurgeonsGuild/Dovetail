using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The generator's pipeline holds equatable models rather than symbols or the compilation, so an edit that changes neither a
///     joint nor the references leaves the exports and imports cached, and neither file is generated again.
/// </summary>
public class IncrementalCachingTests() : GeneratorTest()
{
    private const string Joint = """
        using Joinery.Dovetail;
        using Joinery.Dovetail.Attributes;

        namespace Sample;

        [DovetailExport, DependsOnJoint(typeof(Other))]
        internal class Contrib : IDovetailJoint
        {
            public Contrib(System.IServiceProvider services, int retries = 3) { }
        }

        [DovetailExport]
        internal class Other : IDovetailJoint { }
        """;

    [Test]
    public async Task Keeps_the_generated_files_cached_when_unrelated_code_changes()
    {
        var steps = await RunTwice("namespace Sample; public class Unrelated { }");

        await steps.Select(z => z.Reason).Should().All(z => z is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged);
    }

    [Test]
    public async Task Keeps_the_references_unchanged_when_unrelated_code_changes()
    {
        var run = await RunTwiceWithResult(AddSource("namespace Sample; public class Unrelated { }"));

        await run.TrackedSteps["dovetail:referenced_exports"]
                 .SelectMany(z => z.Outputs)
                 .Select(z => z.Reason)
                 .Should()
                 .All(z => z is IncrementalStepRunReason.Unchanged or IncrementalStepRunReason.Cached);
    }

    [Test]
    public async Task Keeps_the_generated_files_cached_when_the_file_of_a_joint_changes_but_its_joints_do_not()
    {
        // The edit moves every joint in the file. The exports hold no locations, so moving a joint changes nothing in them.
        var steps = await RunTwice(
            (compilation, parseOptions) =>
            {
                var tree = compilation.SyntaxTrees.Single(z => z.ToString().Contains("class Contrib", StringComparison.Ordinal));
                return compilation.ReplaceSyntaxTree(tree, CSharpSyntaxTree.ParseText("// moved\r\n" + tree, parseOptions, tree.FilePath));
            }
        );

        await steps.Select(z => z.Reason).Should().All(z => z is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged);
    }

    [Test]
    public async Task Generates_the_exports_again_when_a_joint_is_added()
    {
        var steps = await RunTwice("namespace Sample; [Joinery.Dovetail.Attributes.DovetailExport] internal class Added : Joinery.Dovetail.IDovetailJoint { }");

        await steps.Select(z => z.Reason).Should().Contain(IncrementalStepRunReason.Modified);
    }

    private Task<IReadOnlyList<(object Value, IncrementalStepRunReason Reason)>> RunTwice(string addedSource) => RunTwice(AddSource(addedSource));

    private static Func<Compilation, CSharpParseOptions, Compilation> AddSource(string source) =>
        (compilation, parseOptions) => compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(source, parseOptions));

    /// <summary>Runs the generator, then again on the compilation <paramref name="edit" /> makes, and returns the second run's output steps.</summary>
    private async Task<IReadOnlyList<(object Value, IncrementalStepRunReason Reason)>> RunTwice(Func<Compilation, CSharpParseOptions, Compilation> edit) =>
        ( await RunTwiceWithResult(edit) )
       .TrackedOutputSteps.SelectMany(z => z.Value)
       .SelectMany(z => z.Outputs)
       .ToList();

    /// <summary>Runs the generator, then again on the compilation <paramref name="edit" /> makes, and returns the second run.</summary>
    private async Task<GeneratorRunResult> RunTwiceWithResult(Func<Compilation, CSharpParseOptions, Compilation> edit)
    {
        var result = await WithSharedDeps().AddSources(Joint).Build().GenerateAsync(TestContext.CancellationToken);

        var compilation = result.InputCompilation;
        var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new DovetailAttributesGenerator().AsSourceGenerator()],
            parseOptions: parseOptions,
            driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
        );
        driver = driver.RunGenerators(compilation, TestContext.CancellationToken);
        driver = driver.RunGenerators(edit(compilation, parseOptions), TestContext.CancellationToken);

        return driver.GetRunResult().Results.Single();
    }
}
