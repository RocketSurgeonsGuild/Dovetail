# DovetailConfigurationAttribute<T>

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.3-beta.1`
- Namespace: `Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`

Declares the configuration section of an options type `T` from the assembly, for types you don't own.

## Definition

```csharp
namespace Dovetail.Attributes;

public sealed class DovetailConfigurationAttribute<T>
    : System.Attribute
    where T : class
{
    // ...
}
```

## Type Parameters

- `T` -- The options type.

## Remarks

For example, `[assembly: DovetailConfiguration<HostOptions>("Hosting")]`.

The section path follows the same rules as [DovetailConfigurationAttribute](/Dovetail/api/dovetail/dovetailconfigurationattribute.md). Apply it once for each type you configure. A type has one section: naming it twice, or also declaring it with the class form, is reported as DT0015. `DovetailConfigurationAttribute`1.KeyStyle` works as it does on the class form.

Only works with source generators enabled.

## Constructors

- [DovetailConfigurationAttribute](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/constructors.md#constructor) -- Declares `T` under the section derived from its name.
- [DovetailConfigurationAttribute(string)](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/constructors.md#constructor-string) -- Declares `T` under `section`.

## Properties

- [KeyStyle](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/properties.md#keystyle) : [ConfigurationKeyStyle](/Dovetail/api/dovetail/configurationkeystyle.md) `get; set` -- How the configuration keys are spelled: those of every bound property and constructor parameter of the options type and of the object types nested in it, and the default section path derived from the type name. The default is [ConfigurationKeyStyle.Undefined](/Dovetail/api/dovetail/configurationkeystyle/fields.md), which keeps the declared names. See [ConfigurationKeyStyle](/Dovetail/api/dovetail/configurationkeystyle.md).
- [RegisterOptions](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/properties.md#registeroptions) : `bool` `get; set` -- Whether to register `IOptions<T>` for the type. `false` only describes the section in the schema. The default is `true`.
- [Section](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/properties.md#section) : `string?` `get` -- The section path, or `null` for the one derived from the type name.
- [ValidateOnStart](/Dovetail/api/dovetail/dovetailconfigurationattribute-1/properties.md#validateonstart) : `bool` `get; set` -- Whether to validate the options when the host starts. The default is `false`.
