# OpenTelemetryJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the OpenTelemetry part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void OpenTelemetryJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.IOpenTelemetryBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)

## Constructors

- [OpenTelemetryJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
