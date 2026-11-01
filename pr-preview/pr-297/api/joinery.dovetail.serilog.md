# Joinery.Dovetail.Serilog

- Package: `Joinery.Dovetail.Serilog`
- Version: `0.4.2-pr.2`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Serilog](https://www.nuget.org/packages/Joinery.Dovetail.Serilog)

## Namespaces

### Joinery.Dovetail.Joints

- [ISerilogAsyncJoint](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/iserilogasyncjoint.md) -- `interface` -- Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [ISerilogJoint](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/iserilogjoint.md) -- `interface` -- Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [SerilogAsyncJointDelegate](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/serilogasyncjointdelegate.md) -- `delegate` -- Async delegate for the Serilog part.
- [SerilogHostBuilderExtensions](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/seriloghostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Serilog parts to a `Dovetail.DovetailContextBuilder`.
- [SerilogJointDelegate](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/serilogjointdelegate.md) -- `delegate` -- Delegate for the Serilog part.
- [SerilogJointExtensions](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/serilogjointextensions.md) -- `class` `static` -- Extension methods for applying Serilog parts.

### Joinery.Dovetail.Serilog

- [SerilogJoint](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.serilog/serilogjoint.md) -- `class` `sealed` -- A joint that applies the Serilog joints and registers the Serilog logger they configure: the logger, Serilog's `ILogger` and the `ILoggerFactory` (which backs every `ILogger<T>`) take the place of the logging providers the host adds by default. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
