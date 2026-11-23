# SerilogAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.serilog.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.1faa3d8c1fb998da38c062158c5a17b8390b25fa/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/SerilogAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the Serilog part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask SerilogAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services,
    Serilog.LoggerConfiguration loggerConfiguration,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `loggerConfiguration` (`LoggerConfiguration`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [SerilogAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.serilog/serilogasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
