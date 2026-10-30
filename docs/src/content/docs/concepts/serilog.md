---
title: Serilog
description: Serilog integration that builds a Serilog logger from Serilog joints and the application's configuration, and registers it in place of the host's default logging providers.
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

1. reads the `Serilog` section of the application's configuration into the logger configuration, if the configuration has one
   ([below](#configuration));
2. removes the logging providers the host adds by default (console, debug, event source and event log — everything that
   `Microsoft.Extensions.Logging` itself ships) and the default `ILoggerFactory`;
3. registers the logger, as `Serilog.Core.Logger` and as Serilog's `ILogger`, and `ILoggerFactory` (a
   `SerilogLoggerFactory`, which is what backs every `ILogger<T>`), as singletons.

So `Serilog.ILogger`, `Microsoft.Extensions.Logging.ILogger<T>` and `ILoggerFactory.CreateLogger(...)` all write to the one
logger, once. The container owns it: disposing the host disposes the logger, which flushes and disposes its sinks.

A few consequences of replacing `ILoggerFactory`:

- The `Logging:LogLevel` filters and `builder.Logging.SetMinimumLevel(...)` belong to the factory that was replaced. Use
  Serilog's levels, `MinimumLevel` and its `Override` entries, instead.
- A logging provider that is not one of the defaults (one from another package, say) stays registered, but nothing reads it:
  events reach it only through a Serilog sink for the same destination, such as `Serilog.Sinks.OpenTelemetry`.
- Dovetail does not set the static `Log.Logger`. Resolve the logger from the container.

## The Serilog joint

`Joinery.Dovetail.Serilog` declares one `<DovetailJoint>`, `Serilog`, for `Serilog.LoggerConfiguration`: `ISerilogJoint`,
`ISerilogAsyncJoint`, the `SerilogJointDelegate` and the `ConfigureSerilog(...)` builder method. A joint is handed the
context, the service collection and the logger configuration:

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
2. the `Serilog` section of the configuration;
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

The `Serilog` section is read by [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration),
so it controls the minimum level, the enrichers and the sinks the way that package documents:

```json
{
    "Serilog": {
        "Using": ["Serilog.Sinks.Console"],
        "MinimumLevel": {
            "Default": "Information",
            "Override": { "Microsoft": "Warning" }
        },
        "Enrich": ["FromLogContext"],
        "Properties": { "Application": "Sample" },
        "WriteTo": [{ "Name": "Console" }]
    }
}
```

The sinks and enrichers are the application's own packages (`Serilog.Sinks.Console` above). Changes to the levels of a
configuration that reloads are applied while the application runs. `Joinery.Dovetail.Serilog` ships the section's JSON Schema, which
is merged into the application's [`appsettings.schema.json`](../guides/editor-setup) for editor completion and validation.

> [!NOTE]
> Serilog.Settings.Configuration skips a sink, enricher or filter method that it cannot find, and reports it to Serilog's
> `SelfLog` rather than throwing. A misspelt name, or one whose package is missing from `Using`, shows up as a logger without it.
> Turn `SelfLog` on while setting it up: `SelfLog.Enable(Console.Error)`.

## Trimming and Native AOT

Everything `Joinery.Dovetail.Serilog` does is reflection free except reading the `Serilog` section.
Serilog.Settings.Configuration finds the sink, enricher and filter methods that the section names (`"WriteTo": ["Console"]`
means `WriteTo.Console(...)`) by reflection, in the assemblies listed under `Using` or, without it, in the packages the
application's `.deps.json` lists whose name contains "Serilog". It then calls them by reflection, converting their arguments
from strings. That package is not annotated for trimming or Native AOT, and its AOT analysis warns (IL3053).

Dovetail therefore reads the section only when the configuration has one. An application that configures its logger in code, with
joints and options, has no `Serilog` section and runs none of that reflection. `Joinery.Dovetail.Serilog` is marked
`IsAotCompatible`: the one call that reads the section is annotated `RequiresUnreferencedCode` and `RequiresDynamicCode` inside the
package, and its warnings are suppressed where that call is guarded by the check for the section. The trim and AOT analyzers of an
application that references the package therefore stay quiet whether or not it has a section, and this page is the contract instead.

An application that does have a `Serilog` section has to keep what the section names alive itself and, when it is published as a
single file or with Native AOT, has to tell Serilog where to find it, because such an application has no `.deps.json` to discover it from:

- Root the assemblies that declare the methods the section names, so that the trimmer keeps them. Without it, the trimmer removes
  an assembly that nothing references, and the application fails at startup, while Dovetail reads the section, with a
  `FileNotFoundException` for the sink's assembly:

    ```xml
    <ItemGroup>
        <TrimmerRootAssembly Include="Serilog.Sinks.Console" />
    </ItemGroup>
    ```

- List those assemblies under `Using`. In a single-file or Native AOT application, discovery from `.deps.json` finds nothing, and
  Serilog.Settings.Configuration throws an `InvalidOperationException` at startup that says so.

| Application                                              | Trimming                                                  | Single file                | Native AOT                                                                    |
| -------------------------------------------------------- | --------------------------------------------------------- | -------------------------- | ----------------------------------------------------------------------------- |
| No `Serilog` section: the logger is set up in code       | Supported                                                 | Supported                  | Supported, as far as Serilog and the sinks used are                           |
| `Serilog` section, sink assemblies rooted and in `Using` | Works                                                     | Works                      | Works for what was tried; not guaranteed, because the reader is not annotated |
| `Serilog` section, sink assemblies not rooted            | Fails at startup: the sink's assembly is gone             | Fails in the same way      | Fails in the same way                                                         |
| `Serilog` section, without `Using`                       | Works for a trimmed app that still ships its `.deps.json` | Fails: `Using` is required | Fails: `Using` is required                                                    |

"Works" was tried by publishing an application that reads a section with `MinimumLevel` and its overrides, `Enrich` with
`FromLogContext`, `Properties` and the `Console` sink with `outputTemplate` and `restrictedToMinimumLevel` arguments (Serilog 4.4,
Serilog.Settings.Configuration 10.0.1, Serilog.Sinks.Console 6.0, .NET 10, `PublishTrimmed`, `PublishSingleFile` and `PublishAot`).
A section can name methods and argument types that nothing else in the application uses, so test the section you ship against the
published application: what is missing is reported by `SelfLog`, or by an exception at startup. Where the output matters in an AOT
application, configure the logger in code and leave the section out.
