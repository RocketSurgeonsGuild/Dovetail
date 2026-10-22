using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;

namespace Dovetail.Configuration.Yaml;

/// <summary>
///     Yaml configuration conventions for hosts that embed their configuration files (see <see cref="DovetailEmbeddedConfiguration" />)
/// </summary>
[DovetailExport]
internal class YamlEmbeddedJoint : IConfigurationJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IConfigurationBuilder builder)
    {
        if (context.EmbeddedConfigurationAssembly is not { } assembly) return;

        foreach (var fileName in DovetailEmbeddedConfiguration.GetConventionalFileNames(context, "yaml", "yml"))
        {
            if (DovetailEmbeddedConfiguration.OpenResource(assembly, fileName) is { } stream)
            {
                builder.AddYamlStream(stream);
            }
        }
    }
}
