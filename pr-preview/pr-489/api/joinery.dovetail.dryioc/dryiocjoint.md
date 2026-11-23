# DryIocJoint

- Kind: `class`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail.DryIoc`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail.DryIoc/DryIocJoint.cs)
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

- [DryIocJoint](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.dryioc/dryiocjoint/constructors.md#constructor)
