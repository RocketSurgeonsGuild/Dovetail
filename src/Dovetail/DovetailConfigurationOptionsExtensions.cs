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
    ///         returned for it. A change token source for the default name watches the configuration the provider returns for it,
    ///         so <see cref="IOptionsMonitor{TOptions}" /> and <see cref="IOptionsSnapshot{TOptions}" /> observe reloads of the
    ///         default options. For named options, <see cref="IOptionsSnapshot{TOptions}" /> needs nothing more, and
    ///         <see cref="IOptionsMonitor{TOptions}" /> needs
    ///         <see cref="AddDovetailConfigurationChangeTokenSource{TOptions}(IServiceCollection, string)" /> for each name.
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

        // TryAddEnumerable keys on the implementation type, so registering the same options type again doesn't add a second source.
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TOptions>, DovetailConfigurationChangeTokenSource<TOptions>>(provider => new DovetailConfigurationChangeTokenSource<TOptions>(
                    provider.GetRequiredService<IDovetailConfigurationProvider>(),
                    Options.DefaultName
                )
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

    /// <summary>
    ///     Makes <see cref="IOptionsMonitor{TOptions}" /> observe reloads of the options named <paramref name="optionsName" />,
    ///     which it does for the default name already.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Registers an <see cref="IOptionsChangeTokenSource{TOptions}" /> that watches the configuration the
    ///         <see cref="IDovetailConfigurationProvider" /> returns for <paramref name="optionsName" />. An
    ///         <see cref="IOptionsMonitor{TOptions}" /> takes its change token sources when it is created, and a source has one
    ///         fixed name, so call this for each name up front, such as for each known tenant, before the first
    ///         <see cref="IOptionsMonitor{TOptions}" /> for the type is resolved. A name without a source is created once and not
    ///         reloaded by the monitor.
    ///     </para>
    ///     <para>
    ///         <see cref="IOptionsSnapshot{TOptions}" /> needs no registration: it creates its options for each scope from the
    ///         current configuration.
    ///     </para>
    ///     <para>
    ///         The source asks the provider for the configuration when the monitor is created. The default provider throws for
    ///         every name but the default one, so a registration for another name needs a provider of your own, or resolving the
    ///         monitor throws. Call it once per name: registering a name twice adds two sources, and the monitor then reports each reload twice.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TOptions">The options type.</typeparam>
    /// <param name="services">The service collection to register against.</param>
    /// <param name="optionsName">The options name, as passed to <see cref="IOptionsMonitor{TOptions}.Get(string?)" />.</param>
    /// <returns>The <paramref name="services" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services" /> or <paramref name="optionsName" /> is <see langword="null" />.</exception>
    public static IServiceCollection AddDovetailConfigurationChangeTokenSource<TOptions>(this IServiceCollection services, string optionsName)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(optionsName);

        // Typed as the interface so it never collides with the default-name source above, which TryAddEnumerable de-duplicates.
        services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(provider => new DovetailConfigurationChangeTokenSource<TOptions>(
                provider.GetRequiredService<IDovetailConfigurationProvider>(),
                optionsName
            )
        );
        return services;
    }
}
