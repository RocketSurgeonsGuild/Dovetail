using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.Storage;

/// <summary>
///     Exported to every host that references this library. <see cref="StorageOptions" /> and <see cref="ArchiveOptions" />
///     need no registration here: Dovetail's generated configuration joint creates them from <c>SdkSample.Storage.yaml</c>.
/// </summary>
[DovetailExport]
public sealed class StorageServiceJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<IStore, Store>();
}
