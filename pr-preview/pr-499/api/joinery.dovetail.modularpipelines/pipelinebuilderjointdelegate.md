# PipelineBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the PipelineBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void PipelineBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    ModularPipelines.PipelineBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`PipelineBuilder`)

## Constructors

- [PipelineBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
