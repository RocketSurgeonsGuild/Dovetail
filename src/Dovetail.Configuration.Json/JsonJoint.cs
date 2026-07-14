using System.Runtime.InteropServices;
using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace Dovetail.Configuration.Json;

/// <summary>
///     Json configuraiton conventions
/// </summary>
[DovetailExport]
internal class JsonJoint : IConfigurationJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IConfigurationBuilder builder)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser"))) return;
        var applicationName = context.Get<string>("ApplicationName");
        var environmentName = context.Get<string>("EnvironmentName");

        AddJsonFile(builder, "appsettings.json");
        if (applicationName is { Length: > 0 }) AddJsonFile(builder, $"{applicationName}.json");

        if (environmentName is { Length: > 0 })
        {
            AddJsonFile(builder, $"appsettings.{environmentName}.json");
            if (applicationName is { Length: > 0 }) AddJsonFile(builder, $"{applicationName}.{environmentName}.json");
        }

        AddJsonFile(builder, "appsettings.local.json");
        if (applicationName is { Length: > 0 }) AddJsonFile(builder, $"{applicationName}.local.json");
    }

    private static void AddJsonFile(IConfigurationBuilder builder, string path) =>
        builder.Add(
            new JsonConfigurationSource
            {
                Path = path,
                FileProvider = builder.GetFileProvider(),
                ReloadOnChange = true,
                Optional = true,
            }
        );
}
