# ResourceBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.OpenTelemetry/Joints/ResourceBuilderJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the `Resources.ResourceBuilder` part.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public sealed delegate void ResourceBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    OpenTelemetry.Resources.ResourceBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ResourceBuilder`)

## Constructors

- [ResourceBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ResourceBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-resourcebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
