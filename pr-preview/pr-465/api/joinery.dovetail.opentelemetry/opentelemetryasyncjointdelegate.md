# OpenTelemetryAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/OpenTelemetryAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the OpenTelemetry part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask OpenTelemetryAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.IOpenTelemetryBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [OpenTelemetryAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
