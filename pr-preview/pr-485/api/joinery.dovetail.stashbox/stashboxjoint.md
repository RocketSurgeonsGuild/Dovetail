# StashboxJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-pr.10`
- Namespace: `Joinery.Dovetail.Stashbox`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.Stashbox/StashboxJoint.cs)
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

- [StashboxJoint](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.stashbox/stashboxjoint/constructors.md#constructor)
