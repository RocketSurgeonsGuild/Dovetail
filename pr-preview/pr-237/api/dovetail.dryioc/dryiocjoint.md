# DryIocJoint

- Kind: `class`
- Package: [Dovetail.DryIoc](/Dovetail/pr-preview/pr-237/api/dovetail.dryioc.md)
- Version: `0.2.2-pr.3`
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

- [DryIocJoint](/Dovetail/pr-preview/pr-237/api/dovetail.dryioc/dryiocjoint/constructors.md#constructor)
