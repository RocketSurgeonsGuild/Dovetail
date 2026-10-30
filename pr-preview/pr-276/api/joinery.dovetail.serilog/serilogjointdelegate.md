# SerilogJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.serilog.md)
- Version: `0.3.1-pr.10`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [SerilogJointDelegate(object, nint)](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.serilog/serilogjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, AsyncCallback, object)](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
