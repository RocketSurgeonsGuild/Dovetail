using Stashbox.Configuration;

namespace Joinery.Dovetail.Stashbox;

/// <summary>
///     Options for building the Stashbox Container
/// </summary>
[PublicAPI]
public class StashboxOptions
{
    /// <summary>
    ///     Configures the container (registration behavior, disposable transient tracking, etc.).  Applied when the container is created and again once the
    ///     service collection has been registered, so these settings win over the defaults set by the Microsoft.Extensions.DependencyInjection adapter.
    /// </summary>
    public Action<ContainerConfigurator>? ConfigureContainer { get; set; }

    /// <summary>
    ///     Validates the whole container once the service provider has been built (defaults to false)
    /// </summary>
    public bool ValidateOnBuild { get; set; }

    /// <summary>
    ///     Enables lifetime validation when the environment is Development (defaults to true)
    /// </summary>
    public bool ValidateLifetimesInDevelopment { get; set; } = true;
}
