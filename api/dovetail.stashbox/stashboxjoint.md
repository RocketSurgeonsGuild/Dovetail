# StashboxJoint

- Kind: `class`
- Package: [Dovetail.Stashbox](/Dovetail/api/dovetail.stashbox.md)
- Version: `0.2.2-beta.2`
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

- [StashboxJoint](/Dovetail/api/dovetail.stashbox/stashboxjoint/constructors.md#constructor)
