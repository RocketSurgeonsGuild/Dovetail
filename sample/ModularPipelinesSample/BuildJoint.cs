using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelinesSample;

/// <summary>
///     Adds the sample's module to every pipeline that applies this assembly's joints, and registers the service the module uses.
/// </summary>
[DovetailExport]
public sealed class BuildJoint : IPipelineBuilderJoint
{
    public void Register(IDovetailContext context, PipelineBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
        builder.AddModule<GreetingModule>();
    }
}

/// <summary>
///     The module the joint adds.
/// </summary>
public sealed class GreetingModule(TimeProvider timeProvider) : Module<string>
{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
        Task.FromResult($"Joined at {timeProvider.GetUtcNow():O}");
}
