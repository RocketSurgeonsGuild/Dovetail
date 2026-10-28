# StashboxJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.stashbox.md)
- Version: `0.2.3-pr.7`
- Namespace: `Joinery.Dovetail.Stashbox`
- Target framework: `net10.0`
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

- [StashboxJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.stashbox/stashboxjoint/constructors.md#constructor)
