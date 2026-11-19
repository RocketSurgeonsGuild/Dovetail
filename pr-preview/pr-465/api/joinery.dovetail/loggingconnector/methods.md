# LoggingConnector Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Type: [LoggingConnector](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/loggingconnector.md)
- Kind: `Methods`
- Members: `1`

A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection-cancellationtoken"></a>

## Register(IDovetailContext, IServiceCollection, CancellationToken)

- Name: `Register(IDovetailContext, IServiceCollection, CancellationToken)`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/Infrastructure/LoggingJoint.cs#L15)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`) `optional`
