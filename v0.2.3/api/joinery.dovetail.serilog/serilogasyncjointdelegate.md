# SerilogAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/v0.2.3/api/joinery.dovetail.serilog.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [SerilogAsyncJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.serilog/serilogasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.serilog/serilogasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
