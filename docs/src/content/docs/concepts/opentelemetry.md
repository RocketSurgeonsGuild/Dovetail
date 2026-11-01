---
title: OpenTelemetry
description: OpenTelemetry integration that exposes the OpenTelemetry builder, resource, and tracer, meter and logger providers as Dovetail joints, turned on for the application's host.
tags: [opentelemetry, observability, tracing, metrics, logging]
---

# OpenTelemetry

`Joinery.Dovetail.OpenTelemetry` exposes OpenTelemetry's configuration surface — the top-level `IOpenTelemetryBuilder`, the
resource builder, and the tracer, meter and logger provider builders — as ordinary Dovetail [joints](./defining-joints). Any
library can contribute an activity source, a meter, a log processor, or a resource attribute without taking a direct dependency on
how the host wires up OpenTelemetry. Coming from `LaunchPad.Telemetry`? [Its parity check](./opentelemetry-launchpad-parity) maps each of
its conventions to a joint here.

## Turning it on

An application turns OpenTelemetry on by referencing `Joinery.Dovetail.OpenTelemetry` along with `Joinery.Dovetail.Hosting`.
Joinery.Dovetail.Sdk adds `Joinery.Dovetail.OpenTelemetry` to every project that references `OpenTelemetry` or
`OpenTelemetry.Extensions.Hosting`, and to every file-based app (`dotnet run app.cs`) along with `OpenTelemetry.Extensions.Hosting`;
set `DovetailEnableOpenTelemetry` to `false` to leave OpenTelemetry to the application.

`Joinery.Dovetail.Hosting` then emits joints into the application that add OpenTelemetry to its services, apply every
OpenTelemetry joint, and start it. OpenTelemetry only listens to activity sources and meters once its providers are resolved,
which `OpenTelemetry.Extensions.Hosting` does from a hosted service, so what an application needs depends on whether its host runs
hosted services:

| Host                                    | Runs hosted services                                                                   | Needs `OpenTelemetry.Extensions.Hosting` | Started by                                                                                                                    |
| --------------------------------------- | -------------------------------------------------------------------------------------- | ---------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| Generic host: workers, ASP.NET Core     | yes                                                                                    | yes                                      | its hosted service                                                                                                            |
| Uno Platform (`Uno.Extensions.Hosting`) | once started: `NavigateAsync<TShell>()` starts the host, `ConfigureDovetail()` doesn't | no                                       | `DovetailOpenTelemetryUnoStartJoint`, once the host is built                                                                  |
| .NET MAUI (`MauiAppBuilder`)            | no                                                                                     | no                                       | `DovetailOpenTelemetryMauiStartJoint`, once the app is built                                                                  |
| Blazor WebAssembly                      | from .NET 11                                                                           | no                                       | `DovetailOpenTelemetryWebAssemblyStartJoint`, once the host is built (and on .NET 11 the hosted service too, when referenced) |

- With `OpenTelemetry.Extensions.Hosting`, whatever the host, `DovetailOpenTelemetryJoint` calls `services.AddOpenTelemetry()`. On
  .NET 11 it also traces ASP.NET Core's `Microsoft.AspNetCore` activity source.
- Without it, `DovetailOpenTelemetryServicesJoint` adds OpenTelemetry to the services directly, for the hosts that don't need it.
- A start joint resolves the meter, tracer and logger providers right after the host is built, as the hosted service does when the
  host starts, so OpenTelemetry runs on those hosts without a package the application wouldn't otherwise use.
- Only an application whose hosts all need `OpenTelemetry.Extensions.Hosting` (the generic host, with no Uno, MAUI or WebAssembly
  host) and that doesn't reference it fails to compile, with an `#error` (CS1029) that says to add it. Packages that build on it,
  such as the Azure Monitor distro or .NET Aspire's service defaults, bring it along, so they count.

OpenTelemetry runs in the browser too: in single-threaded Blazor WebAssembly its batch export processor and periodic metric reader
run on tasks instead of threads.

Only applications and test projects get the joint, as an internal class: a library's copy would be exported to every application
that references it. A test project that imports its application's joints gets the application's copy as well as its own, and only
the first one to run applies OpenTelemetry.

