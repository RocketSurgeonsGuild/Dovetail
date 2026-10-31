# IMeterProviderBuilderJoint Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.opentelemetry.md)
- Type: [IMeterProviderBuilderJoint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-meterproviderbuilder"></a>

## Register(IDovetailContext, MeterProviderBuilder)

- Name: `Register(IDovetailContext, MeterProviderBuilder)`
- Modifiers: `abstract`

Register the `Metrics.MeterProviderBuilder` part.

```csharp
public interface IMeterProviderBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        MeterProviderBuilder meterProvider)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `meterProvider` (`MeterProviderBuilder`)
