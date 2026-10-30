namespace Joinery.Dovetail.Hosting;

/// <summary>
///     The moments in a host's life at which Dovetail runs the handlers an application registered with <c>OnHostStarting</c>,
///     <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c>. They are the four phases of
///     <c>Microsoft.Extensions.Hosting.IHostedLifecycleService</c>, for the hosts that run it (the generic host and ASP.NET Core).
/// </summary>
[PublicAPI]
public enum HostLifecycleMoment
{
    /// <summary>
    ///     Before the host starts its hosted services (<c>IHostedLifecycleService.StartingAsync</c>).
    /// </summary>
    Starting,

    /// <summary>
    ///     After the host started its hosted services (<c>IHostedLifecycleService.StartedAsync</c>).
    /// </summary>
    Started,

    /// <summary>
    ///     Before the host stops its hosted services (<c>IHostedLifecycleService.StoppingAsync</c>).
    /// </summary>
    Stopping,

    /// <summary>
    ///     After the host stopped its hosted services (<c>IHostedLifecycleService.StoppedAsync</c>).
    /// </summary>
    Stopped,
}
