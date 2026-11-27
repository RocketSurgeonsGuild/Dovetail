# DryIocJoint

- Kind: `class`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-beta.21`
- Namespace: `Joinery.Dovetail.DryIoc`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.f0c56e1dcadc9219196cd48facce90770bb6bd05/src/Joinery.Dovetail.DryIoc/DryIocJoint.cs)
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers DryIoc services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.DryIoc;

public class DryIocJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [DryIocJoint](/Dovetail/api/joinery.dovetail.dryioc/dryiocjoint/constructors.md#constructor)
