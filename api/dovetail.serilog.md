# Dovetail.Serilog

- Package: `Dovetail.Serilog`
- Version: `0.2.2-beta.1`
- Target framework: `net10.0`
- NuGet: [Dovetail.Serilog](https://www.nuget.org/packages/Dovetail.Serilog)

## Namespaces

### Dovetail.Joints

- [ISerilogAsyncJoint](/Dovetail/api/dovetail.serilog/iserilogasyncjoint.md) -- `interface` -- Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [ISerilogJoint](/Dovetail/api/dovetail.serilog/iserilogjoint.md) -- `interface` -- Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`
- [SerilogAsyncJointDelegate](/Dovetail/api/dovetail.serilog/serilogasyncjointdelegate.md) -- `delegate` -- Async delegate for the Serilog part.
- [SerilogHostBuilderExtensions](/Dovetail/api/dovetail.serilog/seriloghostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Serilog parts to a `DovetailContextBuilder`.
- [SerilogJointDelegate](/Dovetail/api/dovetail.serilog/serilogjointdelegate.md) -- `delegate` -- Delegate for the Serilog part.
- [SerilogJointExtensions](/Dovetail/api/dovetail.serilog/serilogjointextensions.md) -- `class` `static` -- Extension methods for applying Serilog parts.

### Dovetail.Serilog

- [SerilogConfigurationJoint](/Dovetail/api/dovetail.serilog/serilogconfigurationjoint.md) -- `class` `sealed` -- Hand-authored stand-in for the generator-emitted `IConfigurationJoint` that `openspec/changes/dovetail-managed-configuration` will eventually emit for this library's `appsettings.json` (see design.md, Decision 4). Registers [SerilogRuntimeConfiguration](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md) with Microsoft's `Configure<TOptions>(IConfiguration)`, which binds the `Serilog` section of the context's `Configuration` with the reflection-based configuration binder and observes reloads. The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.
- [SerilogJoint](/Dovetail/api/dovetail.serilog/serilogjoint.md) -- `class` `sealed` -- A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [SerilogRuntimeConfiguration](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md) -- `record` `sealed` -- Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.
