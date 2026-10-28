# DovetailExceptionPolicy

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail`
- Target framework: `net10.0`

A policy that determines how to handle exceptions thrown by conventions

## Definition

```csharp
namespace Dovetail;

public static class DovetailExceptionPolicy
{
    // ...
}
```

## Properties

- [IgnoreNotSupported](/Dovetail/pr-preview/pr-240/api/dovetail/dovetailexceptionpolicy/properties.md#ignorenotsupported) : [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-240/api/dovetail/dovetailexceptionpolicydelegate.md) `static` `get` -- A policy that ignores `NotSupportedException` exceptions
