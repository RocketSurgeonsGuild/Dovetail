# DovetailConfigurationAttribute Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.md)
- Type: [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute.md)
- Kind: `Properties`
- Members: `4`

Declares the configuration section of an options class. Dovetail's source generator describes the class as JSON Schema, which is merged into the application's `appsettings.schema.json` for editor completion and validation, and registers it with the options pipeline.

<a id="keystyle"></a>

## KeyStyle

- Name: `KeyStyle`
- Modifiers: `get; set`
- Returns: [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle.md)

How the configuration keys are spelled: those of every bound property and constructor parameter of the options type and of the object types nested in it, and the default section path derived from the type name. The default is [ConfigurationKeyStyle.Undefined](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle/fields.md), which keeps the declared names. See [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle.md).

```csharp
public ConfigurationKeyStyle KeyStyle { get; set; }
```

<a id="registeroptions"></a>

## RegisterOptions

- Name: `RegisterOptions`
- Modifiers: `get; set`
- Returns: `bool`

Whether to register `IOptions<T>` for the type. `false` only describes the section in the schema. The default is `true`.

```csharp
public bool RegisterOptions { get; set; }
```

<a id="section"></a>

## Section

- Name: `Section`
- Modifiers: `nullable` `get`
- Returns: `string?`

The section path, or `null` for the one derived from the type name.

```csharp
public string? Section { get; }
```

<a id="validateonstart"></a>

## ValidateOnStart

- Name: `ValidateOnStart`
- Modifiers: `get; set`
- Returns: `bool`

Whether to validate the options when the host starts. The default is `false`.

```csharp
public bool ValidateOnStart { get; set; }
```
