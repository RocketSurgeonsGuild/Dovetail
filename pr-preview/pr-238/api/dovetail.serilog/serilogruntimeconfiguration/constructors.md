# SerilogRuntimeConfiguration Constructors

- Package: [Dovetail.Serilog](/Dovetail/pr-preview/pr-238/api/dovetail.serilog.md)
- Type: [SerilogRuntimeConfiguration](/Dovetail/pr-preview/pr-238/api/dovetail.serilog/serilogruntimeconfiguration.md)
- Kind: `Constructors`
- Members: `1`

Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.

<a id="constructor"></a>

## SerilogRuntimeConfiguration

- Name: `Constructor`

```csharp
public record SerilogRuntimeConfiguration
{
    public SerilogRuntimeConfiguration()
    {
        // ...
    }
}
```
