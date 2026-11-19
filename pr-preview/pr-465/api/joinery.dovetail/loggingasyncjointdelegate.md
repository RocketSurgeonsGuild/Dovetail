# LoggingAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/obj/Release/net10.0/LoggingAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the Logging part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask LoggingAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Logging.ILoggingBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [LoggingAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/loggingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/loggingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/loggingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
