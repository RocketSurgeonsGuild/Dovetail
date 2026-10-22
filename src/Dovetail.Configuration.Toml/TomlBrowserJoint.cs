using System.Runtime.InteropServices;
using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Tomlyn.Extensions.Configuration;


namespace Dovetail.Configuration.Toml;

/// <summary>
///     Default toml convention
/// </summary>
[DovetailExport]
internal class TomlBrowserJoint : IConfigurationAsyncJoint
{
    /// <inheritdoc />
    public async ValueTask Register(IDovetailContext context, IConfigurationBuilder builder, CancellationToken cancellationToken = default)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser"))) return;
        var baseAddress = context.Get<string>("BaseAddress");
        if (baseAddress is not { Length: > 0, }) return;

        var applicationName = context.Get<string>("ApplicationName");
        var environmentName = context.Get<string>("EnvironmentName");

        using var http = new HttpClient { BaseAddress = new(baseAddress), };

        await AddTomlFile(http, builder, "appsettings.toml", cancellationToken).ConfigureAwait(false);
        if (applicationName is { Length: > 0, }) await AddTomlFile(http, builder, $"{applicationName}.toml", cancellationToken).ConfigureAwait(false);

        if (environmentName is { Length: > 0, })
        {
            await AddTomlFile(http, builder, $"appsettings.{environmentName}.toml", cancellationToken).ConfigureAwait(false);
            if (applicationName is { Length: > 0, })
                await AddTomlFile(http, builder, $"{applicationName}.{environmentName}.toml", cancellationToken).ConfigureAwait(false);
        }

        await AddTomlFile(http, builder, "appsettings.local.toml", cancellationToken).ConfigureAwait(false);
        if (applicationName is { Length: > 0, }) await AddTomlFile(http, builder, $"{applicationName}.local.toml", cancellationToken).ConfigureAwait(false);
    }

    private static async ValueTask AddTomlFile(HttpClient http, IConfigurationBuilder builder, string path, CancellationToken cancellationToken)
    {
        try
        {
            // The browser's HttpClient can't read synchronously, and the configuration parsers do, so buffer the file first.
            var stream = new MemoryStream(await http.GetByteArrayAsync(path, cancellationToken).ConfigureAwait(false));
            builder.AddTomlStream(stream);
        }
        catch (HttpRequestException) { }
    }
}
