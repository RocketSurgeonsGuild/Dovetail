# LoggingJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.md)
- Type: [LoggingJointExtensions](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/loggingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Logging parts.

<a id="applylogging"></a>
<a id="applylogging-iloggingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
