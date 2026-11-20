# LoggerProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-473-merge.Sha.c55e59e58abb6483addadae4101ca132a19fd392/src/Joinery.Dovetail.OpenTelemetry/Joints/LoggerProviderJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the `Logs.LoggerProviderBuilder` part.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public sealed delegate void LoggerProviderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.Logs.LoggerProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`LoggerProviderBuilder`)

## Constructors

- [LoggerProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, LoggerProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-loggerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
