# SerilogConfigurationJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/api/joinery.dovetail.serilog.md)
- Version: `0.2.4-beta.14`
- Namespace: `Joinery.Dovetail.Serilog`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceJoint`

Hand-authored stand-in for the generator-emitted `IConfigurationJoint` that `openspec/changes/dovetail-managed-configuration` will eventually emit for this library's `appsettings.json` (see design.md, Decision 4). Registers [SerilogRuntimeConfiguration](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration.md) with Microsoft's `Configure<TOptions>(IConfiguration)`, which binds the `Serilog` section of the context's `Configuration` with the reflection-based configuration binder and observes reloads. The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.

## Definition

```csharp
namespace Joinery.Dovetail.Serilog;

public sealed class SerilogConfigurationJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceJoint
{
    // ...
}
```

## Constructors

- [SerilogConfigurationJoint](/Dovetail/api/joinery.dovetail.serilog/serilogconfigurationjoint/constructors.md#constructor)

## Methods

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/api/joinery.dovetail.serilog/serilogconfigurationjoint/methods.md#register-idovetailcontext-iservicecollection) -- Register the Service part.
