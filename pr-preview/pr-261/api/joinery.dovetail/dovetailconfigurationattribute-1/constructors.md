# DovetailConfigurationAttribute<T> Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.md)
- Type: [DovetailConfigurationAttribute<T>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1.md)
- Kind: `Constructors`
- Members: `2`

Declares the configuration section of an options type `T` from the assembly, for types you don't own.

<a id="constructor"></a>

## DovetailConfigurationAttribute

- Name: `Constructor`

Declares `T` under the section derived from its name.

```csharp
public sealed class DovetailConfigurationAttribute<T>
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

Declares `T` under `section`.

```csharp
public sealed class DovetailConfigurationAttribute<T>
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
