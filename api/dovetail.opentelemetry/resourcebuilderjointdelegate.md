# ResourceBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.OpenTelemetry](/Dovetail/api/dovetail.opentelemetry.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the `Resources.ResourceBuilder` part.

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public sealed delegate void ResourceBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    OpenTelemetry.Resources.ResourceBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ResourceBuilder`)

## Constructors

- [ResourceBuilderJointDelegate(object, nint)](/Dovetail/api/dovetail.opentelemetry/resourcebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ResourceBuilder, AsyncCallback, object)](/Dovetail/api/dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-resourcebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
