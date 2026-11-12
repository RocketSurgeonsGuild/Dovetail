using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;

namespace Joinery.Dovetail.ModularPipelines.Tests;

/// <summary>
///     Records what the pipeline did, so a test can see that a module a joint added ran with a service another joint registered.
/// </summary>
public sealed class PipelineRecorder
{
    public List<string> Ran { get; } = [];
}

/// <summary>
///     Exported, so the source generator adds it to this assembly's imports and every pipeline of the tests gets the module.
/// </summary>
[DovetailExport]
public sealed class AddRecordedModuleJoint : IPipelineBuilderJoint, IServiceJoint
{
    public void Register(IDovetailContext context, PipelineBuilder builder) => builder.AddModule<RecordedModule>();

    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<PipelineRecorder>();
}

public sealed class RecordedModule(PipelineRecorder recorder) : Module<bool>
{
    protected override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        recorder.Ran.Add(nameof(RecordedModule));
        return Task.FromResult(true);
    }
}

public sealed class DelegateModule : Module<bool>
{
    protected override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(true);
}
