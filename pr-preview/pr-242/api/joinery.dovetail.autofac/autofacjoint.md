# AutofacJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.autofac.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Autofac`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers Autofac services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.Autofac;

public class AutofacJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [AutofacJoint](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.autofac/autofacjoint/constructors.md#constructor)
