using System.ComponentModel;
using System.Reflection;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Joinery.Dovetail;

/// <summary>
///     Shared helpers used to wire a <see cref="MauiAppBuilder" /> up to a Dovetail <see cref="IDovetailContext" />. Not intended to be
///     called directly by consumers; hidden from IntelliSense via <see cref="EditorBrowsableState.Never" />.
/// </summary>
[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailMauiHelpers
{
    /// <summary>
    ///     The name of the <see cref="AssemblyMetadataAttribute" /> the Dovetail SDK stamps on a MAUI application with its build-time
    ///     environment name (<c>Development</c> for Debug builds, <c>Production</c> otherwise, or <c>$(DovetailEnvironmentName)</c>).
    /// </summary>
    public const string EnvironmentNameMetadataKey = "DovetailEnvironmentName";

    /// <summary>
    ///     Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one
    ///     was registered), and builds the app.
    /// </summary>
    /// <param name="builder">The MAUI app builder to configure.</param>
    /// <param name="buildHost">A callback that builds the app from the configured builder.</param>
    /// <param name="applicationAssembly">
    ///     The application's assembly. Its embedded configuration files are loaded, and its name and build-time environment name are
    ///     used, since <see cref="Assembly.GetEntryAssembly" /> isn't reliable on every MAUI platform.
    /// </param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration and building the app.</param>
    /// <returns>The built and configured <see cref="MauiApp" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="builder" />, <paramref name="buildHost" />, <paramref name="applicationAssembly" />, or
    ///     <paramref name="contextBuilder" /> is <see langword="null" />.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<MauiApp> Configure(
        this MauiAppBuilder builder,
        Func<MauiAppBuilder, MauiApp> buildHost,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(buildHost);
        ArgumentNullException.ThrowIfNull(applicationAssembly);
        ArgumentNullException.ThrowIfNull(contextBuilder);

        // MAUI's own IHostEnvironment reports a fixed environment and can't be changed (its setters throw), so replace it with one
        // that carries Dovetail's environment and application name, for the context and for services that read IHostEnvironment.
        IHostApplicationBuilder hostBuilder = builder;
        var environment = new MauiHostEnvironment(
            GetEnvironmentName(applicationAssembly),
            applicationAssembly.GetName().Name ?? hostBuilder.Environment.ApplicationName
        );
        builder.Services.Replace(ServiceDescriptor.Singleton<IHostEnvironment>(environment));

        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.GetType(), builder)
           .AddIfMissing(builder.Configuration)
           .AddIfMissing<IConfiguration>(builder.Configuration)
           .AddIfMissing<IHostEnvironment>(environment)
           .AddIfMissing("ExecutingAssembly", applicationAssembly)
           .AddIfMissing(DovetailEmbeddedConfiguration.AssemblyKey, applicationAssembly)
           .AddIfMissing("ApplicationName", environment.ApplicationName)
           .AddIfMissing("EnvironmentName", environment.EnvironmentName)
           .AddIfMissing("Platform", GetPlatform());

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);
        await DovetailHostApplicationHelpers.SharedHostConfigurationAsync(context, builder, cancellationToken).ConfigureAwait(false);
        await builder.Services.ApplyService(context, cancellationToken).ConfigureAwait(false);

        if (context.Get<ServiceProviderFactoryAdapter>() is { } factory)
            builder.ConfigureContainer(await factory(context, builder.Services, cancellationToken).ConfigureAwait(false));

        await builder.ApplyHostApplication(context, cancellationToken).ConfigureAwait(false);
        await builder.ApplyMauiAppBuilder(context, cancellationToken).ConfigureAwait(false);
        var app = buildHost(builder);
        await app.ApplyMauiApp(context, cancellationToken).ConfigureAwait(false);
        await app.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return app;
    }

    /// <summary>
    ///     MAUI reports a fixed environment, so take it from <c>DOTNET_ENVIRONMENT</c> when set, then from the build-time value the SDK
    ///     stamped on the application, and otherwise assume <c>Production</c>.
    /// </summary>
    internal static string GetEnvironmentName(Assembly applicationAssembly) =>
        Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") is { Length: > 0, } fromEnvironment
            ? fromEnvironment
            : applicationAssembly
             .GetCustomAttributes<AssemblyMetadataAttribute>()
             .FirstOrDefault(attribute => attribute.Key == EnvironmentNameMetadataKey)
            ?.Value is { Length: > 0, } fromBuild
                ? fromBuild
                : Environments.Production;

    private static string GetPlatform() =>
        OperatingSystem.IsAndroid() ? "Android"
        : OperatingSystem.IsMacCatalyst() ? "MacCatalyst"
        : OperatingSystem.IsIOS() ? "iOS"
        : OperatingSystem.IsWindows() ? "Windows"
        : OperatingSystem.IsMacOS() ? "macOS"
        : OperatingSystem.IsLinux() ? "Linux"
        : "Unknown";

    private sealed class MauiHostEnvironment(string environmentName, string applicationName) : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = environmentName;
        public string ApplicationName { get; set; } = applicationName;
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
