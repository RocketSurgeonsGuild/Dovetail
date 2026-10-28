using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Serilog;

/// <summary>
///     Hand-authored stand-in for the generator-emitted <c>IConfigurationJoint</c> that
///     <c>openspec/changes/dovetail-managed-configuration</c> will eventually emit for this library's
///     <c>appsettings.json</c> (see design.md, Decision 4). Registers <see cref="SerilogRuntimeConfiguration" /> with
///     Microsoft's <c>Configure&lt;TOptions&gt;(IConfiguration)</c>, which binds the <c>Serilog</c> section of
///     the context's <c>Configuration</c> with the reflection-based configuration binder and observes reloads.
///     The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.
/// </summary>
[DovetailExport]
public sealed class SerilogConfigurationJoint : IServiceJoint
{
    /// <inheritdoc />
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dogfood stand-in for generator-emitted binding; Configure<T> binds through the reflection-based ConfigurationBinder.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dogfood stand-in for generator-emitted binding; Configure<T> binds through the reflection-based ConfigurationBinder.")]
    public void Register(IDovetailContext context, IServiceCollection services) =>
        services.Configure<SerilogRuntimeConfiguration>(context.Configuration.GetSection("Serilog"));
}
