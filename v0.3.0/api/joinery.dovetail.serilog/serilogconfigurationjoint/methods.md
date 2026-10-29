# SerilogConfigurationJoint Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/v0.3.0/api/joinery.dovetail.serilog.md)
- Type: [SerilogConfigurationJoint](/Dovetail/v0.3.0/api/joinery.dovetail.serilog/serilogconfigurationjoint.md)
- Kind: `Methods`
- Members: `1`

Hand-authored stand-in for the generator-emitted `IConfigurationJoint` that `openspec/changes/dovetail-managed-configuration` will eventually emit for this library's `appsettings.json` (see design.md, Decision 4). Registers [SerilogRuntimeConfiguration](/Dovetail/v0.3.0/api/joinery.dovetail.serilog/serilogruntimeconfiguration.md) with Microsoft's `Configure<TOptions>(IConfiguration)`, which binds the `Serilog` section of the context's `Configuration` with the reflection-based configuration binder and observes reloads. The generator emits a reflection-free registration instead, so this handwritten one is retired once it does.

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection"></a>

## Register(IDovetailContext, IServiceCollection)

- Name: `Register(IDovetailContext, IServiceCollection)`

Register the Service part.

```csharp
public sealed class SerilogConfigurationJoint
{
    public void Register(
        IDovetailContext context,
        IServiceCollection services)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
