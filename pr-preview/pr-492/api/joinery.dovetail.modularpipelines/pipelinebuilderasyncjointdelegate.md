# PipelineBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderAsyncJointDelegate.cs)
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

- [PipelineBuilderAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
