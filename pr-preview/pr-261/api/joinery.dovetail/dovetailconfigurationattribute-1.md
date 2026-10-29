# DovetailConfigurationAttribute<T>

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.md)
- Version: `0.3.1-pr.3`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`

Declares the configuration section of an options type `T` from the assembly, for types you don't own.

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

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

The section path follows the same rules as [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute.md). Apply it once for each type you configure. A type has one section: naming it twice, or also declaring it with the class form, is reported as DT0015. `DovetailConfigurationAttribute`1.KeyStyle` works as it does on the class form.

Only works with source generators enabled.

## Constructors

- [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/constructors.md#constructor) -- Declares `T` under the section derived from its name.
- [DovetailConfigurationAttribute(string)](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/constructors.md#constructor-string) -- Declares `T` under `section`.

## Properties

- [KeyStyle](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/properties.md#keystyle) : [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle.md) `get; set` -- How the configuration keys are spelled: those of every bound property and constructor parameter of the options type and of the object types nested in it, and the default section path derived from the type name. The default is [ConfigurationKeyStyle.Undefined](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle/fields.md), which keeps the declared names. See [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/configurationkeystyle.md).
- [RegisterOptions](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/properties.md#registeroptions) : `bool` `get; set` -- Whether to register `IOptions<T>` for the type. `false` only describes the section in the schema. The default is `true`.
- [Section](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/properties.md#section) : `string?` `get` -- The section path, or `null` for the one derived from the type name.
- [ValidateOnStart](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailconfigurationattribute-1/properties.md#validateonstart) : `bool` `get; set` -- Whether to validate the options when the host starts. The default is `false`.
