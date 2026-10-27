# ServiceJointExtensions

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-229/api/dovetail.md)
- Version: `0.2.1-pr.18`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`

Extension methods for applying Service parts.

## Definition

```csharp
namespace Dovetail.Joints;

public static class ServiceJointExtensions
{
    // ...
}
```

## Methods

- [ApplyService(IServiceCollection, IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-229/api/dovetail/servicejointextensions/methods.md#applyservice-iservicecollection-idovetailcontext-cancellationtoken) : `ValueTask` `extension` -- Applies the registered Service parts.
