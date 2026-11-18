# TracerProviderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.OpenTelemetry/Joints/TracerProviderJointDelegate.cs)
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

- [TracerProviderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#begininvoke-idovetailcontext-tracerproviderbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
