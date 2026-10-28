# MeterProviderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry.md)
- Version: `0.2.2-pr.2`
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

- [MeterProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/meterproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/meterproviderjointdelegate/methods.md#begininvoke-idovetailcontext-meterproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.opentelemetry/meterproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
