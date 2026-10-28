# StashboxJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.stashbox.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Stashbox`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers Stashbox services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.Stashbox;

public class StashboxJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [StashboxJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.stashbox/stashboxjoint/constructors.md#constructor)
