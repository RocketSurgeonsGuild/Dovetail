# PipelineBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-493/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-493-merge.Sha.6497bf3ce2e5a41f8b0e13cad9b2c876f4918e36/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderAsyncJointDelegate.cs)
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

- [PipelineBuilderAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-493/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-493/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-493/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
