# SerilogJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.serilog.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/SerilogJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Serilog part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void SerilogJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services,
    Serilog.LoggerConfiguration loggerConfiguration)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `loggerConfiguration` (`LoggerConfiguration`)

## Constructors

- [SerilogJointDelegate(object, nint)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.serilog/serilogjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, AsyncCallback, object)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
