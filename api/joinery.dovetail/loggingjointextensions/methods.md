# LoggingJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [LoggingJointExtensions](/Dovetail/api/joinery.dovetail/loggingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Logging parts.

<a id="applylogging"></a>
<a id="applylogging-iloggingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyLogging(ILoggingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

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
- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
