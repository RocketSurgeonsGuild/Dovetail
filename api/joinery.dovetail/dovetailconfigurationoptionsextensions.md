# DovetailConfigurationOptionsExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.3.1-beta.7`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Registers strongly-typed configuration classes with the standard `Extensions.Options` pipeline.

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailConfigurationOptionsExtensions
{
    // ...
}
```

## Remarks

The configuration joint that Dovetail's source generator emits for `[DovetailConfiguration]` types calls [DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions(IServiceCollection, string, Func<IConfiguration, TOptions>)](/Dovetail/api/joinery.dovetail/dovetailconfigurationoptionsextensions/methods.md#adddovetailconfigurationoptions-iservicecollection-string-func-iconfiguration-toptions) for every type, passing the `Create...` method it generated for it. That code creates the instance and binds the section onto it without reflection, so the registration is trimming and AOT safe, and nothing here uses reflection. A library registers only its own section, and reads it from the application's complete `Configuration.IConfiguration`, which the service provider supplies when the options are first created.

## Methods

- [AddDovetailConfigurationOptions(IServiceCollection, string, Func<IConfiguration, TOptions>)](/Dovetail/api/joinery.dovetail/dovetailconfigurationoptionsextensions/methods.md#adddovetailconfigurationoptions-iservicecollection-string-func-iconfiguration-toptions) : `OptionsBuilder<TOptions>` `extension` -- Creates `TOptions` from the configuration section at `sectionPath` with `create`, without reflection.
