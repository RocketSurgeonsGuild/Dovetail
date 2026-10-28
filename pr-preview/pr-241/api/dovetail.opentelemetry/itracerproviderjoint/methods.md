# ITracerProviderJoint Methods

- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-241/api/dovetail.opentelemetry.md)
- Type: [ITracerProviderJoint](/Dovetail/pr-preview/pr-241/api/dovetail.opentelemetry/itracerproviderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-tracerproviderbuilder"></a>

## Register(IDovetailContext, TracerProviderBuilder)

- Name: `Register(IDovetailContext, TracerProviderBuilder)`
- Modifiers: `abstract`

Register the `Trace.TracerProviderBuilder` part.

```csharp
public interface ITracerProviderJoint
{
    public abstract void Register(
        IDovetailContext context,
        TracerProviderBuilder tracerProvider)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `tracerProvider` (`TracerProviderBuilder`)
