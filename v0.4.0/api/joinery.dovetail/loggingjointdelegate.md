# LoggingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Logging part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void LoggingJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Logging.ILoggingBuilder builder)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)

## Constructors

- [LoggingJointDelegate(object, nint)](/Dovetail/v0.4.0/api/joinery.dovetail/loggingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)](/Dovetail/v0.4.0/api/joinery.dovetail/loggingjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.4.0/api/joinery.dovetail/loggingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
