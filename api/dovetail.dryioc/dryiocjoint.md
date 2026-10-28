# DryIocJoint

- Kind: `class`
- Package: [Dovetail.DryIoc](/Dovetail/api/dovetail.dryioc.md)
- Version: `0.2.3-beta.1`
- Namespace: `Dovetail.DryIoc`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers DryIoc services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.DryIoc;

public class DryIocJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [DryIocJoint](/Dovetail/api/dovetail.dryioc/dryiocjoint/constructors.md#constructor)
