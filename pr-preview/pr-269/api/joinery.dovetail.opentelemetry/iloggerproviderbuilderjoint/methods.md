# ILoggerProviderBuilderJoint Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.opentelemetry.md)
- Type: [ILoggerProviderBuilderJoint](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.opentelemetry/iloggerproviderbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for `Logs.LoggerProvider`. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-loggerproviderbuilder"></a>

## Register(IDovetailContext, LoggerProviderBuilder)

- Name: `Register(IDovetailContext, LoggerProviderBuilder)`
- Modifiers: `abstract`

Register the `Logs.LoggerProviderBuilder` part.

```csharp
public interface ILoggerProviderBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        LoggerProviderBuilder loggerProviderBuilder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `loggerProviderBuilder` (`LoggerProviderBuilder`)
