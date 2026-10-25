using Dovetail.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dovetail;

/// <summary>
///     Registers strongly-typed configuration classes with the standard <see cref="Microsoft.Extensions.Options" /> pipeline.
/// </summary>
/// <remarks>
///     <para>
///         The configuration joint that Dovetail's source generator emits for <c>[DovetailConfiguration]</c> types calls
///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Func{IConfiguration, TOptions})" />
///         for every type, passing the <c>Create…</c> method it generated for it. That code creates the instance and binds the
///         section onto it without reflection, so the registration is trimming and AOT safe, and nothing here uses reflection.
///         A library registers only its own section, and reads it from the application's complete <see cref="IConfiguration" />,
///         which the service provider supplies when the options are first created.
///     </para>
/// </remarks>
[PublicAPI]
public static class DovetailConfigurationOptionsExtensions
{
    /// <summary>
    ///     Creates <typeparamref name="TOptions" /> from the configuration section at <paramref name="sectionPath" /> with
    ///     <paramref name="create" />, without reflection.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is the one registration the generated configuration joint emits, for every <c>[DovetailConfiguration]</c> type.
    ///         It registers an <see cref="IOptionsFactory{TOptions}" /> that creates each instance with
    ///         <paramref name="create" />, unless a factory is already registered for <typeparamref name="TOptions" />. That is a
    ///         <c>TryAdd</c>, so an <see cref="IOptionsFactory{TOptions}" /> the application registered itself wins over this one,
    ///         and <paramref name="create" /> is never called for that type. Registered before this call, the application's factory
    ///         makes this registration a no-op. Registered after it with a plain <c>Add</c>, it is the last registration, which the
    ///         container resolves. The factory derives from <see cref="OptionsFactory{TOptions}" />, so configure and post-configure
    ///         actions and validators still run on the instance <paramref name="create" /> returns, for named options too.
    ///     </para>
    ///     <para>
    ///         A type is created from one section only. When several calls register a section for the same type, the first one
    ///         wins and the rest are ignored, as with any other <c>TryAdd</c> registration.
    ///     </para>
    ///     <para>
    ///         The configuration comes from the <see cref="IDovetailConfigurationProvider" /> registered in the service provider,
    ///         which is resolved when the options are first created. The default provider returns the application's complete
    ///         <see cref="IConfiguration" /> for the default name, and throws for any other options name, so named options need
    ///         a provider of your own (for example one that returns a configuration per tenant). <paramref name="create" /> is
    ///         called for every name with the section at <paramref name="sectionPath" /> of the configuration the provider
    ///         returned for it. One <see cref="IOptionsChangeTokenSource{TOptions}" /> per type watches the application's
    ///         configuration, so <see cref="IOptionsMonitor{TOptions}" /> and <see cref="IOptionsSnapshot{TOptions}" /> observe
    ///         reloads of the default options. For named options whose configurations can reload, register an
    ///         <see cref="IOptionsChangeTokenSource{TOptions}" /> with that name yourself.
    ///     </para>
    ///     <para>
    ///         On .NET 11, don't give a type registered this way asynchronous validators: the options pipeline only runs them
    ///         through the built-in <see cref="OptionsFactory{TOptions}" /> itself, and through a derived factory they fail
    ///         synchronous validation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TOptions">The options type.</typeparam>
    /// <param name="services">The service collection to register against.</param>
    /// <param name="sectionPath">The path of the configuration section to create from, such as <c>Server</c> or <c>Http:Retry</c>.</param>
    /// <param name="create">Creates an instance from a configuration section.</param>
    /// <returns>
    ///     The <see cref="OptionsBuilder{TOptions}" /> for the default name, for further configuration such as <c>Validate</c> and
    ///     <c>ValidateOnStart()</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="services" />, <paramref name="sectionPath" />, or <paramref name="create" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="sectionPath" /> is empty.</exception>
    public static OptionsBuilder<TOptions> AddDovetailConfigurationOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
        this IServiceCollection services,
        string sectionPath,
        Func<IConfiguration, TOptions> create
    )
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(sectionPath);
        ArgumentNullException.ThrowIfNull(create);

        var builder = services.AddOptions<TOptions>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TOptions>, ConfigurationChangeTokenSource<TOptions>>(
                static provider => new ConfigurationChangeTokenSource<TOptions>(provider.GetRequiredService<IConfiguration>())
            )
        );
        services.TryAddSingleton<IDovetailConfigurationProvider>(static provider => new DefaultDovetailConfigurationProvider(provider.GetRequiredService<IConfiguration>()));
        services.TryAdd(
            ServiceDescriptor.Transient<IOptionsFactory<TOptions>, DovetailOptionsFactory<TOptions>>(provider => new DovetailOptionsFactory<TOptions>(
                    provider.GetRequiredService<IDovetailConfigurationProvider>(),
                    sectionPath,
                    create,
                    provider.GetServices<IConfigureOptions<TOptions>>(),
                    provider.GetServices<IPostConfigureOptions<TOptions>>(),
                    provider.GetServices<IValidateOptions<TOptions>>()
                )
            )
        );
        return builder;
    }
}
