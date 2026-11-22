# OpenTelemetryJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/OpenTelemetryJointDelegate.cs)
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

- [OpenTelemetryJointDelegate(object, nint)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.opentelemetry/opentelemetryjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
