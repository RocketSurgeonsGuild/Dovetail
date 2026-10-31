# ISerilogAsyncJoint Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/v0.4.0/api/joinery.dovetail.serilog.md)
- Type: [ISerilogAsyncJoint](/Dovetail/v0.4.0/api/joinery.dovetail.serilog/iserilogasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Serilog. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection-loggerconfiguration-cancellationtoken"></a>

## Register(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken)

- Name: `Register(IDovetailContext, IServiceCollection, LoggerConfiguration, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Serilog part asynchronously.

```csharp
public interface ISerilogAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IServiceCollection services,
        LoggerConfiguration loggerConfiguration,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `loggerConfiguration` (`LoggerConfiguration`)
- `cancellationToken` (`CancellationToken`) `optional`
