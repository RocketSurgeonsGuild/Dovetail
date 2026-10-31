# ConfigurationKeyStyle

- Kind: `enum`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.3.1-beta.13`
- Namespace: `Joinery.Dovetail.Attributes`
- Target framework: `net10.0`

How [DovetailConfigurationAttribute](/Dovetail/api/joinery.dovetail/dovetailconfigurationattribute.md) spells the configuration keys of an options type and of every object type nested in it.

## Definition

```csharp
namespace Joinery.Dovetail.Attributes;

public enum ConfigurationKeyStyle
```

## Remarks

The style decides the canonical key of every bound property and constructor parameter, in both the generated binder and the configuration schema. It also decides the default section path derived from the type name. A `[ConfigurationKeyName]` on a member is always used as written, and an explicit section path is never changed. Dictionary keys, enum values and other data are never restyled.

Word splitting matches `Json.JsonNamingPolicy`: [ConfigurationKeyStyle.Camel](/Dovetail/api/joinery.dovetail/configurationkeystyle/fields.md) is `JsonNamingPolicy.CamelCase`, [ConfigurationKeyStyle.Dash](/Dovetail/api/joinery.dovetail/configurationkeystyle/fields.md) is `JsonNamingPolicy.KebabCaseLower` and [ConfigurationKeyStyle.Underscore](/Dovetail/api/joinery.dovetail/configurationkeystyle/fields.md) is `JsonNamingPolicy.SnakeCaseLower`.

The generated binder exists once for each type, so a type reached from sections of styles that spell its keys differently is reported as DT0016.

## Enum Members

| Name | Value | Description |
| --- | --- | --- |
| Undefined | 0 | Nothing is respelled: the canonical key is the declared name exactly as written, and the derived section path is the type name without its suffix. Configuration keys ignore case, so data in any casing binds, as long as the letters are right. This is the default. |
| Pascal | 1 | The declared name with its first letter upper-cased: maxFileSizeMb becomes MaxFileSizeMb, and GreetingsOptions is the section Greetings. For the usual C# property name this is the same as Undefined. |
| Camel | 2 | Lower camel case: MaxFileSizeMb becomes maxFileSizeMb, HTTPServer becomes httpServer, and HttpClientOptions is the section httpClient. Configuration keys ignore case, so this only changes the spelling the schema suggests. |
| Dash | 3 | Lower-case words joined with -: MaxFileSizeMb becomes max-file-size-mb, and HttpClientOptions is the section http-client. The binder reads exactly these keys, so a value written as MaxFileSizeMb no longer binds. |
| Underscore | 4 | Lower-case words joined with _: MaxFileSizeMb becomes max_file_size_mb, and HttpClientOptions is the section http_client. The binder reads exactly these keys, so a value written as MaxFileSizeMb no longer binds. |
