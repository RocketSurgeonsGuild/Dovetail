# SerilogRuntimeConfiguration

- Kind: `record`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/api/joinery.dovetail.serilog.md)
- Version: `0.2.4-beta.10`
- Namespace: `Joinery.Dovetail.Serilog`
- Target framework: `net10.0`
- Implements: `IEquatable<SerilogRuntimeConfiguration>`

Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.

## Definition

```csharp
namespace Joinery.Dovetail.Serilog;

public sealed record SerilogRuntimeConfiguration
    : System.IEquatable<Joinery.Dovetail.Serilog.SerilogRuntimeConfiguration>
{
    // ...
}
```

## Constructors

- [SerilogRuntimeConfiguration](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/constructors.md#constructor)

## Properties

- [MinimumLevel](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/properties.md#minimumlevel) : `string` `get; set` -- The minimum Serilog level to emit, e.g. `"Information"`, `"Debug"`, `"Warning"`.
- [WriteToConsole](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/properties.md#writetoconsole) : `bool` `get; set` -- Whether the console sink should be enabled.

## Methods

- [<Clone>$](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#clone) : [SerilogRuntimeConfiguration](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration.md)
- [Equals(object?)](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [Equals(SerilogRuntimeConfiguration?)](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#equals-serilogruntimeconfiguration) : `bool` -- Indicates whether the current object is equal to another object of the same type.
- [GetHashCode](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Equality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#op-equality-serilogruntimeconfiguration-serilogruntimeconfiguration) : `bool` `static`
- [op_Inequality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#op-inequality-serilogruntimeconfiguration-serilogruntimeconfiguration) : `bool` `static`
- [ToString](/Dovetail/api/joinery.dovetail.serilog/serilogruntimeconfiguration/methods.md#tostring) : `string` -- Returns a string that represents the current object.
