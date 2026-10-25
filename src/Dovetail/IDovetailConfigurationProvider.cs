using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail;

/// <summary>
///     Supplies the configuration that Dovetail creates a named instance of an options type from.
/// </summary>
/// <remarks>
///     <para>
///         Options registered by a generated configuration joint (or by
///         <see cref="DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions{TOptions}(Microsoft.Extensions.DependencyInjection.IServiceCollection, string, Func{IConfiguration, TOptions})" />)
///         are created from <c>GetConfiguration(name).GetSection(path)</c>, where the path is the one the type was registered with.
///         The registration resolves this service from the service provider when it creates the options.
///     </para>
///     <para>
///         The name passed is the options name, as in <see cref="IOptionsMonitor{TOptions}.Get(string?)" /> and
///         <see cref="IOptionsSnapshot{TOptions}.Get(string?)" />: <see cref="Options.DefaultName" />, which is
///         <see cref="string.Empty" />, for the default options.
///     </para>
///     <para>
///         The default implementation returns the application's <see cref="IConfiguration" /> for the default name and throws
///         <see cref="InvalidOperationException" /> for any other, so named options need a provider of your own. Register it
///         before or after the options, since a provider you register replaces the default one. That is how a multi-tenant
///         application gives each tenant its own options: a provider that returns a different configuration root per options
///         name, such as one loaded for each tenant, while the section path stays the same.
///     </para>
///     <para>
///         Reloads reach <see cref="IOptionsMonitor{TOptions}" /> through the configuration this provider returns for a name.
///         For the default name a change token source watches it and tells <see cref="IOptionsMonitor{TOptions}.OnChange" />
///         listeners. For another name the monitor's cache watches the configuration it created the options from, and
///         creates them again on the next read after a reload, without telling listeners.
///         <see cref="IOptionsSnapshot{TOptions}" /> reads current values for every name.
///     </para>
/// </remarks>
[PublicAPI]
public interface IDovetailConfigurationProvider
{
    /// <summary>
    ///     Gets the configuration used to create the options with the given name.
    /// </summary>
    /// <param name="optionsName">
    ///     The options name: <see cref="Options.DefaultName" /> (<see cref="string.Empty" />) for the default options.
    /// </param>
    /// <returns>
    ///     The root configuration that the registered section path is looked up in, not the section itself.
    /// </returns>
    /// <exception cref="InvalidOperationException">No configuration is available for <paramref name="optionsName" />.</exception>
    IConfiguration GetConfiguration(string optionsName);
}
