# LoggingJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Type: [LoggingJointExtensions](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/loggingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Logging parts.

<a id="applylogging"></a>
<a id="applylogging-iloggingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Logging.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
