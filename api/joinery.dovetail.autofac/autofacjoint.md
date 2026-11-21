# AutofacJoint

- Kind: `class`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-beta.2`
- Namespace: `Joinery.Dovetail.Autofac`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.65fca43d4a7154e28dbe9531d628c844781169f5/src/Joinery.Dovetail.Autofac/AutofacJoint.cs)
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

- [AutofacJoint](/Dovetail/api/joinery.dovetail.autofac/autofacjoint/constructors.md#constructor)
