# HostLifecycleMoment

- Kind: `enum`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting.md)
- Version: `0.3.1-pr.5`
- Namespace: `Joinery.Dovetail.Hosting`
- Target framework: `net10.0`

The moments in a host's life at which Dovetail runs the handlers an application registered with `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped`. They are the four phases of `Microsoft.Extensions.Hosting.IHostedLifecycleService`, for the hosts that run it (the generic host and ASP.NET Core).

## Definition

```csharp
namespace Joinery.Dovetail.Hosting;

public enum HostLifecycleMoment
```

## Enum Members

| Name | Value | Description |
| --- | --- | --- |
| Starting | 0 | Before the host starts its hosted services (IHostedLifecycleService.StartingAsync). |
| Started | 1 | After the host started its hosted services (IHostedLifecycleService.StartedAsync). |
| Stopping | 2 | Before the host stops its hosted services (IHostedLifecycleService.StoppingAsync). |
| Stopped | 3 | After the host stopped its hosted services (IHostedLifecycleService.StoppedAsync). |
