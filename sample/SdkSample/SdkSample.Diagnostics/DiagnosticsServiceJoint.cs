using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.Diagnostics;

/// <summary>
///     Exported to every host that references this library. The options need no registration here: Dovetail's generated
///     configuration joint binds <see cref="DiagnosticsOptions" /> and, for the assembly attributes, <c>HostOptions</c> and
///     <c>ConsoleLifetimeOptions</c> from <c>SdkSample.Diagnostics.yaml</c>.
/// </summary>
[DovetailExport]
public sealed class DiagnosticsServiceJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<IDiagnosticsReport, DiagnosticsReport>();
}
