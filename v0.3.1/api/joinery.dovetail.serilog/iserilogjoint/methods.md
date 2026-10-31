# ISerilogJoint Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/v0.3.1/api/joinery.dovetail.serilog.md)
- Type: [ISerilogJoint](/Dovetail/v0.3.1/api/joinery.dovetail.serilog/iserilogjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection-loggerconfiguration"></a>

## Register(IDovetailContext, IServiceCollection, LoggerConfiguration)

- Name: `Register(IDovetailContext, IServiceCollection, LoggerConfiguration)`
- Modifiers: `abstract`

Register the Serilog part.

```csharp
public interface ISerilogJoint
{
    public abstract void Register(
        IDovetailContext context,
        IServiceCollection services,
        LoggerConfiguration loggerConfiguration)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `loggerConfiguration` (`LoggerConfiguration`)
