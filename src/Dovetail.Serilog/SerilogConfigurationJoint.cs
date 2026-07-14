using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Serilog;

/// <summary>
///     Hand-authored stand-in for the generator-emitted <c>IConfigurationJoint</c> that
///     <c>openspec/changes/dovetail-managed-configuration</c> will eventually emit for this library's
///     <c>appsettings.json</c> (see design.md, Decision 4). Registers
///     <see cref="SerilogRuntimeConfiguration" /> through the same hook
///     (<see cref="DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions{TOptions}" />)
///     the generator is expected to call, so this dogfood composes without changes once the
///     generator lands and this handwritten registration is retired.
/// </summary>
[DovetailExport]
public sealed class SerilogConfigurationJoint : IServiceJoint
{
    /// <inheritdoc />
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dogfood stand-in for generator-emitted binding; see AddDovetailConfigurationOptions.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dogfood stand-in for generator-emitted binding; see AddDovetailConfigurationOptions.")]
    public void Register(IDovetailContext context, IServiceCollection services) =>
        services.AddDovetailConfigurationOptions<SerilogRuntimeConfiguration>(context.Configuration, "Serilog");
}
