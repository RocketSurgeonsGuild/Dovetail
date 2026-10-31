# SerilogJoint Constructors

- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.serilog.md)
- Type: [SerilogJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.serilog/serilogjoint.md)
- Kind: `Constructors`
- Members: `1`

A joint that applies the Serilog joints and registers the Serilog logger they configure: the logger, Serilog's `ILogger` and the `ILoggerFactory` (which backs every `ILogger<T>`) take the place of the logging providers the host adds by default. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

<a id="constructor"></a>

## SerilogJoint

- Name: `Constructor`

```csharp
public sealed class SerilogJoint
{
    public SerilogJoint()
    {
        // ...
    }
}
```
