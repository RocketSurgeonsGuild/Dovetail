using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;

namespace Dovetail.Configuration.Json;

/// <summary>
///     Json configuration conventions for hosts that embed their configuration files (see <see cref="DovetailEmbeddedConfiguration" />)
/// </summary>
[DovetailExport]
internal class JsonEmbeddedJoint : IConfigurationJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IConfigurationBuilder builder)
    {
        if (context.EmbeddedConfigurationAssembly is not { } assembly) return;

        foreach (var fileName in DovetailEmbeddedConfiguration.GetConventionalFileNames(context, "json"))
        {
            if (DovetailEmbeddedConfiguration.OpenResource(assembly, fileName) is { } stream)
            {
                builder.AddJsonStream(stream);
            }
        }
    }
}
