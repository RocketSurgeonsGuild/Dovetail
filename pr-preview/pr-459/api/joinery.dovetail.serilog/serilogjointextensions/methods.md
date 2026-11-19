# SerilogJointExtensions Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.serilog.md)
- Type: [SerilogJointExtensions](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.serilog/serilogjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Serilog parts.

<a id="applyserilog"></a>
<a id="applyserilog-loggerconfiguration-idovetailcontext-iservicecollection-cancellationtoken"></a>

## ApplySerilog(LoggerConfiguration, IDovetailContext, IServiceCollection, CancellationToken)

- Name: `ApplySerilog(LoggerConfiguration, IDovetailContext, IServiceCollection, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/DovetailJoint.Serilog.g.cs#L51-L52)

Applies the registered Serilog parts.

```csharp
public static class SerilogJointExtensions
{
    public static ValueTask ApplySerilog(
        this LoggerConfiguration loggerConfiguration,
        IDovetailContext context,
        IServiceCollection services,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `loggerConfiguration` (`LoggerConfiguration`)
- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
