# LoggingAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-239/api/dovetail.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Logging part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask LoggingAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Logging.ILoggingBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-239/api/dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [LoggingAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-239/api/dovetail/loggingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-239/api/dovetail/loggingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-239/api/dovetail/loggingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
