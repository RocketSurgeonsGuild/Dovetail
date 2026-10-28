# OpenTelemetryJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the OpenTelemetry part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void OpenTelemetryJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.IOpenTelemetryBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)

## Constructors

- [OpenTelemetryJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/opentelemetryjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
