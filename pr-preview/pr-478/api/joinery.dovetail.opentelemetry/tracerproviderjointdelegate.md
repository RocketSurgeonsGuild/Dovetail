# TracerProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.4`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-478-merge.Sha.3cefccc51138c5f35edba1dddeb09aae10973315/src/Joinery.Dovetail.OpenTelemetry/Joints/TracerProviderJointDelegate.cs)
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

- [TracerProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-tracerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
