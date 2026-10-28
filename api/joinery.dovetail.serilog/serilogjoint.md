# SerilogJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/api/joinery.dovetail.serilog.md)
- Version: `0.2.4-beta.6`
- Namespace: `Joinery.Dovetail.Serilog`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

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

## Constructors

- [SerilogJoint](/Dovetail/api/joinery.dovetail.serilog/serilogjoint/constructors.md#constructor)
