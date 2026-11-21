# LoggingConnector

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/Infrastructure/LoggingJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/idovetailjoint.md), [IServiceAsyncJoint](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/iserviceasyncjoint.md)

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

- [LoggingConnector](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/loggingconnector/constructors.md#constructor)

## Methods

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/loggingconnector/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` -- Register the Service part asynchronously.
