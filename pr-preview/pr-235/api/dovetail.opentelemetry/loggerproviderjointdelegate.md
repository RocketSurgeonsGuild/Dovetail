# LoggerProviderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the `Logs.LoggerProviderBuilder` part.

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public sealed delegate void LoggerProviderJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.Logs.LoggerProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`LoggerProviderBuilder`)

## Constructors

- [LoggerProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/loggerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, LoggerProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/loggerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-loggerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/loggerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
