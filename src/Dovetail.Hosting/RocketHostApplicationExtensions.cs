using System.ComponentModel;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Dovetail;

/// <summary>
///     Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an
///     <see cref="IHostApplicationBuilder" /> up to a Dovetail <see cref="IDovetailContext" />. Not intended to be
///     called directly by consumers; hidden from IntelliSense via <see cref="EditorBrowsableState.Never" />.
/// </summary>
[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailHostApplicationHelpers
{
    /// <summary>
    ///     Populates the context, applies shared host configuration, services, and logging, wires up the
    ///     service provider factory (if one was registered), and builds the host.
    /// </summary>
    /// <typeparam name="T">The <see cref="IHostApplicationBuilder" /> type being configured.</typeparam>
    /// <typeparam name="THost">The <see cref="IHost" /> type produced by <paramref name="buildHost" />.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <param name="buildHost">A callback that builds the host from the configured builder.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration and building the host.</param>
    /// <returns>The built and configured <typeparamref name="THost" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="buildHost" /> or <paramref name="contextBuilder" /> is <see langword="null" />.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<THost> Configure<T, THost>(
        T builder,
        Func<T, THost> buildHost,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
        where T : IHostApplicationBuilder
        where THost : IHost
    {
        ArgumentNullException.ThrowIfNull(buildHost);
        ArgumentNullException.ThrowIfNull(contextBuilder);

        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.GetType(), builder)
           .AddIfMissing(builder.Configuration)
           .AddIfMissing<IConfiguration>(builder.Configuration)
           .AddIfMissing(builder.Configuration.GetType(), builder.Configuration)
           .AddIfMissing(builder.Environment)
           .AddIfMissing(builder.Environment.GetType(), builder.Environment)
           .AddIfMissing("ApplicationName", builder.Environment.ApplicationName)
           .AddIfMissing("EnvironmentName", builder.Environment.EnvironmentName);

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);
        await SharedHostConfigurationAsync(context, builder, cancellationToken).ConfigureAwait(false);
        await builder.Services.ApplyService(context, cancellationToken).ConfigureAwait(false);

        if (context.Get<ServiceProviderFactoryAdapter>() is { } factory)
            builder.ConfigureContainer(await factory(context, builder.Services, cancellationToken).ConfigureAwait(false));

        await builder.ApplyHostApplication(context, cancellationToken).ConfigureAwait(false);
        var host = buildHost(builder);
        await host.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return host;
    }

    internal static ValueTask SharedHostConfigurationAsync(
        IDovetailContext context,
        IHostApplicationBuilder hostApplicationBuilder,
        CancellationToken cancellationToken
    )
    {
        // Dovetail's own IConfigurationJoint conventions (JsonDovetail/YamlDovetail/TomlDovetail, etc.) now
        // own file-based configuration loading end to end, so the host's own default file providers
        // (appsettings.json, appsettings.{Environment}.json, secrets.json, ...) would just be redundant/
        // conflicting - strip them before inserting Dovetail's own configuration.
        foreach (var fileSource in hostApplicationBuilder.Configuration.Sources.OfType<FileConfigurationSource>().ToArray())
        {
            hostApplicationBuilder.Configuration.Sources.Remove(fileSource);
        }

        // Insert after everything that's left (e.g. anything the consumer added before calling into Dovetail)
        // but before command line/environment variable configuration, so those retain the highest precedence.
        var insertIndex = hostApplicationBuilder.Configuration.Sources.Count;
        for (var i = 0; i < hostApplicationBuilder.Configuration.Sources.Count; i++)
        {
            var candidate = hostApplicationBuilder.Configuration.Sources[i];
            if (candidate is CommandLineConfigurationSource
             || ( candidate is EnvironmentVariablesConfigurationSource env
                 && ( string.IsNullOrWhiteSpace(env.Prefix) || string.Equals(env.Prefix, "RSG_", StringComparison.OrdinalIgnoreCase) ) ))
            {
                insertIndex = i;
                break;
            }
        }
        var cb = new ConfigurationBuilder();
        if (cb.Sources is { Count: > 0, })
        {
            hostApplicationBuilder.Configuration.Sources.Insert(
                insertIndex,
                new ChainedConfigurationSource
                {
                    Configuration = cb.Build(),
                    ShouldDisposeConfiguration = true,
                }
            );
        }

        hostApplicationBuilder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?> { ["RocketSurgeryDovetails:HostType"] = context.GetHostType().ToString(), }
        );
        return cb.ApplyConfiguration(context, cancellationToken);
    }
}
