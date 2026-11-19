# SerilogJointExtensions Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.serilog.md)
- Type: [SerilogJointExtensions](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.serilog/serilogjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Serilog parts.

<a id="applyserilog"></a>
<a id="applyserilog-loggerconfiguration-idovetailcontext-iservicecollection-cancellationtoken"></a>

## ApplySerilog(LoggerConfiguration, IDovetailContext, IServiceCollection, CancellationToken)

- Name: `ApplySerilog(LoggerConfiguration, IDovetailContext, IServiceCollection, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.Serilog/obj/Release/net10.0/DovetailJoint.Serilog.g.cs#L51-L52)

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