Every `[DovetailExport]` joint, and anything registered with a `DovetailContextBuilder` method such as
`ConfigureTracerProvider(...)`, is then applied with nothing else to call:

```c#
var host = await Host
                .CreateApplicationBuilder(args)
                .ConfigureDovetail(dovetail => dovetail.ConfigureTracerProvider((_, tracing) => tracing.AddConsoleExporter()));
```

## The joints

`Joinery.Dovetail.OpenTelemetry` declares one [`<DovetailJoint>`](../../reference/dovetail-joint/), `OpenTelemetry`, for `OpenTelemetry.IOpenTelemetryBuilder`:
`IOpenTelemetryJoint`, its `OpenTelemetryJointDelegate`, the `ConfigureOpenTelemetry(...)` builder method and the
`ApplyOpenTelemetry` extension that the emitted joint calls. Narrower interfaces derive from `IOpenTelemetryJoint`, each for one
of OpenTelemetry's builders:

| Joint interface               | Builder                                      | `DovetailContextBuilder` method |
| ----------------------------- | -------------------------------------------- | ------------------------------- |
| `IOpenTelemetryJoint`         | `OpenTelemetry.IOpenTelemetryBuilder`        | `ConfigureOpenTelemetry`        |
| `IResourceBuilderJoint`       | `OpenTelemetry.Resources.ResourceBuilder`    | `ConfigureResourceBuilder`      |
| `ITracerProviderJoint`        | `OpenTelemetry.Trace.TracerProviderBuilder`  | `ConfigureTracerProvider`       |
| `IMeterProviderBuilderJoint`  | `OpenTelemetry.Metrics.MeterProviderBuilder` | `ConfigureMeterProvider`        |
| `ILoggerProviderBuilderJoint` | `OpenTelemetry.Logs.LoggerProviderBuilder`   | `ConfigureLoggerProvider`       |

Since they're all OpenTelemetry joints, they run together in the executor's usual priority order. A library that only wants to
register an activity source implements `ITracerProviderJoint` without knowing anything about `WithMetrics`, `WithLogging` or the
resource — the same narrow, composable extension point the rest of Dovetail follows.

## Example: contributing an activity source

A library adds a `System.Diagnostics.ActivitySource` to tracing by implementing `ITracerProviderJoint` and marking the class
`[DovetailExport]`, the same pattern used for any other discovered Dovetail joint:

```c#
using System.Diagnostics;
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.OpenTelemetry.Joints;
using OpenTelemetry.Trace;

namespace MyLibrary;

[DovetailExport]
public class MyLibraryTracing : ITracerProviderJoint
{
    public static readonly ActivitySource ActivitySource = new("MyLibrary");

    public void Register(IDovetailContext context, TracerProviderBuilder builder) =>
        builder.AddSource(ActivitySource.Name);
}
```

## What this package does _not_ do

`Joinery.Dovetail.OpenTelemetry` only opens the extension points — it does not ship any exporters or instrumentation. In
particular, it does **not** include:

- The console exporter, the OTLP exporter, or any other exporter package.
- ASP.NET Core, HttpClient, or other instrumentation packages.
- Any default sampler, resource attribute, or processor configuration of its own. The OpenTelemetry SDK's default resource still
  applies, and already has the telemetry SDK attributes and the environment variable detector.

Consumers add exporters and instrumentation the same way they add anything else to a provider: by writing their own
`ITracerProviderJoint`/`IMeterProviderBuilderJoint`/`ILoggerProviderBuilderJoint` implementation (or a plain
`Configure{Kind}(...)` delegate) that calls the relevant `AddConsoleExporter()`, `AddOtlpExporter()`,
`AddAspNetCoreInstrumentation()`, etc. This keeps `Joinery.Dovetail.OpenTelemetry` itself free of exporter dependencies, which also
keeps it friendly to trimming/AOT — see
[Non-Goals in the OpenTelemetry design doc](https://github.com/RocketSurgeonsGuild/Dovetail/blob/main/openspec/changes/add-dovetail-opentelemetry/design.md#goals--non-goals)
for the full rationale.
