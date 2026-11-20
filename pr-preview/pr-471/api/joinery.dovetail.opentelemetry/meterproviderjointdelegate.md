# MeterProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-471-merge.Sha.bf40892310bcd6ceeb21a319fcdbc6a1dfabd05f/src/Joinery.Dovetail.OpenTelemetry/Joints/MeterProviderJointDelegate.cs)
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

- [MeterProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/methods.md#begininvoke-idovetailcontext-meterproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
