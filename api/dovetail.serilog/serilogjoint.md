# SerilogJoint

- Kind: `class`
- Package: [Dovetail.Serilog](/Dovetail/api/dovetail.serilog.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Serilog`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.Serilog;

public sealed class SerilogJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [SerilogJoint](/Dovetail/api/dovetail.serilog/serilogjoint/constructors.md#constructor)
