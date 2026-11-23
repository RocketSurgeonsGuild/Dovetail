# SerilogJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.serilog.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail.Serilog`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail.Serilog/SerilogJoint.cs)
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that applies the Serilog joints and registers the Serilog logger they configure: the logger, Serilog's `ILogger` and the `ILoggerFactory` (which backs every `ILogger<T>`) take the place of the logging providers the host adds by default. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.Serilog;

public sealed class SerilogJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Remarks

Once every Serilog joint has run, the minimum level and the level overrides in the `Serilog:MinimumLevel` section of the application's configuration are applied (sinks and enrichers are configured by the Serilog joints) and, when the logger is first resolved, the `IConfigureOptions<LoggerConfiguration>` and `IPostConfigureOptions<LoggerConfiguration>` registered in the services are applied to the logger configuration. The logger writes every event to the other `ILoggerProvider` s of the container as well (OpenTelemetry's, say), except Serilog's own and the default providers it replaces.

## Constructors

- [SerilogJoint](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.serilog/serilogjoint/constructors.md#constructor)
