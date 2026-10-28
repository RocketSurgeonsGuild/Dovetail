# MeterProviderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/v0.2.2/api/dovetail.opentelemetry.md)
- Version: `0.2.2`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the `Metrics.MeterProvider` part.

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public sealed delegate void MeterProviderJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.Metrics.MeterProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MeterProviderBuilder`)

## Constructors

- [MeterProviderJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/meterproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/meterproviderjointdelegate/methods.md#begininvoke-idovetailcontext-meterproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/meterproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
