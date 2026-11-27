# LoggingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.md)
- Version: `0.8.2-pr.19`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail/obj/Release/net10.0/LoggingJointDelegate.cs)
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)

## Constructors

- [LoggingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/loggingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/loggingjointdelegate/methods.md#begininvoke-idovetailcontext-iloggingbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/loggingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
