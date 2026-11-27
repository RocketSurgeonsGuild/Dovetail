# AutofacJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-pr.19`
- Namespace: `Joinery.Dovetail.Autofac`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail.Autofac/AutofacJoint.cs)
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

- [AutofacJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofacjoint/constructors.md#constructor)
