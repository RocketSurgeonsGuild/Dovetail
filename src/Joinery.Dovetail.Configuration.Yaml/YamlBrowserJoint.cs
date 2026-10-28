using System.Runtime.InteropServices;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;


namespace Joinery.Dovetail.Configuration.Yaml;

/// <summary>
///     Default yaml convention
/// </summary>
[DovetailExport]
internal class YamlBrowserJoint : IConfigurationAsyncJoint
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

        await AddYamlFile(http, builder, "appsettings.yaml", cancellationToken).ConfigureAwait(false);
        await AddYamlFile(http, builder, "appsettings.yml", cancellationToken).ConfigureAwait(false);
        if (applicationName is { Length: > 0, })
        {
            await AddYamlFile(http, builder, $"{applicationName}.yaml", cancellationToken).ConfigureAwait(false);
            await AddYamlFile(http, builder, $"{applicationName}.yml", cancellationToken).ConfigureAwait(false);
        }

        if (environmentName is { Length: > 0, })
        {
            await AddYamlFile(http, builder, $"appsettings.{environmentName}.yaml", cancellationToken).ConfigureAwait(false);
            await AddYamlFile(http, builder, $"appsettings.{environmentName}.yml", cancellationToken).ConfigureAwait(false);
            if (applicationName is { Length: > 0, })
            {
                await AddYamlFile(http, builder, $"{applicationName}.{environmentName}.yaml", cancellationToken).ConfigureAwait(false);
                await AddYamlFile(http, builder, $"{applicationName}.{environmentName}.yml", cancellationToken).ConfigureAwait(false);
            }
        }

        await AddYamlFile(http, builder, "appsettings.local.yaml", cancellationToken).ConfigureAwait(false);
        await AddYamlFile(http, builder, "appsettings.local.yml", cancellationToken).ConfigureAwait(false);
        if (applicationName is { Length: > 0, })
        {
            await AddYamlFile(http, builder, $"{applicationName}.local.yaml", cancellationToken).ConfigureAwait(false);
            await AddYamlFile(http, builder, $"{applicationName}.local.yml", cancellationToken).ConfigureAwait(false);
        }
    }

    private static async ValueTask AddYamlFile(HttpClient http, IConfigurationBuilder builder, string path, CancellationToken cancellationToken)
    {
        try
        {
            // The browser's HttpClient can't read synchronously, and the configuration parsers do, so buffer the file first.
            var stream = new MemoryStream(await http.GetByteArrayAsync(path, cancellationToken).ConfigureAwait(false));
            builder.AddYamlStream(stream);
        }
        catch (HttpRequestException) { }
    }
}
