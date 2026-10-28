# OpenTelemetryAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/api/dovetail.opentelemetry.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the OpenTelemetry part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask OpenTelemetryAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.IOpenTelemetryBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [OpenTelemetryAsyncJointDelegate(object, nint)](/Dovetail/api/dovetail.opentelemetry/opentelemetryasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
