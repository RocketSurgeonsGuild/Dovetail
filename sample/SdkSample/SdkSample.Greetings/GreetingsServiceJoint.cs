using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.Greetings;

/// <summary>
///     Exported to every host that references this library; the host's generated Imports class
///     discovers it at build time.
/// </summary>
[DovetailExport]
public sealed class GreetingsServiceJoint : IServiceJoint
{
    private const string MessageKey = "Greetings:Message";

    public void Register(IDovetailContext context, IServiceCollection services)
    {
        // No fallback on purpose: the value only exists if the SDK's generated YAML configuration
        // joint loaded SdkSample.Greetings.yaml, so a silent default would hide a broken pipeline.
        var message = context.Configuration[MessageKey]
         ?? throw new InvalidOperationException($"'{MessageKey}' is not configured - SdkSample.Greetings.yaml was not loaded into the host configuration.");
        services.AddSingleton<IGreeter>(new Greeter(message));
    }
}
