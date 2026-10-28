# Joinery.Dovetail.Serilog

- Package: `Joinery.Dovetail.Serilog`
- Version: `0.2.3-pr.8`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Serilog](https://www.nuget.org/packages/Joinery.Dovetail.Serilog)

## Namespaces

### Joinery.Dovetail.Joints

- [ISerilogAsyncJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/iserilogasyncjoint.md) -- `interface` -- Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [ISerilogJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/iserilogjoint.md) -- `interface` -- Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [SerilogAsyncJointDelegate](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogasyncjointdelegate.md) -- `delegate` -- Async delegate for the Serilog part.
- [SerilogHostBuilderExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/seriloghostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Serilog parts to a `Dovetail.DovetailContextBuilder`.
- [SerilogJointDelegate](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogjointdelegate.md) -- `delegate` -- Delegate for the Serilog part.
- [SerilogJointExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogjointextensions.md) -- `class` `static` -- Extension methods for applying Serilog parts.

### Joinery.Dovetail.Serilog

- [SerilogConfigurationJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogconfigurationjoint.md) -- `class` `sealed` -- Hand-authored stand-in for the generator-emitted `IConfigurationJoint` that `openspec/changes/dovetail-managed-configuration` will eventually emit for this library's `appsettings.json` (see design.md, Decision 4). Registers [SerilogRuntimeConfiguration](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogruntimeconfiguration.md) with Microsoft's `Configure<TOptions>(IConfiguration)`, which binds the `Serilog` section of the context's `Configuration` with the reflection-based configuration binder and observes reloads. The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.
- [SerilogJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogjoint.md) -- `class` `sealed` -- A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [SerilogRuntimeConfiguration](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.serilog/serilogruntimeconfiguration.md) -- `record` `sealed` -- Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.
