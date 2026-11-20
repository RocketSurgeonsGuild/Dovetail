# PipelineBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.1-pr.11`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-468-merge.Sha.1950047ba9b0769796f6cf96621cfafcd184f8e2/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderJointDelegate.cs)
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

- [PipelineBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
