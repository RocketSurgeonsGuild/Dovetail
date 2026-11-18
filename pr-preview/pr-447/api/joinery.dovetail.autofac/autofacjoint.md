# AutofacJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.autofac.md)
- Version: `0.7.2-pr.13`
- Namespace: `Joinery.Dovetail.Autofac`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-447-merge.Sha.07e64d25a30d87fc48248bbdc4924e0013be133c/src/Joinery.Dovetail.Autofac/AutofacJoint.cs)
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers Autofac services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.Autofac;

public class AutofacJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [AutofacJoint](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.autofac/autofacjoint/constructors.md#constructor)
