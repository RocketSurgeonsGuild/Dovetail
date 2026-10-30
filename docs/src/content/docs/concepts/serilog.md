---
title: Serilog
description: Serilog integration that builds a Serilog logger from Serilog joints and the application's configuration, and registers it in place of the host's default logging providers. Trimming and Native AOT safe.
tags: [logging, configuration, aot]
---

# Serilog

`Joinery.Dovetail.Serilog` makes Serilog the logging of an application, and exposes Serilog's `LoggerConfiguration` as an
ordinary Dovetail [joint](./defining-joints). Any library can contribute a sink, an enricher or a destructuring policy
without taking a dependency on how the application builds its logger.

## Turning it on

Reference `Joinery.Dovetail.Serilog`. Its `SerilogJoint` is exported to the application like any other joint, so there is
nothing to call: every host that calls `ConfigureDovetail` and references it gets a Serilog logger. With
`Joinery.Dovetail.Sdk`, set `DovetailEnableSerilog` to `true` and every project that references `Serilog` gets the package.

## What it does

Once every Serilog joint has run, `SerilogJoint`:

1. applies the minimum level and the level overrides in the `Serilog:MinimumLevel` section of the application's configuration
   to the logger configuration ([below](#configuration));
2. removes the logging providers the host adds by default (console, debug, event source and event log — everything that
   `Microsoft.Extensions.Logging` itself ships) and the default `ILoggerFactory`;
3. registers the logger, as `Serilog.Core.Logger` and as Serilog's `ILogger`, and `ILoggerFactory` (a
   `SerilogLoggerFactory`, which is what backs every `ILogger<T>`), as singletons. The logger also writes to the other logging
   providers of the container ([below](#the-other-logging-providers)).

So `Serilog.ILogger`, `Microsoft.Extensions.Logging.ILogger<T>` and `ILoggerFactory.CreateLogger(...)` all write to the one
logger, once. The container owns it: disposing the host disposes the logger, which flushes and disposes its sinks.

A few consequences of replacing `ILoggerFactory`:

- The `Logging:LogLevel` filters and `builder.Logging.SetMinimumLevel(...)` belong to the factory that was replaced. Use
  Serilog's levels, `Serilog:MinimumLevel` and its `Override` entries, instead.
- Serilog's levels decide what every logging provider receives, the other providers included. A provider that used to be
  given `Debug` events by its own filter gets them only if the `Serilog:MinimumLevel` lets them through.
- Dovetail does not set the static `Log.Logger`. Resolve the logger from the container.

### The other logging providers

Replacing `ILoggerFactory` would leave every other `ILoggerProvider` of the container without events: the default factory is what
writes to them. OpenTelemetry's is one, since its logging joints (`ConfigureLoggerProvider`) add the `OpenTelemetryLoggerProvider`
that exports log records, and so is any provider an application or a package registers. So the logger forwards each event it writes
to them, the way Serilog.Extensions.Hosting does with `writeToProviders`: a `WriteTo.Providers(...)` sink over a
`LoggerProviderCollection`, which the logger factory fills with the container's `ILoggerProvider`s when it is created. A provider
registered at any point before the container is built is included, by a Serilog joint or after it, with the logging builder
(`builder.Logging.AddProvider(...)`) or with the services. The events keep their level, category (the `SourceContext`) and exception, and
the Serilog properties are the state they are logged with.

Each event is written once to every sink and once to every provider. Two kinds of provider are not written to:

- Serilog's own `SerilogLoggerProvider`, which would write the event to Serilog again (and, if it wraps this very logger, without end).
- The default providers that Serilog replaces, which are the ones in the `Microsoft.Extensions.Logging` namespaces (console, debug,
  event source, event log). They are removed from the services; one that is registered after the Serilog joint ran stays out of the
  forwarding too, so that the events are not written to the console twice.

The logger factory is created when the container is asked for it, which a host does while it is being built. Until then the other
providers are not known: an event written through `Serilog.ILogger` before that reaches the sinks only. The logger disposes the providers it writes to when it is disposed, as the container does, so they are disposed twice, which
`Dispose` allows.

## The Serilog joint

`Joinery.Dovetail.Serilog` declares one `<DovetailJoint>`, `Serilog`, for `Serilog.LoggerConfiguration`: `ISerilogJoint`,
`ISerilogAsyncJoint`, the `SerilogJointDelegate` and the `ConfigureSerilog(...)` builder method. A joint is handed the
context, the service collection and the logger configuration. Sinks and enrichers are configured here, in code:

```c#
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MyLibrary;

[DovetailExport]
public class MyLibraryLogging : ISerilogJoint
{
    public void Register(IDovetailContext context, IServiceCollection services, LoggerConfiguration loggerConfiguration) =>
        loggerConfiguration.Enrich.WithProperty("Library", "MyLibrary");
}
```

or, for one application, without a class:

```c#
var host = await Host
                .CreateApplicationBuilder(args)
                .ConfigureDovetail(dovetail => dovetail.ConfigureSerilog((_, _, logger) => logger.WriteTo.Console()));
```

## Configuring the logger

The logger configuration is built in this order, and what comes later wins where only one value can exist (the minimum level):

1. the Serilog joints, in the executor's usual priority order;
2. the `Serilog:MinimumLevel` section of the configuration;
3. the `IConfigureOptions<LoggerConfiguration>` registered in the services (`services.Configure<LoggerConfiguration>(...)`);
4. the `IPostConfigureOptions<LoggerConfiguration>` registered in the services (`services.PostConfigure<LoggerConfiguration>(...)`).

The options are applied the way the options pattern applies them, to the unnamed options, when the logger is first resolved
(which a host does while it is being built). That is what lets them use other services:

```c#
services
   .AddOptions<LoggerConfiguration>()
   .Configure<IHostEnvironment>((logger, environment) => logger.Enrich.WithProperty("Environment", environment.EnvironmentName));
```

### Configuration

The application's configuration sets the minimum level of the logger and of each source, in the `Serilog:MinimumLevel`
section, in the same format as Serilog.Settings.Configuration:

```json
{
    "Serilog": {
        "MinimumLevel": {
            "Default": "Information",
            "Override": { "Microsoft": "Warning", "Microsoft.Hosting.Lifetime": "Information" }
        }
    }
}
```

`"MinimumLevel": "Debug"` is the short form of `"Default": "Debug"`. A level is the name of one of Serilog's levels (`Verbose`,
`Debug`, `Information`, `Warning`, `Error`, `Fatal`; casing is ignored), and anything else fails while Dovetail builds the host,
with an exception that names the key.

Levels follow the configuration when it reloads (a file provider with `reloadOnChange`, say). Only the levels that were in the
configuration when the application started do: an override that is added later is not picked up, and a level that is removed or not a
level any more leaves the level in effect as it is.

That is all that is read from the `Serilog` section. In particular, **sinks, enrichers, filters and `Using` are not read from
configuration**: Serilog.Settings.Configuration finds the methods they name by reflection, which is neither trimming nor Native AOT
safe, so `Joinery.Dovetail.Serilog` does not depend on it. A `"WriteTo"` or `"Enrich"` in the section is ignored, and the schema flags
it (below). Configure them in a [Serilog joint](#the-serilog-joint) instead, in code.

`Joinery.Dovetail.Serilog` ships a JSON Schema for the section, which is merged into the application's
[`appsettings.schema.json`](../guides/editor-setup) for editor completion and validation. It describes exactly what is read:
`MinimumLevel`, as a level name or as an object with `Default` and `Override`. Any other key of the `Serilog` section is flagged, so
a `WriteTo` copied from Serilog's documentation does not look as if it were working.

## Trimming and Native AOT

`Joinery.Dovetail.Serilog` is trimming and Native AOT compatible: the project sets `IsAotCompatible`, builds with the trim and AOT
analyzers on without a warning, and has no reflection to annotate or suppress. Reading the configuration uses only `IConfiguration`
and enum parsing (`Enum.TryParse<LogEventLevel>`), and the rest is registration of the logger, which is Serilog's, and of
`Microsoft.Extensions.Logging`'s own open generic `ILogger<T>`.

Forwarding to the other logging providers is not reflection either: it is Serilog.Extensions.Logging's `WriteTo.Providers` sink and
`SerilogLoggerFactory`, and a type check on the providers.

The claim was checked by publishing a generic-host application that references `Joinery.Dovetail.Serilog`, reads
`Serilog:MinimumLevel:Default` and `Override` from configuration, configures a sink in a `ConfigureSerilog(...)` joint and registers an
`ILoggerProvider` in its services, with `PublishTrimmed` (`TrimMode=full`) and with `PublishAot` (.NET 10, osx-arm64,
self-contained). Neither publish reported a trim or AOT warning, and both applications ran: the factory was a `SerilogLoggerFactory`,
`ILogger<T>` wrote through the sink and to the provider, the default level and the override applied to both, and no default logging
provider was left.

What a published application depends on is therefore Serilog and the sinks and enrichers it configures in its joints, which are
referenced from code and so are kept by the trimmer and compiled by the AOT compiler like any other code. Check the ones you use:
Serilog and its sinks are not Dovetail's.
