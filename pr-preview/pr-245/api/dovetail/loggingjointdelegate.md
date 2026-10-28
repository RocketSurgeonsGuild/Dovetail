# LoggingJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-245/api/dovetail.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Logging part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void LoggingJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Logging.ILoggingBuilder builder)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-245/api/dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)

## Constructors

- [LoggingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-245/api/dovetail/loggingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-245/api/dovetail/loggingjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-245/api/dovetail/loggingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
