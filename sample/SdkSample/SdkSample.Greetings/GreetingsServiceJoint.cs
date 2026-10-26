using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.Greetings;

/// <summary>
///     Exported to every host that references this library; the host's generated Imports class
///     discovers it at build time. <see cref="GreetingsOptions" /> and <see cref="FarewellOptions" /> need no
///     registration here: Dovetail's generated configuration joint binds them from the library's YAML file.
/// </summary>
[DovetailExport]
public sealed class GreetingsServiceJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<IGreeter, Greeter>();
}
