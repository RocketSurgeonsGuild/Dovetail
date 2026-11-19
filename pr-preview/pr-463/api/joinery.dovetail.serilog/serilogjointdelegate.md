# SerilogJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.serilog.md)
- Version: `0.7.2-pr.8`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/SerilogJointDelegate.cs)
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

- [SerilogJointDelegate(object, nint)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.serilog/serilogjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, AsyncCallback, object)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.serilog/serilogjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
