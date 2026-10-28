# TracerProviderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/v0.2.2/api/dovetail.opentelemetry.md)
- Version: `0.2.2`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the `Trace.TracerProvider` part.

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public sealed delegate void TracerProviderJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.Trace.TracerProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TracerProviderBuilder`)

## Constructors

- [TracerProviderJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/tracerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-tracerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
