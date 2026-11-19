# OpenTelemetryAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/OpenTelemetryAsyncJointDelegate.cs)
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

- [OpenTelemetryAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iopentelemetrybuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
