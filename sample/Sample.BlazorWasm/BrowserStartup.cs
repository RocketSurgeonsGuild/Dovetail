using Dovetail.Attributes;
using Dovetail.Joints;
#if NET11_0_OR_GREATER
using Microsoft.Extensions.Hosting;
#endif

namespace Sample.BlazorWasm;

/// <summary>
///     Registers a hosted service through Dovetail, so the browser tests can see that a Dovetail-registered
///     <c>IHostedService</c> runs (Blazor WebAssembly starts hosted services since .NET 11).
/// </summary>
[DovetailExport]
public class BrowserStartupJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services)
    {
        services.AddSingleton<BrowserStartupStatus>();
#if NET11_0_OR_GREATER
        services.AddHostedService<BrowserStartupService>();
#endif
    }
}

public class BrowserStartupStatus
{
    private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Task Started => _started.Task;

    internal void MarkStarted() => _started.TrySetResult();
}

#if NET11_0_OR_GREATER
public class BrowserStartupService(BrowserStartupStatus status) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        status.MarkStarted();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
#endif
