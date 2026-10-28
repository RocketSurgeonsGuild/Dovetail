# SerilogRuntimeConfiguration Properties

- Package: [Dovetail.Serilog](/Dovetail/pr-preview/pr-245/api/dovetail.serilog.md)
- Type: [SerilogRuntimeConfiguration](/Dovetail/pr-preview/pr-245/api/dovetail.serilog/serilogruntimeconfiguration.md)
- Kind: `Properties`
- Members: `2`

Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.

<a id="minimumlevel"></a>

## MinimumLevel

- Name: `MinimumLevel`
- Modifiers: `get; set`
- Returns: `string`

The minimum Serilog level to emit, e.g. `"Information"`, `"Debug"`, `"Warning"`.

```csharp
public string MinimumLevel { get; set; }
```

<a id="writetoconsole"></a>

## WriteToConsole

- Name: `WriteToConsole`
- Modifiers: `get; set`
- Returns: `bool`

Whether the console sink should be enabled.

```csharp
public bool WriteToConsole { get; set; }
```
