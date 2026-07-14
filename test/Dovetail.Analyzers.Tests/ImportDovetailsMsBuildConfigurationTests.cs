

namespace Dovetail.Analyzers.Tests;

public class ImportDovetailsMsBuildConfigurationTests() : GeneratorTest()
{
    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method()
    {
        var result = await WithSharedDeps()
                          .AddGlobalOption("build_property.DovetailImportAssembly", "true")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Not_Generate_Static_Assembly_Level_Method_By_Default()
    {
        var result = await WithSharedDeps()
                          .AddGlobalOption("build_property.DovetailImportAssembly", "false")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_Custom_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddDovetailConfiguration(importNamespace: "Test.My.Namespace", exportNamespace: "", importClassName: "MyImports")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }


    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_No_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddDovetailConfiguration(importNamespace: "", exportNamespace: "", importClassName: "MyImports")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_Custom_MethodName()
    {
        var result = await WithSharedDeps()
                          .AddDovetailConfiguration(importNamespace: "Test.My.Namespace", exportNamespace: "", importClassName: "MyImports", importMethodName: "ImportDovetails")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Use_Assembly_Configuration_If_Defined()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

"
                           )
                          .AddDovetailConfiguration(importNamespace: "Test.Other.Namespace", exportNamespace: "", importMethodName: "ImportsDovetails")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_FullName()
    {
        var result = await WithSharedDeps()
                          .AddGlobalOption("build_property.DovetailImportAssembly", "true")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Support_No_Exported_Dovetail_Assemblies()
    {
        var result = await Builder
                          .AddGlobalOption("build_property.DovetailImportAssembly", "true")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Before(Test)]
    public Task InitializeAsync()
    {
        Configure(b => b.IgnoreOutputFile("Exported_Dovetails.cs"));
        return Task.CompletedTask;
    }
}
