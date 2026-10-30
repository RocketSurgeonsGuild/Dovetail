# SerilogJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.serilog.md)
- Version: `0.3.1-pr.5`
- Namespace: `Joinery.Dovetail.Serilog`
- Target framework: `net10.0`
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

Once every Serilog joint has run, the `Serilog` section of the application's configuration is read (if it has one) and, when the logger is first resolved, the `IConfigureOptions<LoggerConfiguration>` and `IPostConfigureOptions<LoggerConfiguration>` registered in the services are applied to the logger configuration.

## Constructors

- [SerilogJoint](/Dovetail/pr-preview/pr-270/api/joinery.dovetail.serilog/serilogjoint/constructors.md#constructor)
