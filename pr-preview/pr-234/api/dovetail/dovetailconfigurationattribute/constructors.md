# DovetailConfigurationAttribute Constructors

- Package: [Dovetail](/Dovetail/pr-preview/pr-234/api/dovetail.md)
- Type: [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-234/api/dovetail/dovetailconfigurationattribute.md)
- Kind: `Constructors`
- Members: `2`

Declares the configuration section of an options class. Dovetail's source generator describes the class as JSON Schema, which is merged into the application's `appsettings.schema.json` for editor completion and validation, and registers it with the options pipeline.

<a id="constructor"></a>

## DovetailConfigurationAttribute

- Name: `Constructor`

Declares the attributed class under the section derived from its name.

```csharp
public sealed class DovetailConfigurationAttribute
{
    public DovetailConfigurationAttribute()
    {
        // ...
    }
}
```

<a id="constructor-string"></a>

## DovetailConfigurationAttribute(string)

- Name: `Constructor(string)`

Declares the attributed class under `section`.

```csharp
public sealed class DovetailConfigurationAttribute
{
    public DovetailConfigurationAttribute(
        string section)
    {
        // ...
    }
}
```

## Parameters

- `section` (`string`)
  The section path, e.g. `"Greetings"` or `"Http:Retry"`.
