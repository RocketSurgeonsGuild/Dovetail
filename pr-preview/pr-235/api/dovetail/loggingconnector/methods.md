# LoggingConnector Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Type: [LoggingConnector](/Dovetail/pr-preview/pr-235/api/dovetail/loggingconnector.md)
- Kind: `Methods`
- Members: `1`

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection-cancellationtoken"></a>

## Register(IDovetailContext, IServiceCollection, CancellationToken)

- Name: `Register(IDovetailContext, IServiceCollection, CancellationToken)`
- Returns: `ValueTask`

Register the Service part asynchronously.

```csharp
public sealed class LoggingConnector
{
    public ValueTask Register(
        IDovetailContext context,
        IServiceCollection services,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`) `optional`
