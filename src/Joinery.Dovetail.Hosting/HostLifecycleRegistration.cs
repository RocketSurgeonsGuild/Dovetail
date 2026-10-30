namespace Joinery.Dovetail.Hosting;

/// <summary>
///     A handler to run at one <see cref="HostLifecycleMoment" />, added to the service collection by the <c>OnHostStarting</c>,
///     <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c> extension methods that Joinery.Dovetail.Hosting emits into
///     every project that references the generic host (Microsoft.Extensions.Hosting).
/// </summary>
/// <remarks>
///     <para>
///         Applications don't create these. They are public because every assembly that references the generic host gets its own
///         copy of the (internal, embedded) extension methods and of the hosted service that runs the handlers, and the registrations
///         are the one thing those copies must share: a library's <c>OnHostStarted</c> call has to reach the hosted service that the
///         application's copy of the file registered.
///     </para>
///     <para>
///         The handlers of one moment are started together and awaited together, not one after the other.
///     </para>
/// </remarks>
/// <param name="Moment">The moment the <paramref name="Handler" /> runs at.</param>
/// <param name="Handler">
///     The handler. It receives the host's service provider and the cancellation token that the host passes to the lifecycle method.
/// </param>
[PublicAPI]
public sealed record HostLifecycleRegistration(HostLifecycleMoment Moment, Func<IServiceProvider, CancellationToken, Task> Handler);
