# SerilogJointExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.serilog.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/DovetailJoint.Serilog.g.cs)

Extension methods for applying Serilog parts.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public static class SerilogJointExtensions
{
    // ...
}
```

## Methods

- [ApplySerilog(LoggerConfiguration, IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.serilog/serilogjointextensions/methods.md#applyserilog-loggerconfiguration-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` `extension` -- Applies the registered Serilog parts.
