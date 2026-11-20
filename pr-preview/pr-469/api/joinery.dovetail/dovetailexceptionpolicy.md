# DovetailExceptionPolicy

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/DovetailExceptionPolicy.cs)

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

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
