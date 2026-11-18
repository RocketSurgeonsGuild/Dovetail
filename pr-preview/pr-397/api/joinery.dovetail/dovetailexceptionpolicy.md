# DovetailExceptionPolicy

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/DovetailExceptionPolicy.cs)

A policy that determines how to handle exceptions thrown by conventions

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailExceptionPolicy
{
    // ...
}
```

## Properties

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
