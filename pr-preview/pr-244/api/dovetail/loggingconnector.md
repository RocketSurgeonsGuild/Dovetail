# LoggingConnector

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-244/api/dovetail.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailjoint.md), [IServiceAsyncJoint](/Dovetail/pr-preview/pr-244/api/dovetail/iserviceasyncjoint.md)

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed class LoggingConnector
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [LoggingConnector](/Dovetail/pr-preview/pr-244/api/dovetail/loggingconnector/constructors.md#constructor)

## Methods

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-244/api/dovetail/loggingconnector/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` -- Register the Service part asynchronously.
