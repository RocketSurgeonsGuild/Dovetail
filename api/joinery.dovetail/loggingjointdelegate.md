# LoggingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.11`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.2281acdd6f0011b26520551395637e54409e48e8/src/Joinery.Dovetail/obj/Release/net10.0/LoggingJointDelegate.cs)
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

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)

## Constructors

- [LoggingJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail/loggingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/loggingjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/loggingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
