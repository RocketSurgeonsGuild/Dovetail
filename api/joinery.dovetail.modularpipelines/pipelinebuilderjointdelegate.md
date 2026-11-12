# PipelineBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Version: `0.5.0-beta.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
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

- [PipelineBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, PipelineBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#begininvoke-idovetailcontext-pipelinebuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
