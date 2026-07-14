using System.ComponentModel;
using Dovetail;
using Dovetail.Hosting.WebAssembly;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Dovetail;

[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailWebAssemblyHelpers
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<WebAssemblyHost> Configure(
        this WebAssemblyHostBuilder builder,
        Func<WebAssemblyHostBuilder, WebAssemblyHost> buildHost,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(buildHost);
        ArgumentNullException.ThrowIfNull(contextBuilder);

        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.GetType(), builder)
           .AddIfMissing<IConfiguration>(builder.Configuration)
           .AddIfMissing(builder.HostEnvironment)
           .AddIfMissing(builder.HostEnvironment.GetType(), builder.HostEnvironment)
           .AddIfMissing("BaseAddress", builder.HostEnvironment.BaseAddress)
           .AddIfMissing("EnvironmentName", builder.HostEnvironment.Environment);
        contextBuilder.Set("BlazorWasm", true);

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);

        await SharedHostConfigurationAsync(context, builder, cancellationToken).ConfigureAwait(false);
        await builder.Services.ApplyService(context, cancellationToken).ConfigureAwait(false);
        await builder.Logging.ApplyLogging(context, cancellationToken).ConfigureAwait(false);

        if (context.Get<ServiceProviderFactoryAdapter>() is { } factory)
            builder.ConfigureContainer(await factory(context, builder.Services, cancellationToken).ConfigureAwait(false));

        await builder.ApplyWebAssemblyHostBuilder(context, cancellationToken).ConfigureAwait(false);
        var host = buildHost(builder);
        await host.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return host;
    }

    internal static ValueTask SharedHostConfigurationAsync(
        IDovetailContext context,
        WebAssemblyHostBuilder builder,
        CancellationToken cancellationToken
    )
    {
        var configurationBuilder = (IConfigurationBuilder)builder.Configuration;

        // Dovetail's own IConfigurationAsyncJoint conventions (JsonBrowserDovetail/YamlBrowserDovetail/
        // TomlBrowserDovetail, etc.) now own configuration loading end to end - including the HTTP fetch
        // that used to be Blazor's own default behavior for appsettings.json/appsettings.{Environment}.json -
        // so strip whatever the default WebAssembly host already loaded to avoid double-loading/precedence
        // conflicts, then insert a freshly-built ConfigurationBuilder in its place.
        foreach (var existing in configurationBuilder.Sources.OfType<JsonStreamConfigurationSource>().ToArray())
        {
            configurationBuilder.Sources.Remove(existing);
        }

        var cb = new ConfigurationBuilder();
        if (cb.Sources is { Count: > 0, })
        {
            configurationBuilder.Add(
                new ChainedConfigurationSource
                {
                    Configuration = cb.Build(),
                    ShouldDisposeConfiguration = true,
                }
            );
        }
        return cb.ApplyConfiguration(context, cancellationToken);
    }
}
