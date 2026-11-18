# PipelineBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Version: `0.7.2-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d6b0f8d96f8417678d91819460b242bed026e72e/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderAsyncJointDelegate.cs)
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
