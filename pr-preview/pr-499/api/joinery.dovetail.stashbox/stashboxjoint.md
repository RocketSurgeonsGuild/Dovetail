# StashboxJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Stashbox`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail.Stashbox/StashboxJoint.cs)
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers Stashbox services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.Stashbox;

public class StashboxJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [StashboxJoint](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.stashbox/stashboxjoint/constructors.md#constructor)
