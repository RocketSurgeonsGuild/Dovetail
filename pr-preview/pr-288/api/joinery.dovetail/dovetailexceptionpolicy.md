# DovetailExceptionPolicy

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.md)
- Version: `0.3.2-pr.4`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

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

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
