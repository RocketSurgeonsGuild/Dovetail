using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Tomlyn.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Toml;

/// <summary>
///     Toml configuration conventions for hosts that embed their configuration files (see <see cref="DovetailEmbeddedConfiguration" />)
/// </summary>
[DovetailExport]
internal class TomlEmbeddedJoint : IConfigurationJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IConfigurationBuilder builder)
    {
        if (context.EmbeddedConfigurationAssembly is not { } assembly) return;

        foreach (var fileName in DovetailEmbeddedConfiguration.GetConventionalFileNames(context, "toml"))
        {
            if (DovetailEmbeddedConfiguration.OpenResource(assembly, fileName) is { } stream)
            {
                builder.AddTomlStream(stream);
            }
        }
    }
}
