using System.ComponentModel;
using System.Reflection;
using Joinery.Dovetail.Hosting.Uno;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Joinery.Dovetail;

/// <summary>
///     Shared helpers used to wire an Uno.Extensions <see cref="IApplicationBuilder" /> up to a Dovetail <see cref="IDovetailContext" />.
///     Not intended to be called directly by consumers; hidden from IntelliSense via <see cref="EditorBrowsableState.Never" />.
/// </summary>
[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailUnoHelpers
{
    /// <summary>
    ///     Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the
    ///     application's host, and builds it.
    /// </summary>
    /// <param name="builder">The Uno application builder to configure.</param>
    /// <param name="buildHost">A callback that builds the host from the configured builder.</param>
    /// <param name="applicationAssembly">The application's assembly, which its embedded configuration files are loaded from.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration and building the host.</param>
    /// <returns>The built and configured <see cref="IHost" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="builder" />, <paramref name="buildHost" />, <paramref name="applicationAssembly" />, or
    ///     <paramref name="contextBuilder" /> is <see langword="null" />.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<IHost> Configure(
        this IApplicationBuilder builder,
        Func<IApplicationBuilder, IHost> buildHost,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(buildHost);
        var context = await PrepareAsync(builder, applicationAssembly, contextBuilder, cancellationToken).ConfigureAwait(false);
        var host = buildHost(builder);
        await host.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return host;
    }

    /// <summary>
    ///     Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the
    ///     application's host, then builds it and navigates to <typeparamref name="TShell" /> (Uno.Extensions.Navigation).
    /// </summary>
    /// <typeparam name="TShell">The shell the application navigates to.</typeparam>
    /// <param name="builder">The Uno application builder to configure.</param>
    /// <param name="applicationAssembly">The application's assembly, which its embedded configuration files are loaded from.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration and building the host.</param>
    /// <returns>The built and configured <see cref="IHost" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="builder" />, <paramref name="applicationAssembly" />, or <paramref name="contextBuilder" /> is <see langword="null" />.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<IHost> Navigate<TShell>(
        this IApplicationBuilder builder,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
        where TShell : UIElement, new()
    {
        var context = await PrepareAsync(builder, applicationAssembly, contextBuilder, cancellationToken).ConfigureAwait(false);
        var host = await builder.NavigateAsync<TShell>().ConfigureAwait(false);
        await host.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return host;
    }

    private static async ValueTask<IDovetailContext> PrepareAsync(
        IApplicationBuilder builder,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(applicationAssembly);
        ArgumentNullException.ThrowIfNull(contextBuilder);

        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.GetType(), builder)
           .AddIfMissing("ExecutingAssembly", applicationAssembly)
           .AddIfMissing(DovetailEmbeddedConfiguration.AssemblyKey, applicationAssembly)
           .AddIfMissing("ApplicationName", applicationAssembly.GetName().Name ?? "");
        if (builder.App is { } app) contextBuilder.AddIfMissing(app).AddIfMissing(app.GetType(), app);
        if (builder.Window is { } window) contextBuilder.AddIfMissing(window);
        if (builder.Arguments is { } arguments) contextBuilder.AddIfMissing(arguments);

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);
        await builder.ApplyUnoApplicationBuilder(context, cancellationToken).ConfigureAwait(false);
        builder.Configure(host => UnoHostBuilderBridge.Configure(host, context, cancellationToken));
        return context;
    }
}
