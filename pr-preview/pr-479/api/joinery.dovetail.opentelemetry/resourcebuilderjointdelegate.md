# ResourceBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail.OpenTelemetry/Joints/ResourceBuilderJointDelegate.cs)
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

- [ResourceBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ResourceBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-resourcebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
