# TracerProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry.md)
- Version: `0.4.2-pr.3`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the `Trace.TracerProvider` part.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public sealed delegate void TracerProviderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.Trace.TracerProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TracerProviderBuilder`)

## Constructors

- [TracerProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-tracerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
