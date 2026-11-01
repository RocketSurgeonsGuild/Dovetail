# DovetailConfigurationAttribute

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.md)
- Version: `0.4.1-pr.6`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`
- Inherits: `Attribute`

Declares the configuration section of an options class. Dovetail's source generator describes the class as JSON Schema, which is merged into the application's `appsettings.schema.json` for editor completion and validation, and registers it with the options pipeline.

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public sealed class DovetailConfigurationAttribute
    : System.Attribute
{
    // ...
}
```

## Remarks

Without a section path the section is the type name with an `Options`, `Settings`, `Configuration` or `Config` suffix removed, so `GreetingsOptions` becomes `Greetings`. Nested sections use `:`, as in `"Http:Retry"`.

A type has one section, so it can be declared once. For a type you don't own, use the assembly form, `Attributes.DovetailConfigurationAttribute`1`.

[DovetailConfigurationAttribute.KeyStyle](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/properties.md#keystyle) picks how the keys of the type and of the types nested in it are spelled (as declared, Pascal, camel, dash or underscore case). A type that several sections reach has to spell its keys the same way in all of them; DT0016 reports a mismatch.

The generated registration reads its own section from the application's complete `IConfiguration` and creates or binds the options.

Only works with source generators enabled.

## Constructors

- [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/constructors.md#constructor) -- Declares the attributed class under the section derived from its name.
- [DovetailConfigurationAttribute(string)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/constructors.md#constructor-string) -- Declares the attributed class under `section`.

## Properties

- [KeyStyle](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/properties.md#keystyle) : [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/configurationkeystyle.md) `get; set` -- How the configuration keys are spelled: those of every bound property and constructor parameter of the options type and of the object types nested in it, and the default section path derived from the type name. The default is [ConfigurationKeyStyle.Undefined](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/configurationkeystyle/fields.md), which keeps the declared names. See [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/configurationkeystyle.md).
- [RegisterOptions](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/properties.md#registeroptions) : `bool` `get; set` -- Whether to register `IOptions<T>` for the type. `false` only describes the section in the schema. The default is `true`.
- [Section](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/properties.md#section) : `string?` `get` -- The section path, or `null` for the one derived from the type name.
- [ValidateOnStart](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailconfigurationattribute/properties.md#validateonstart) : `bool` `get; set` -- Whether to validate the options when the host starts. The default is `false`.
