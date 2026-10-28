# SerilogConfigurationJoint

- Kind: `class`
- Package: [Dovetail.Serilog](/Dovetail/pr-preview/pr-234/api/dovetail.serilog.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Serilog`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceJoint`

Hand-authored stand-in for the generator-emitted `IConfigurationJoint` that `openspec/changes/dovetail-managed-configuration` will eventually emit for this library's `appsettings.json` (see design.md, Decision 4). Registers [SerilogRuntimeConfiguration](/Dovetail/pr-preview/pr-234/api/dovetail.serilog/serilogruntimeconfiguration.md) with Microsoft's `Configure<TOptions>(IConfiguration)`, which binds the `Serilog` section of the context's `Configuration` with the reflection-based configuration binder and observes reloads. The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.

## Definition

```csharp
namespace Dovetail.Serilog;

public sealed class SerilogConfigurationJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceJoint
{
    // ...
}
```

## Constructors

- [SerilogConfigurationJoint](/Dovetail/pr-preview/pr-234/api/dovetail.serilog/serilogconfigurationjoint/constructors.md#constructor)

## Methods

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/pr-preview/pr-234/api/dovetail.serilog/serilogconfigurationjoint/methods.md#register-idovetailcontext-iservicecollection) -- Register the Service part.
