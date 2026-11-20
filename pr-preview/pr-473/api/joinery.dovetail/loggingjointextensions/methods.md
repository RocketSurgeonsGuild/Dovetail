# LoggingJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.md)
- Type: [LoggingJointExtensions](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/loggingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Logging parts.

<a id="applylogging"></a>
<a id="applylogging-iloggingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-473-merge.Sha.c55e59e58abb6483addadae4101ca132a19fd392/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs#L51-L52)

Applies the registered Logging parts.

```csharp
public static class LoggingJointExtensions
{
    public static ValueTask ApplyLogging(
        this ILoggingBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`ILoggingBuilder`)
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
