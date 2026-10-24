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
///         The configuration joint that Dovetail's source generator emits for <c>[DovetailConfiguration]</c> types calls the
///         reflection-free overloads, passing the binder or factory it generated for the type:
///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Action{IConfiguration, TOptions}, int)" />
///         for types the options pipeline can create, and
///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Func{IConfiguration, TOptions}, int)" />
///         for types it can't. Both are trimming and AOT safe, and sections registered through them stack: every section registered
///         for a type, from any assembly, applies to the type's one default <see cref="IOptions{TOptions}" />.
///     </para>
///     <para>
///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, IConfiguration, string)" /> binds with the
///         reflection-based configuration binder instead.
///     </para>
/// </remarks>
[PublicAPI]
public static class DovetailConfigurationOptionsExtensions
{
    /// <summary>
    ///     Binds <typeparamref name="TOptions" /> to the configuration section at
    ///     <paramref name="sectionKey" /> via the standard <see cref="IOptions{TOptions}" /> pipeline.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="OptionsBuilder{TOptions}" />.Bind(<see cref="IConfiguration" />) — from
    ///         <c>Microsoft.Extensions.Options.ConfigurationExtensions</c> — registers both the
    ///         <see cref="IConfigureOptions{TOptions}" /> binder <b>and</b> the
    ///         <see cref="IOptionsChangeTokenSource{TOptions}" /> that watches
    ///         <see cref="IConfiguration.GetReloadToken" />. That single call is what makes
    ///         <see cref="IOptionsMonitor{TOptions}" />/<see cref="IOptionsSnapshot{TOptions}" /> observe
    ///         changes (e.g. a file-system change on a source registered with <c>reloadOnChange: true</c>)
    ///         without any additional wiring — there is intentionally no separate, hand-rolled
    ///         <see cref="IOptionsChangeTokenSource{TOptions}" /> registration here.
    ///     </para>
    ///     <para>
    ///         This calls into <c>Microsoft.Extensions.Options.ConfigurationExtensions</c>'
    ///         reflection-based <c>ConfigurationBinder</c>, so it carries the same trimming/AOT
    ///         caveats as any other <c>IConfiguration</c> binding call — the attributes below make
    ///         that explicit rather than silently swallowing the warning. Generated code calls the reflection-free overloads
    ///         instead,
    ///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Action{IConfiguration, TOptions}, int)" />
    ///         and
    ///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Func{IConfiguration, TOptions}, int)" />.
    ///         Unlike them, this binds the <paramref name="configuration" /> it's given, and each call adds its own binder, applied in
    ///         registration order without their ordering or de-duplication.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TOptions">The generated configuration class type.</typeparam>
    /// <param name="services">The service collection to register against.</param>
    /// <param name="configuration">The root configuration to bind from.</param>
    /// <param name="sectionKey">The configuration section key that maps to <typeparamref name="TOptions" />.</param>
    /// <returns>
    ///     The <see cref="OptionsBuilder{TOptions}" /> for further configuration, such as
    ///     <c>Validate&lt;TValidateOptions&gt;()</c> and <c>ValidateOnStart()</c>. On .NET 11 its asynchronous
    ///     <c>Validate</c> overloads run during host startup through <c>IAsyncStartupValidator</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="services" />, <paramref name="configuration" />, or <paramref name="sectionKey" /> is <see langword="null" />.</exception>
    [RequiresUnreferencedCode("Binds TOptions via reflection-based ConfigurationBinder; TOptions's members must be preserved.")]
    [RequiresDynamicCode("Binds TOptions via reflection-based ConfigurationBinder, which may require runtime code generation.")]
    public static OptionsBuilder<TOptions> AddDovetailConfigurationOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionKey
    )
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(sectionKey);

        return services
              .AddOptions<TOptions>()
              .Bind(configuration.GetSection(sectionKey));
    }

    /// <summary>
    ///     Binds the configuration section at <paramref name="sectionPath" /> onto the default (unnamed)
    ///     <typeparamref name="TOptions" />, without reflection. The options pipeline creates the instance, and
    ///     <paramref name="bind" /> fills it.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is the registration the generated configuration joint emits for mutable types. Sections stack: every section
    ///         registered for <typeparamref name="TOptions" /> through the reflection-free overloads, from any assembly, applies to
    ///         the same instance, ordered by <paramref name="order" />, then by section path compared case-insensitively, then
    ///         ordinally. A path registered more than once for the type (compared case-insensitively) applies once, since binding it
    ///         again would append its lists twice: the registration with the lowest <paramref name="order" /> wins, and at equal order
    ///         the first one.
    ///     </para>
    ///     <para>
    ///         All the sections bind from one <see cref="IConfigureOptions{TOptions}" />, registered where the first section for the
    ///         type was. Options configured before that call apply before every section, and options configured after it apply after
    ///         every section.
    ///     </para>
    ///     <para>
    ///         The <see cref="IConfiguration" /> is resolved from the service provider when the options are first created, and
    ///         <paramref name="bind" /> is passed its section at <paramref name="sectionPath" />. One
    ///         <see cref="IOptionsChangeTokenSource{TOptions}" /> per type watches it for reloads, so
    ///         <see cref="IOptionsMonitor{TOptions}" /> and <see cref="IOptionsSnapshot{TOptions}" /> observe changes. Only the default
    ///         name is bound; named options are left alone.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TOptions">The options type. The options pipeline creates it with its public parameterless constructor.</typeparam>
    /// <param name="services">The service collection to register against.</param>
    /// <param name="sectionPath">The path of the configuration section to bind, such as <c>Greetings</c> or <c>Http:Retry</c>.</param>
    /// <param name="bind">Binds a configuration section onto an existing instance.</param>
    /// <param name="order">Where this section binds among the sections registered for <typeparamref name="TOptions" />; lower first.</param>
    /// <returns>
    ///     The <see cref="OptionsBuilder{TOptions}" /> for the default name, for further configuration such as <c>Validate</c> and
    ///     <c>ValidateOnStart()</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="services" />, <paramref name="sectionPath" />, or <paramref name="bind" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="sectionPath" /> is empty.</exception>
    public static OptionsBuilder<TOptions> AddDovetailConfigurationOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
        this IServiceCollection services,
        string sectionPath,
        Action<IConfiguration, TOptions> bind,
        int order = 0
    )
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(sectionPath);
        ArgumentNullException.ThrowIfNull(bind);

        return AddSection(services, new DovetailConfigurationSection<TOptions>(sectionPath, order, bind));
    }

    /// <summary>
    ///     Creates the default (unnamed) <typeparamref name="TOptions" /> from the configuration section at
    ///     <paramref name="sectionPath" />, without reflection, for types the options pipeline can't create: types with constructor
    ///     parameters, or with init-only or required members.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is the registration the generated configuration joint emits for immutable types. It registers an
    ///         <see cref="IOptionsFactory{TOptions}" /> that creates the default instance with <paramref name="create" />, unless a
    ///         factory is already registered for <typeparamref name="TOptions" /> itself. The factory derives from
    ///         <see cref="OptionsFactory{TOptions}" />, so configure and post-configure actions and validators still run on the
    ///         instance it creates. Named options are created by the base class, which needs a public parameterless constructor.
    ///     </para>
    ///     <para>
    ///         Sections stack, ordered and de-duplicated as for
    ///         <see cref="AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Action{IConfiguration, TOptions}, int)" />.
    ///         With several creating sections, they're merged in order into one configuration, later sections overriding earlier
    ///         ones key by key, and the first section's <paramref name="create" /> creates the instance from it. Binding sections
    ///         registered for the type then bind onto that instance, except those whose path is also a creating section.
    ///     </para>
    ///     <para>
    ///         The <see cref="IConfiguration" /> is resolved from the service provider when the options are first created, and reloads
    ///         reach <see cref="IOptionsMonitor{TOptions}" /> and <see cref="IOptionsSnapshot{TOptions}" />, as for the binding
    ///         overload.
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
    /// <param name="order">Where this section applies among the sections registered for <typeparamref name="TOptions" />; lower first.</param>
    /// <returns>
    ///     The <see cref="OptionsBuilder{TOptions}" /> for the default name, for further configuration such as <c>Validate</c> and
    ///     <c>ValidateOnStart()</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="services" />, <paramref name="sectionPath" />, or <paramref name="create" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="sectionPath" /> is empty.</exception>
    public static OptionsBuilder<TOptions> AddDovetailConfigurationOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
        this IServiceCollection services,
        string sectionPath,
        Func<IConfiguration, TOptions> create,
        int order = 0
    )
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(sectionPath);
        ArgumentNullException.ThrowIfNull(create);

        var builder = AddSection(services, new DovetailConfigurationSection<TOptions>(sectionPath, order, create));
        services.TryAdd(
            ServiceDescriptor.Transient<IOptionsFactory<TOptions>, DovetailOptionsFactory<TOptions>>(
                static provider => new DovetailOptionsFactory<TOptions>(
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetServices<DovetailConfigurationSection<TOptions>>(),
                    provider.GetServices<IConfigureOptions<TOptions>>(),
                    provider.GetServices<IPostConfigureOptions<TOptions>>(),
                    provider.GetServices<IValidateOptions<TOptions>>()
                )
            )
        );
        return builder;
    }

    private static OptionsBuilder<TOptions> AddSection<TOptions>(IServiceCollection services, DovetailConfigurationSection<TOptions> section)
        where TOptions : class
    {
        var builder = services.AddOptions<TOptions>();
        services.AddSingleton(section);
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IConfigureOptions<TOptions>, DovetailConfigureOptions<TOptions>>(
                static provider => new DovetailConfigureOptions<TOptions>(
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetServices<DovetailConfigurationSection<TOptions>>()
                )
            )
        );
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TOptions>, DovetailConfigurationChangeTokenSource<TOptions>>(
                static provider => new DovetailConfigurationChangeTokenSource<TOptions>(provider.GetRequiredService<IConfiguration>())
            )
        );
        return builder;
    }
}
