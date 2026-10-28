# IOpenTelemetryAsyncJoint Methods

- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry.md)
- Type: [IOpenTelemetryAsyncJoint](/Dovetail/pr-preview/pr-244/api/dovetail.opentelemetry/iopentelemetryasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iopentelemetrybuilder-cancellationtoken"></a>

## Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the OpenTelemetry part asynchronously.

```csharp
public interface IOpenTelemetryAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IOpenTelemetryBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
