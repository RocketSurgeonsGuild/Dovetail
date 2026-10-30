# DryIocJoint

- Kind: `class`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.dryioc.md)
- Version: `0.3.1-pr.9`
- Namespace: `Joinery.Dovetail.DryIoc`
- Target framework: `net10.0`
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

- [DryIocJoint](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.dryioc/dryiocjoint/constructors.md#constructor)
