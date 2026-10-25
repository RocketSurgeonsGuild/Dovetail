---
title: Managed Configuration
description: Declare a library's configuration as code and get typed options, a reflection-free binder, and a JSON Schema for editors, merged per application.
tags: [source-generator, configuration, msbuild, json-schema]
---

# Managed Configuration

A library that ships configuration describes it once, as a C# type. Dovetail's source generator reads that type and
produces two things from the same model:

- a **binder** that fills the type from `IConfiguration` and registers it as `IOptions<T>`, with no reflection, so it is
  AOT and trimming safe;
- a **JSON Schema** fragment that describes the type's section. The build merges the fragments of every referenced
  library into one schema for the application, which editors use for completion, hover documentation and validation of
  `appsettings.json`, YAML and TOML files. See [Editor setup](../guides/editor-setup).

The library's default values still ship as an `appsettings` file, which Dovetail packs and copies into hosts
([below](#shipping-default-values)).

> [!NOTE]
> The schema and the generated binder need C# 12 or later. In a project with an older `LangVersion` the generator emits
> nothing and reports [`DT0006`](#diagnostics).

## Declaring a section

Put `[DovetailConfiguration]` on the options class:

```c#
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dovetail.Attributes;

/// <summary>Options for the greetings this library prints.</summary>
[DovetailConfiguration]
public sealed class GreetingsOptions
{
    /// <summary>What the greeting says before the name.</summary>
    [Description("The text that comes before the name, e.g. \"Hello\".")]
    public string Message { get; set; } = "";

    /// <summary>How many times the greeting is said.</summary>
    [Range(1, 5)]
    public int Repeat { get; set; } = 1;
}
```

Consume it the usual way, with `IOptions<GreetingsOptions>`, `IOptionsSnapshot<T>` or `IOptionsMonitor<T>`. The
generator also exports a joint (`DovetailConfigurationOptions`) that registers the binding in every host that references
the assembly, so there is nothing to call. The [`SdkSample.Greetings`](https://github.com/RocketSurgeonsGuild/Dovetail/tree/main/sample/SdkSample/SdkSample.Greetings)
project is a working example.

### Types you don't own

To bind a type from another library, use the generic attribute on the assembly. This registers `HostOptions` under
`Hosting`:

```c#
[assembly: DovetailConfiguration<HostOptions>("Hosting")]
```

The assembly form is `DovetailConfiguration<T>` and needs C# 11. Descriptions for third-party types come from the XML
documentation file that ships with the package, when there is one.

### Section names

| Attribute                                      | Section                                                                                                                           |
| ---------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| `[DovetailConfiguration]`                      | The type name with one `Options`, `Settings`, `Configuration` or `Config` suffix removed: `GreetingsOptions` becomes `Greetings`. |
| `[DovetailConfiguration("Http:Retry")]`        | The path you give. `:` nests sections. Keys are case-insensitive.                                                                 |
| `[assembly: DovetailConfiguration<T>]`         | The same rule as the class form, applied to `T`.                                                                                  |

There is no prefix. A name that would become empty after stripping the suffix (a class called `Options`) is an error
([`DT0008`](#diagnostics)); give it a path.

### One section per type

A type has exactly one section, so it is declared once: `[DovetailConfiguration]` on the class allows one attribute, and
`DovetailConfiguration<T>` on the assembly allows one per `T` (the compiler rejects the same `T` twice). Declaring a
type with both the class form and the assembly form is an error ([`DT0015`](#diagnostics)); the class declaration wins
and the assembly one produces nothing.

At run time a library reads only its own section from the application's complete `IConfiguration` and creates or binds
its options from it. Dovetail doesn't order, de-duplicate or merge sections. The generated joint registers an assembly's
types by section path, then type name, and the standard options pipeline does the rest.

Mutable classes go through the standard options pipeline (`IConfigureOptions<T>`), so your own `Configure<T>` and
`PostConfigure<T>` calls keep working: ones registered before a Dovetail registration for the type run before it, and
ones registered after it run after it.

If two assemblies register the same type through the public `AddDovetailConfigurationOptions` overloads, the
pipeline's usual rules apply: each bind registration adds its own configure step, in registration order, and for a type
that is created rather than bound the first registered factory wins.

### `RegisterOptions` and `ValidateOnStart`

| Property          | Default | Effect                                                                                                  |
| ----------------- | ------- | ------------------------------------------------------------------------------------------------------- |
| `RegisterOptions` | `true`  | `false` only describes the section in the schema. Nothing is registered, so you bind the type yourself. |
| `ValidateOnStart` | `false` | Validates the options when the host starts, instead of the first time they are read.                    |

## Validation

Dovetail never turns on `ValidateDataAnnotations`. `[Range]`, `[MinLength]` and the other annotations describe the
schema, which is how editors flag a bad value; they are not enforced at run time. To enforce rules, write an
`[OptionsValidator]` class, the options validation source generator from `Microsoft.Extensions.Options`:

```c#
[OptionsValidator]
public sealed partial class GreetingsOptionsValidator : IValidateOptions<GreetingsOptions>;
```

Dovetail registers every such validator in the assembly for the option types it registers, with `TryAddEnumerable`, so
it never duplicates one you added yourself. Add `ValidateOnStart = true` on the attribute and a bad configuration fails
the host at startup.

## Supported types

The generator follows Microsoft's configuration binding source generator, not the serializer:

- `string`, `bool`, every numeric type (including `Half`, `Int128` and `UInt128`), `char`, enums (any casing), `TimeSpan`,
  `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `Guid`, `Version`, `Uri`, `CultureInfo`, `byte[]`;
- `Nullable<T>` of the above;
- nested classes, structs and records, and recursive types (the schema uses `$ref`);
- arrays, lists, sets and the other collection types the binder supports, and dictionaries with string, enum or integer
  keys;
- `object` and `IConfigurationSection`, which accept any value.

A type with a `[TypeConverter]` is described as an object, because the binder ignores converters. A member the binder
can't bind is left out of both the schema and the binder, with warning [`DT0009`](#diagnostics). `[ConfigurationKeyName]`
renames a key, and from binder 11 `[ConfigurationIgnore]` leaves a member out.

### Annotations that reach the schema

| Source                                      | Schema                                                    |
| ------------------------------------------- | --------------------------------------------------------- |
| `[Description]`, else the XML `<summary>`   | `description`. For a record parameter, its `<param>`.     |
| `[Range]`, length attributes                | `minimum`/`maximum`, `minLength`/`maxLength`, item counts |
| `[RegularExpression]`                       | `pattern`, when the expression can be anchored            |
| `[Url]`, `[EmailAddress]`                   | `format`                                                  |
| `[Obsolete]`                                | `deprecated` and a deprecation message                    |
| A constant initializer or parameter default | `default`                                                 |

Ranges and defaults are also written into the description, since editors don't show them. The XML summary is read from
the source whether or not `GenerateDocumentationFile` is on. `<inheritdoc />` and `<include>` produce no description.
Annotations that JSON Schema can't express are dropped with [`DT0013`](#diagnostics).

Fragments are permissive: nothing is `required` unless you set `<DovetailConfigurationSchemaRequired>true</DovetailConfigurationSchemaRequired>`,
which marks `required` members, `[Required]` properties and value-type constructor parameters without defaults.

## The generated binder

Dovetail doesn't call Microsoft's binding generator. A source generator never sees another generator's output, so the
calls in Dovetail's own code would not be intercepted. Instead it **ports** that generator: the parser, the type
specifications and the emitter from dotnet/runtime (MIT), at tags `v10.0.13` and `v11.0.0-rc.2`. The rules follow the
**major version of `Microsoft.Extensions.Configuration.Binder`** that the project references (version 11 when it
references none), not the target framework, because a net10.0 application can reference Binder 11.

What the two versions do differently:

| Case                                           | Binder 10              | Binder 11                    |
| ---------------------------------------------- | ---------------------- | ---------------------------- |
| `Nullable<T>` set to `""`                      | value kept             | `null`                       |
| String or object constructor parameter missing | throws                 | `null`                       |
| Value-type constructor parameter set to `""`   | default                | throws                       |
| Nullable or reference-type parameter missing   | throws                 | `null` (structs still throw) |
| Parameter with a default, set to null          | `null`                 | the default                  |
| Properties bound through the constructor       | bound again afterwards | not bound again              |
| `[ConfigurationIgnore]`                        | ignored                | honoured, in the schema too  |

The same data is bound three ways in the parity tests (Microsoft's generator, the reflection binder, Dovetail's) on both
versions.

### Records and other immutable types

Types the options pipeline can't create (constructor parameters, `init`-only or `required` members) get a generated
**options factory** instead of `IConfigureOptions<T>`. It creates the instance from the type's section, and still runs your
validators.

```c#
[DovetailConfiguration]
public sealed record FarewellOptions(string Message, [property: Range(0, 5)] int Exclamations);
```

### Where Dovetail differs from Microsoft's generator, on purpose

1. **Object-initializer types.** Microsoft's generator never sets `init`-only members of a type with a parameterless
   constructor, and can't create a type with `required` members (CS9035). Dovetail creates them with an object
   initializer, then binds the rest. A missing key keeps the member's own initial value, as the reflection binder does;
   a `required` member with no value gets `default`.
2. **`required` members that can't be bound** (an unsupported type or `[ConfigurationIgnore]`) are set to `default!`, so
   the generated code compiles. [`DT0009`](#diagnostics) reports the member.
3. **Empty sections still produce an instance** for factory types. `Get<T>()` returns `null` there, but `IOptions<T>.Value`
   can't be `null`. A missing required constructor parameter throws the first time the options are read.
4. **No `NotSupportedException`s** and no `BindNonPublicProperties`.
5. **Self-expanding generics** such as `class Node<T> { Node<Node<T>> Next; }` are treated as unsupported past a nesting
   depth of 10 or 1,000 types, instead of making the analysis run forever. The schema reports
   [`DT0012`](#diagnostics) for the same graphs.

Microsoft's own quirks are kept. A positional record's extra `init`-only property is set to `default` when its key is
missing, overriding its initializer, and Binder 10 binds constructor-bound collections twice.

Registration also differs from `services.Configure<T>(section)` in that the configuration's reload token feeds
`IOptionsMonitor<T>` through one change token source per type, and the `IConfiguration` is resolved from the container
when the options are first created, not captured at registration.

## The schema

### Library fragments

Each assembly embeds one fragment per section as
`[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:<Section>", "<JSON Schema>")]`. Each fragment is a
Draft-07 schema of the section's value, with every object type described once under `definitions`. Every key also has a
case-insensitive `patternProperties` entry with the same schema, so `retries: 99` is checked even when written as
`Retries`.

A library project also writes the fragments as `ConfigurationSchema.json` (permissive, rooted at the configuration root
like Aspire's) and packs it at the NuGet package root, so Visual Studio and other tools that understand Aspire's layout
can use it. A library with no sections writes nothing.

### The merged application schema

Applications (detected by Dovetail.Sdk, or `OutputType` of `Exe` or `WinExe`) and test projects merge their own fragments
and those of every referenced assembly into `obj/appsettings.schema.json`. The file is a stable path editors can point at,
is copied to the output directory next to `appsettings.json`, and is always published. Hosts that embed their
configuration (MAUI, Uno) keep it in `obj/` only. A host that targets several frameworks writes it from the first one.

- **It is strict.** Each Dovetail section, and the object types it uses, accepts only its own keys. The check is a
  case-insensitive `propertyNames` pattern rather than `additionalProperties: false`, so `message` is accepted and
  `Mesage` is flagged, with an error that lists the known keys. The root, parents shared between sections, dictionaries
  and sections declared by more than one assembly stay open for keys of other sources.
- **It includes SchemaStore's `appsettings.json` schema**, embedded rather than referenced, so `Logging`, `Kestrel`
  and `ConnectionStrings` complete too, online or offline. The copy ships with Dovetail and is refreshed each release (Apache-2.0).
- **It is deterministic.** Sections compare ignoring case; when libraries spell a section differently, the
  ordinal-smallest spelling is displayed. A section several assemblies declare is merged into one object. The same
  references always produce a byte-identical file.

| Property                                        | Default  | Effect                                                                         |
| ----------------------------------------------- | -------- | ------------------------------------------------------------------------------ |
| `DovetailConfigurationSchema`                   | on       | `false` writes no schema files at all.                                         |
| `DovetailConfigurationSchemaHost`               | detected | `true` or `false` overrides which projects count as hosts.                     |
| `DovetailConfigurationSchemaStrict`             | `true`   | `false` leaves Dovetail's sections open (no `propertyNames`) in the host file. |
| `DovetailConfigurationSchemaIncludeSchemaStore` | `true`   | `false` leaves SchemaStore's schema out.                                       |
| `DovetailConfigurationSchemaRequired`           | `false`  | `true` lists required members in `required`, in the fragments.                 |

```xml
<PropertyGroup>
  <!-- Permissive: unknown keys under Dovetail sections aren't flagged -->
  <DovetailConfigurationSchemaStrict>false</DovetailConfigurationSchemaStrict>
</PropertyGroup>
```

> [!NOTE]
> Editors flag a quoted number such as `"Timeout": "30"` as an incorrect type, although the .NET binder accepts it.
> Dovetail keeps the schema strict about types so that ranges are always checked.

The build target is `AfterTargets="CoreCompile"` and skips design-time builds. It writes only when its inputs change.
Merging runs in a compiled MSBuild task (`Dovetail.Build.Tasks`) that reads the fragments with `System.Reflection.Metadata`,
so no assembly is loaded and nothing is embedded in your application.

### Serving the schema

With `DovetailConfigurationSchemaEndpoint` set to `true`, Dovetail.Hosting.Web maps
`GET /.well-known/dovetail/configuration-schema`, which returns the merged schema. It is off by default.
`DovetailConfigurationSchemaEndpointEnvironments` limits it to a list of environments. See [Editor setup](../guides/editor-setup#the-schema-endpoint).

## Shipping default values

Defaults still live in a normal `appsettings` file in the library. The SDK renames it, packs it, and loads it into the
host's configuration, where the generated binder picks it up:

```
MyLibrary/
├── appsettings.yaml
├── appsettings.Development.yaml
├── appsettings.local.yaml   # never packed, never committed
└── MyLibrary.csproj
```

- `appsettings.{ext}` becomes `MyLibrary.{ext}` (`appsettings.Production.json` becomes `MyLibrary.Production.json`), so several
  libraries can ship their own file into one output directory.
- The files are packed under `contentFiles/any/any/configuration/`, copied to a same-solution host's output directory
  through a `ProjectReference`, and restored from the package otherwise.
- The generated joint loads, in ascending precedence, `MyLibrary.{ext}`, `MyLibrary.{Environment}.{ext}` and
  `MyLibrary.local.{ext}`. The `local` layer is never packed, and the SDK warns when no `.gitignore` at or above the
  project ignores `appsettings.local.*`.
- JSON, YAML and TOML are supported. Opt in with `DovetailEnableConfiguration`; YAML is the default format, and
  `DovetailEnableJsonConfiguration` or `DovetailEnableTomlConfiguration` chooses another. Hosts with no file system
  (MAUI, Uno) read copies embedded in the assembly: `DovetailEmbedConfiguration`.

| Property                          | Default                            | Purpose                                                                                             |
| --------------------------------- | ---------------------------------- | --------------------------------------------------------------------------------------------------- |
| `DovetailEnableConfiguration`     | `false`                            | Turns the pipeline on. Existing consumers see no change until it or a format flag is set.           |
| `DovetailEnableYamlConfiguration` | `true` when no other format is set | Adds the YAML provider. Setting it to `true` also enables the pipeline.                             |
| `DovetailEnableJsonConfiguration` | `false`                            | Adds the JSON provider. Setting it to `true` also enables the pipeline, replacing the YAML default. |
| `DovetailEnableTomlConfiguration` | `false`                            | Adds the TOML provider. Setting it to `true` also enables the pipeline, replacing the YAML default. |
| `DovetailPackConfiguration`       | `true`, `false` if not packable    | Packs the library's configuration files into the package.                                           |

## Diagnostics

| Id       | Severity | Meaning                                                                                                                     |
| -------- | -------- | --------------------------------------------------------------------------------------------------------------------------- |
| `DT0006` | Warning  | The project's C# version is older than 12, so Dovetail generated nothing. Set `LangVersion` to 12 or later.                 |
| `DT0007` | Error    | The type can't be described as configuration, for example an interface, a collection or an open generic type.  |
| `DT0008` | Error    | The section isn't valid for the type: empty, or a name that is empty once the suffix is removed.                            |
| `DT0009` | Warning  | A member has a type configuration binding doesn't support. It is left out of the schema and the binder.                     |
| `DT0010` | Warning  | Two members bind the same key (keys are case-insensitive). Only the first is described.                                     |
| `DT0011` | Error    | The type can't be created from configuration, for example an ambiguous constructor or a parameter that matches no property. |
| `DT0012` | Warning  | The type graph uses too many distinct types, so the schema stops describing them at the member named.                       |
| `DT0013` | Warning  | An annotation has no JSON Schema equivalent and is left out.                                                                |
| `DT0014` | Warning  | Schema generation failed unexpectedly for a type. It has no schema; please report it.                                       |
| `DT0015` | Error    | A type is configured by both a class `[DovetailConfiguration]` and an assembly `DovetailConfiguration<T>`.                 |

The generator runs in every project that references a Dovetail package, including those that only reference one
indirectly, so these can fire in transitive consumers. `DT0001` to `DT0005` are described under
[Source Generation](./source-generation).
