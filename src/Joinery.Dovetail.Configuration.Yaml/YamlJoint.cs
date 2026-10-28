using System.Runtime.InteropServices;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;


namespace Joinery.Dovetail.Configuration.Yaml;

/// <summary>
///     Default yaml convention
/// </summary>
[DovetailExport]
internal class YamlJoint : IConfigurationJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IConfigurationBuilder builder)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser")) || context.EmbeddedConfigurationAssembly is not null) return;
        var applicationName = context.Get<string>("ApplicationName");
        var environmentName = context.Get<string>("EnvironmentName");

        AddYamlFile(builder, "appsettings.yaml");
        AddYamlFile(builder, "appsettings.yml");
        if (applicationName is { Length: > 0 })
        {
            AddYamlFile(builder, $"{applicationName}.yaml");
            AddYamlFile(builder, $"{applicationName}.yml");
        }

        if (environmentName is { Length: > 0 })
        {
            AddYamlFile(builder, $"appsettings.{environmentName}.yaml");
            AddYamlFile(builder, $"appsettings.{environmentName}.yml");
            if (applicationName is { Length: > 0 })
            {
                AddYamlFile(builder, $"{applicationName}.{environmentName}.yaml");
                AddYamlFile(builder, $"{applicationName}.{environmentName}.yml");
            }
        }

        AddYamlFile(builder, "appsettings.local.yaml");
        AddYamlFile(builder, "appsettings.local.yml");
        if (applicationName is { Length: > 0 })
        {
            AddYamlFile(builder, $"{applicationName}.local.yaml");
            AddYamlFile(builder, $"{applicationName}.local.yml");
        }
    }

    private static void AddYamlFile(IConfigurationBuilder builder, string path) =>
        builder.Add(
            new YamlConfigurationSource
            {
                Path = path,
                FileProvider = builder.GetFileProvider(),
                ReloadOnChange = true,
                Optional = true,
            }
        );
}
