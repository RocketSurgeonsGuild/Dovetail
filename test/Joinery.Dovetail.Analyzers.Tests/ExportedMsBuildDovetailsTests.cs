

namespace Joinery.Dovetail.Analyzers.Tests;

public class ExportedMsBuildDovetailsTests() : GeneratorTest()
{
    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_Custom_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Tests;

namespace Joinery.Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .AddDovetailConfiguration(importNamespace: "ExportedMsBuildDovetails", exportNamespace: "Source.Space", exportClassName: "SourceClass")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_No_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Tests;

namespace Joinery.Dovetail.Tests
{
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .AddGlobalOption("build_property.DovetailExportNamespace", "")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }


    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_Custom_MethodName()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Tests;

namespace Joinery.Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .AddGlobalOption("build_property.DovetailExportMethodName", "SourceMethod")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Before(Test)]
    public Task InitializeAsync()
    {

        Configure(
            b => b
                .IgnoreOutputFile("Imported_Assembly_Dovetails.cs")
                .IgnoreOutputFile("Compiled_AssemblyProvider.cs")
        );
        return Task.CompletedTask;
    }
}
