using System.Runtime.InteropServices;
using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;


namespace Dovetail.Configuration.Yaml;

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
            var stream = await http.GetStreamAsync(path, cancellationToken).ConfigureAwait(false);
            builder.AddYamlStream(stream);
        }
        catch (HttpRequestException) { }
    }
}
