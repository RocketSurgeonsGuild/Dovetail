# DovetailExceptionPolicy

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail/DovetailExceptionPolicy.cs)

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

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
