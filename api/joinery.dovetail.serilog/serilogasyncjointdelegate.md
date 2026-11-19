# SerilogAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/api/joinery.dovetail.serilog.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/SerilogAsyncJointDelegate.cs)
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

- [SerilogAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.serilog/serilogasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
