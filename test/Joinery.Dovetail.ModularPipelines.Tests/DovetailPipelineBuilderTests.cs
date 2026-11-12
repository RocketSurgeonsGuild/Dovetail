using FakeItEasy;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.ModularPipelines.Tests;

public class DovetailPipelineBuilderTests
{
    [Test]
    public async Task ConfigureDovetail_BuildsThePipeline_WithTheModulesAndServicesOfTheExportedJoints()
    {
        await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail();

        var plan = await pipeline.PlanAsync();
        plan.Modules.OfType<RecordedModule>().Count().ShouldBe(1);
        pipeline.Services.GetService<PipelineRecorder>().ShouldNotBeNull();
    }

    [Test]
    public async Task ConfigureDovetail_RunsTheModulesTheJointsAdded()
    {
        // The test runner has no terminal, whose zero width Spectre.Console refuses when the pipeline writes its output.
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureConsole(z => z with { Width = 120, ShowProgress = false, PrintLogo = false, PrintResults = false, })
                                        .ConfigureDovetail();

        await pipeline.RunAsync();

        pipeline.Services.GetRequiredService<PipelineRecorder>().Ran.ShouldBe([nameof(RecordedModule)]);
    }

    [Test]
    public async Task ConfigureDovetail_HandsThePipelineBuilderJointsTheBuilder()
    {
        var joint = A.Fake<PipelineBuilderJointDelegate>();
        var builder = Pipeline.CreateBuilder();

        await using var pipeline = await builder.ConfigureDovetail(z => z.ConfigurePipelineBuilder(joint));

        A.CallTo(() => joint.Invoke(A<IDovetailContext>._, builder)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task ConfigureDovetail_AppliesPipelineBuilderJointsInPriorityOrder_SyncAndAsync()
    {
        var order = new List<string>();
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z
                                                 .ConfigurePipelineBuilder(
                                                      async (_, builder, cancellationToken) =>
                                                      {
                                                          await Task.Yield();
                                                          order.Add("async");
                                                          builder.AddModule<DelegateModule>();
                                                      },
                                                      priority: 10
                                                  )
                                                 .ConfigurePipelineBuilder((_, _) => order.Add("sync"), priority: -10)
                                         );

        order.ShouldBe(["sync", "async"]);
        var plan = await pipeline.PlanAsync();
        plan.Modules.OfType<DelegateModule>().Count().ShouldBe(1);
    }

    [Test]
    public async Task ConfigureDovetail_AddsTheConfigurationJointsToThePipelineConfiguration()
    {
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z.ConfigureConfiguration(
                                                 (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Pipeline"] = "joined", })
                                             )
                                         );

        pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:Pipeline"].ShouldBe("joined");
    }

    [Test]
    public async Task ConfigureDovetail_HandsTheHostCreatedJointsThePipeline()
    {
        var joint = A.Fake<HostCreatedJointDelegate<IPipeline>>();

        await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail(z => z.ConfigureHostCreated(joint));

        A.CallTo(() => joint.Invoke(A<IDovetailContext>._, pipeline)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task DovetailPipelinePlugin_AppliesTheJoints_WhenItIsAdded()
    {
        var plugin = new DovetailPipelinePlugin(DovetailContextBuilder.Create());
        var builder = Pipeline.CreateBuilder().AddPlugin(plugin);

        plugin.Context.ShouldNotBeNull();
        plugin.Context.Get<PipelineBuilder>().ShouldBeSameAs(builder);
        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        plan.Modules.OfType<RecordedModule>().Count().ShouldBe(1);
    }
}
