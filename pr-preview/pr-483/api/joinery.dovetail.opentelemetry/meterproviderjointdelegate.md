# MeterProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.OpenTelemetry/Joints/MeterProviderJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the `Metrics.MeterProvider` part.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public sealed delegate void MeterProviderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.Metrics.MeterProviderBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`MeterProviderBuilder`)

## Constructors

- [MeterProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/methods.md#begininvoke-idovetailcontext-meterproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
