# OpenTelemetry parity with LaunchPad.Telemetry

# OpenTelemetry parity with LaunchPad.Telemetry

`Rocket.Surgery.LaunchPad.Telemetry` offered six convention abstractions, a default convention that turned OpenTelemetry on, and a
default convention for the resource. `Joinery.Dovetail.OpenTelemetry` (with the joints `Joinery.Dovetail.Hosting` emits) covers all
of that, so an application can drop the LaunchPad package. This page is the written check, made against OpenTelemetry SDK 1.20.1, and
names the test that shows each row. See [OpenTelemetry](./opentelemetry) for how the joints work.

The one LaunchPad.Telemetry convention that has no equivalent here is the instrumentation convention (HTTP client tracing, and HTTP
client, runtime and process metrics). Dovetail's package ships no instrumentation on purpose, so LaunchPad keeps it as a joint of its
own in `LaunchPad.Hosting`
([LaunchPad#3061](https://github.com/RocketSurgeonsGuild/LaunchPad/issues/3061)).

## The six abstractions

| LaunchPad.Telemetry                                                   | Dovetail                                                                               | Registered with                                 | Shown by                                                                                                                              |
| --------------------------------------------------------------------- | -------------------------------------------------------------------------------------- | ----------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| `IOpenTelemetryConvention`, `OpenTelemetryConvention`                 | `IOpenTelemetryJoint`, `OpenTelemetryJointDelegate` (`IOpenTelemetryBuilder`)          | `ConfigureOpenTelemetry`                        | `Base_Joint_Configures_Services_And_Every_Signal`                                                                                     |
| `IOpenTelemetryAsyncConvention`, `OpenTelemetryAsyncConvention`       | `IOpenTelemetryAsyncJoint`, `OpenTelemetryAsyncJointDelegate` (generated)              | `ConfigureOpenTelemetry` with an async delegate | `Async_Delegate_Is_Awaited_With_The_Cancellation_Token_Given_To_ConfigureDovetail`, `Exported_Async_Joint_Is_Discovered_And_Awaited`  |
| `IOpenTelemetryLoggingConvention`, `OpenTelemetryLoggingConvention`   | `ILoggerProviderBuilderJoint`, `LoggerProviderJointDelegate` (`LoggerProviderBuilder`) | `ConfigureLoggerProvider`                       | `Exported_Logger_Joint_Is_Discovered_And_Applied`, `Logs_Are_Exported_Through_The_Logging_Provider`                                   |
| `IOpenTelemetryMetricsConvention`, `OpenTelemetryMetricsConvention`   | `IMeterProviderBuilderJoint`, `MeterProviderJointDelegate` (`MeterProviderBuilder`)    | `ConfigureMeterProvider`                        | `Exported_Meter_Joint_Is_Discovered_And_Applied`                                                                                      |
| `IOpenTelemetryResourceConvention`, `OpenTelemetryResourceConvention` | `IResourceBuilderJoint`, `ResourceBuilderJointDelegate` (`ResourceBuilder`)            | `ConfigureResourceBuilder`                      | `Exported_Resource_Joint_Is_Discovered_And_Applied_To_Every_Provider`                                                                 |
| `IOpenTelemetryTracingConvention`, `OpenTelemetryTracingConvention`   | `ITracerProviderJoint`, `TracerProviderJointDelegate` (`TracerProviderBuilder`)        | `ConfigureTracerProvider`                       | `DovetailExport_Annotated_TracerJoint_Is_Auto_Discovered_Without_Manual_Configure`                                                    |
| `TelemetryConvention` (applies the conventions, adds the logger)      | `DovetailOpenTelemetryJoint` and its siblings, emitted by `Joinery.Dovetail.Hosting`   | nothing: emitted into the application           | `Starting_The_Host_Starts_Traces_And_Metrics_With_The_Resource`, `Starting_The_Web_Application_Starts_Every_Signal_With_The_Resource` |
| `DefaultTelemetryConvention` (resource defaults)                      | nothing: the SDK's default resource                                                    | nothing                                         | [Resource defaults](#resource-defaults)                                                                                               |

How the shape differs, none of it a capability:

- A joint receives the `IDovetailContext` where a convention received the context and an `IConfiguration`. The configuration is
  `context.Configuration` (`Joints_Read_The_Application_Configuration_From_The_Context`).
- Delegates always take the context: write `(_, builder) => ...` where LaunchPad also had `Action<TBuilder>` overloads.
- `ConfigureOpenTelemetryLogging`, `ConfigureOpenTelemetryMetrics` and `ConfigureOpenTelemetryTracing` are `ConfigureLoggerProvider`,
  `ConfigureMeterProvider` and `ConfigureTracerProvider`; the resource one is `ConfigureResourceBuilder`.
- The base joint types and `ConfigureOpenTelemetry` are in `Joinery.Dovetail.Joints`, the narrower ones in
  `Joinery.Dovetail.OpenTelemetry.Joints`. Import both.
- `[ExportConvention]` is `[DovetailExport]`. `ConventionCategory` and `AfterConvention<T>` become the `priority` and `category`
  parameters, and every OpenTelemetry joint, base or narrow, runs in the one priority order.
- LaunchPad's `SerilogTelemetryConvention` (Serilog's OpenTelemetry sink) belongs to the Serilog package, not this one.

## Logs through the logging provider

LaunchPad's `TelemetryConvention` was also an `ILoggingConvention` that called `ILoggingBuilder.AddOpenTelemetry()`, so the
OpenTelemetry provider was always one of the application's logger providers.

Dovetail adds it when something configures logging. Every `ILoggerProviderBuilderJoint` (and `ConfigureLoggerProvider`) runs inside
`IOpenTelemetryBuilder.WithLogging`, which in SDK 1.20.1 does `AddLogging(logging => logging.UseOpenTelemetry(...))`, and so does the
`UseOtlpExporter()` call, which enables logging, metrics and tracing together. An application with neither has no log exporter, so it
had nothing to export. To have the provider regardless, call `WithLogging()` from any OpenTelemetry joint.

`Logs_Are_Exported_Through_The_Logging_Provider` shows the result: the provider is an `OpenTelemetryLoggerProvider` among the host's
`ILoggerProvider`s, and a warning written through `ILogger<T>` reaches the exporter with its category, level, body and structured
state.

Log export keeps working when the [Serilog](./serilog) joint is also applied: Serilog's logger forwards each event to the other logging
providers of the container, the OpenTelemetry one included ([the other logging providers](./serilog#the-other-logging-providers)).

## Resource defaults

Yes: the SDK's default resource already holds what LaunchPad's `DefaultTelemetryConvention` added. `ResourceBuilder.CreateDefault()`
is the default resource, `AddTelemetrySdk()` and `AddEnvironmentVariableDetector()`, and every provider starts from it, so a joint only
adds to it. Dovetail adds no resource defaults of its own.

- `Default_Resource_Has_The_Telemetry_Sdk_Attributes_On_Every_Signal`: with no resource joint, the tracer, meter and logger providers
  all carry `telemetry.sdk.name`, `telemetry.sdk.language` and `telemetry.sdk.version`, and `service.name` is
  `unknown_service:<process>`.
- `Default_Resource_Has_The_Environment_Variable_Detector` and `Default_Resource_Reads_The_Process_Environment`: `OTEL_SERVICE_NAME`
  and `OTEL_RESOURCE_ATTRIBUTES` set the resource, whether they come from the host's configuration (the detector reads that first)
  or from the process environment.
- `LaunchPads_Default_Resource_Convention_Changes_Nothing`: adding `AddTelemetrySdk().AddEnvironmentVariableDetector()` through a
  resource joint produces exactly the same attributes on every provider.
- Blazor WebAssembly's LaunchPad default only added `AddTelemetrySdk()`. The browser has the same default resource
  (`Should_Have_The_Default_Resource_On_Every_Provider`); it has no process environment, so the detector only reads configuration.

To name the service, set `OTEL_SERVICE_NAME` or call `AddService(...)` from a resource joint; LaunchPad's convention only had that
call commented out.

## Hosts

| Host               | Shown by                                                                                                                                                                                                                  |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Generic host       | `OpenTelemetryParityTests` and `OpenTelemetryBridgeTests` in `Joinery.Dovetail.OpenTelemetry.Tests`, on .NET 10 and 11                                                                                                    |
| ASP.NET Core       | `OpenTelemetryAspNetCoreTests` in `Joinery.Dovetail.Hosting.AspNetCore.Tests`: every signal and the resource once the web host starts, the default resource, and on .NET 11 a request traced without tracer configuration |
| Blazor WebAssembly | `BrowserHostingTests` and `BrowserOpenTelemetryTests` in `Joinery.Dovetail.WebAssembly.Hosting.Tests`: traces, metrics and logs exported in a real browser, with the resource on each provider                            |

The browser tests run the sample's `net11.0-browser` build behind the Blazor Gateway, so they run on .NET 11 only.