using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.Notifications;

/// <summary>
///     Exported to every host that references this library. <see cref="NotificationsOptions" /> needs no registration here:
///     Dovetail's generated configuration joint binds it from <c>SdkSample.Notifications.yaml</c>.
/// </summary>
[DovetailExport]
public sealed class NotificationsServiceJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<INotifier, Notifier>();
}
