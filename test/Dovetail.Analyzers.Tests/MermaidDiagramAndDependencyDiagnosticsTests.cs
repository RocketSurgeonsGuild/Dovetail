using Microsoft.CodeAnalysis;

namespace Dovetail.Analyzers.Tests;

public class MermaidDiagramAndDependencyDiagnosticsTests() : GeneratorTest()
{
    [Test]
    public async Task Should_Include_Local_Joint_Dependency_Edge_In_Mermaid_Diagram()
    {
        var result = await Builder
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Tests
{
    [DovetailExport]
    [DependsOnJoint<B>]
    internal class A : IDovetailJoint { }

    [DovetailExport]
    internal class B : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Omit_Mermaid_Diagram_When_Disabled()
    {
        var result = await Builder
                          .AddGlobalOption("build_property.DovetailGenerateMermaidDiagram", "false")
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Tests
{
    [DovetailExport]
    [DependsOnJoint<B>]
    internal class A : IDovetailJoint { }

    [DovetailExport]
    internal class B : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Report_Diagnostic_For_Malformed_Dependency_Attribute_Instead_Of_Silently_Dropping_It()
    {
        var result = await Builder
                          .WithDiagnosticSeverity(DiagnosticSeverity.Warning)
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Tests
{
    [DovetailExport]
#pragma warning disable CS7036
    [DependsOnJoint]
#pragma warning restore CS7036
    internal class A : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Report_Diagnostic_For_Circular_Joint_Dependency()
    {
        var result = await Builder
                          .WithDiagnosticSeverity(DiagnosticSeverity.Warning)
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Tests
{
    [DovetailExport]
    [DependsOnJoint<B>]
    internal class A : IDovetailJoint { }

    [DovetailExport]
    [DependsOnJoint<A>]
    internal class B : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Before(Test)]
    public Task InitializeAsync()
    {
        Configure(b => b.IgnoreOutputFile("Exported_Dovetails.cs").IgnoreOutputFile("Imported_Assembly_Dovetails.cs"));
        return Task.CompletedTask;
    }
}
