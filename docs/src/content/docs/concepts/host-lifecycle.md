---
title: Host Lifecycle
description: Run code when the host is starting, started, stopping and stopped, without writing a hosted service, from an application builder or a service collection.
tags: [hosting, di]
---

# Host Lifecycle

A host that runs `IHostedLifecycleService` (the generic host, and so ASP.NET Core) tells its hosted services when it is about to
start, has started, is about to stop and has stopped. Most applications only want to run a little code at one of those moments —
warm a cache once the host has started, flush a buffer before it stops — and writing a hosted service for that is a lot of
ceremony. `Joinery.Dovetail.Hosting` lets you register the code directly:

```c#
var builder = Host.CreateApplicationBuilder(args);

builder
   .OnHostStarting(provider => Console.WriteLine("Starting"))
   .OnHostStarted(async provider => await provider.GetRequiredService<CacheWarmer>().WarmUpAsync())
   .OnHostStopping(async (provider, cancellationToken) => await provider.GetRequiredService<Outbox>().FlushAsync(cancellationToken))
   .OnHostStopped(provider => Console.WriteLine("Stopped"));

using var host = await builder.ConfigureDovetail();
await host.RunAsync();
```

An `ASP.NET Core` application does the same on its `WebApplicationBuilder`.

## The methods

There are four, one per moment, and each comes in three forms:

| Method           | Runs                                           | `IHostedLifecycleService` method |
| ---------------- | ---------------------------------------------- | -------------------------------- |
| `OnHostStarting` | before the host starts its hosted services     | `StartingAsync`                  |
| `OnHostStarted`  | after the host has started its hosted services | `StartedAsync`                   |
| `OnHostStopping` | before the host stops its hosted services      | `StoppingAsync`                  |
| `OnHostStopped`  | after the host has stopped its hosted services | `StoppedAsync`                   |

| Form         | Handler                                           |
| ------------ | ------------------------------------------------- |
| synchronous  | `Action<IServiceProvider>`                        |
| asynchronous | `Func<IServiceProvider, Task>`                    |
| cancellable  | `Func<IServiceProvider, CancellationToken, Task>` |

The handler gets the host's service provider. The cancellable form also gets the cancellation token the host passes to the
lifecycle method, which follows the token given to `StartAsync` or `StopAsync`.

## From an application builder or a service collection

Every method is an extension of both `IHostApplicationBuilder` (`HostApplicationBuilder`, `WebApplicationBuilder`, ...), which
returns the builder so the calls chain, and `IServiceCollection`, which returns the service collection. A host built the older
way, with an `IHostBuilder`, has no application builder, but it has a service collection:

```c#
using var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(services => services.OnHostStopped(_ => Console.WriteLine("Stopped")))
                .Build();
```

A [joint](./defining-joints) registers its handlers the same way, while the application's services are configured. That is how a
library runs code with the host without the application doing anything:

```c#
[DovetailExport]
public class WarmUpCache : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) =>
        services.OnHostStarted(async (provider, cancellationToken) => await provider.GetRequiredService<ICache>().WarmUpAsync(cancellationToken));
}
```

Handlers have to be registered before the host is built. The first registration adds the hosted service that runs them, so there
is nothing else to call, and an application that registers no handler gets no hosted service.

## Handlers of one moment run in parallel

All the handlers registered for a moment are started together and awaited together (with `Task.WhenAll`), not one after the
other, so one slow handler doesn't hold up the rest. The host moves on to the next step once every one of them has finished.

- **Don't depend on the order** the handlers of one moment run in. The moments themselves are ordered by the host, which calls
  `StartingAsync` on all its hosted lifecycle services, then `StartAsync` on all of them, then `StartedAsync` on all of them (and
  likewise `StoppingAsync`, `StopAsync` and `StoppedAsync` when it stops). The handlers of a moment have all finished before the
  host moves on to the next.
- A **synchronous** handler runs to completion when it is started, so synchronous handlers of one moment, which don't wait for
  anything, happen to run one after another. Handlers that `await` overlap.
- A handler that **throws**, even before its first `await`, fails that step of the host like any hosted service would, and the
  other handlers of the moment have still been started.

## Where it is available

The methods are emitted by `Joinery.Dovetail.Hosting` into the projects that reference the generic host
(`Microsoft.Extensions.Hosting`), the same way it emits its other helpers, and nothing is emitted where the host doesn't run
`IHostedLifecycleService`:

| Host                                    | Methods                                                                                                                                        |
| --------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| Generic host: workers                   | yes                                                                                                                                            |
| ASP.NET Core                            | yes                                                                                                                                            |
| Uno Platform (`Uno.Extensions.Hosting`) | yes: its host is a generic host, so handlers run once the host is started (`NavigateAsync<TShell>()` starts it, `ConfigureDovetail()` doesn't) |
| .NET MAUI                               | no                                                                                                                                             |
| Blazor WebAssembly                      | no                                                                                                                                             |

A .NET MAUI or Blazor WebAssembly project gets none even when `Microsoft.Extensions.Hosting` ends up among its references
(another package brought it along, say): methods that compile but never run their handlers would be worse than methods that
don't exist, so calling one is a compile error.

The methods are `internal`, as are all the helpers `Joinery.Dovetail.Hosting` emits, so a library that registers handlers needs to
reference `Joinery.Dovetail.Hosting` and the generic host itself. Every project gets its own copy of the methods and of the hosted
service, but the registrations they add (`HostLifecycleRegistration`, which is public) are shared, and whichever project registers
first adds the one hosted service that runs them all. A library's handlers therefore run with the application's, once.
