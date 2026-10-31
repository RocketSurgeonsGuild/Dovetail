# LoggingConnector

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.md)
- Version: `0.3.2-pr.4`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/idovetailjoint.md), [IServiceAsyncJoint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/iserviceasyncjoint.md)

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed class LoggingConnector
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [LoggingConnector](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/loggingconnector/constructors.md#constructor)

## Methods

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/loggingconnector/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` -- Register the Service part asynchronously.
