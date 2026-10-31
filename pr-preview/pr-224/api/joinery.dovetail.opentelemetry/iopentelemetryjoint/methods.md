# IOpenTelemetryJoint Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.opentelemetry.md)
- Type: [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iopentelemetrybuilder"></a>

## Register(IDovetailContext, IOpenTelemetryBuilder)

- Name: `Register(IDovetailContext, IOpenTelemetryBuilder)`
- Modifiers: `abstract`

Register the OpenTelemetry part.

```csharp
public interface IOpenTelemetryJoint
{
    public abstract void Register(
        IDovetailContext context,
        IOpenTelemetryBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)
