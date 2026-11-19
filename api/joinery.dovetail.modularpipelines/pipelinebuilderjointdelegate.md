# PipelineBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/PipelineBuilderJointDelegate.cs)
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
