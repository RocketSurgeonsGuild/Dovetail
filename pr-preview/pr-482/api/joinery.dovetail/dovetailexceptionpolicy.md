# DovetailExceptionPolicy

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/DovetailExceptionPolicy.cs)

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

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
