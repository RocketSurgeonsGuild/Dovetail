# PipelineBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-beta.13`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.9116ea12c6002cf1bba224cfb363e5a7ac536a99/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the PipelineBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask PipelineBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    ModularPipelines.PipelineBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`PipelineBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [PipelineBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
