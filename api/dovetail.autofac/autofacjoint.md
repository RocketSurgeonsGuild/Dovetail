# AutofacJoint

- Kind: `class`
- Package: [Dovetail.Autofac](/Dovetail/api/dovetail.autofac.md)
- Version: `0.2.3-beta.1`
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

- [AutofacJoint](/Dovetail/api/dovetail.autofac/autofacjoint/constructors.md#constructor)
