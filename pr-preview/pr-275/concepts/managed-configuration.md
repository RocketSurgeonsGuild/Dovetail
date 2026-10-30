# Managed Configuration

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
using Joinery.Dovetail.Attributes;

/// <summary>Options for the greetings this library prints.</summary>
[DovetailConfiguration]
public sealed class GreetingsOptions
{
    /// <summary>What the greeting says before the name.</summary>
    [Description("The text that comes before the name, e.g. \"Hello\".")]
    public string Message { get; set; } = "Hello";

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

| Attribute                               | Section                                                                                                                                                                     |
| --------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `[DovetailConfiguration]`               | The type name with one `Options`, `Settings`, `Configuration` or `Config` suffix removed: `GreetingsOptions` becomes `Greetings` (in the declared [key style](#key-style)). |
| `[DovetailConfiguration("Http:Retry")]` | The path you give. `:` nests sections. Keys are case-insensitive.                                                                                                           |
| `[assembly: DovetailConfiguration<T>]`  | The same rule as the class form, applied to `T`.                                                                                                                            |

There is no prefix. A name that would become empty after stripping the suffix (a class called `Options`) is an error
([`DT0008`](#diagnostics)); give it a path.

### Key style

`KeyStyle` on `[DovetailConfiguration]` (and on `DovetailConfiguration<T>`) picks how the keys of the section are spelled.
It is set per attribute; there is no assembly-wide or MSBuild default. There are five values:

| `KeyStyle`   | What the canonical key is                            | `MaxFileSizeMb`    | `HTTPServer`  | `HttpClientOptions` section |
| ------------ | ---------------------------------------------------- | ------------------ | ------------- | --------------------------- |
| `Undefined`  | The declared name exactly as written. The default.   | `MaxFileSizeMb`    | `HTTPServer`  | `HttpClient`                |
| `Pascal`     | The declared name with its first letter upper-cased. | `MaxFileSizeMb`    | `HTTPServer`  | `HttpClient`                |
| `Camel`      | Lower camel case (`JsonNamingPolicy.CamelCase`).     | `maxFileSizeMb`    | `httpServer`  | `httpClient`                |
| `Dash`       | Lower-case words joined with `-` (`KebabCaseLower`). | `max-file-size-mb` | `http-server` | `http-client`               |
| `Underscore` | Lower-case words joined with `_` (`SnakeCaseLower`). | `max_file_size_mb` | `http_server` | `http_client`               |

```csharp
[DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Underscore)]
public class HttpClientOptions // section "http_client"
{
    public int MaxFileSizeMb { get; set; } // key "max_file_size_mb"
    public RetryPolicy Retry { get; set; } = new(); // "retry:max_attempts" through RetryPolicy.MaxAttempts
}
```

- **`Undefined` respells nothing.** The binder literal, the schema's `properties` key, the strict closing of the section
  and the derived section path (after the suffix is stripped) are the names as declared. Configuration keys are
  case-insensitive, so data in any casing binds, as long as the letters are right: `maxfilesizemb`, `MaxFileSizeMb` and
  `MAXFILESIZEMB` are the same key. Dashes and underscores are other letters, so `max-file-size-mb` does not match
  `MaxFileSizeMb`.
- **`Pascal` is an explicit style.** It upper-cases the first letter of a declared name. C# property names already start
  in upper case, so for them `Undefined` and `Pascal` give identical output; they differ for a member declared as
  `myKey`, which stays `myKey` under `Undefined` and becomes `MyKey` under `Pascal`.
- **What it covers.** The key of every bound property and constructor parameter of the options type and of every object
  type nested in it (in lists and dictionary values too). The generated binder reads those keys, and the schema uses them
  for `properties`, the case-insensitive `patternProperties` twins and the strict `propertyNames`.
- **What it leaves alone.** Dictionary keys, enum values and every other piece of data are never restyled, and neither
  is an explicit section string (`"Http:Retry"` is used as written, segment by segment).
- **Word splitting** is `System.Text.Json`'s: `Camel` is `JsonNamingPolicy.CamelCase`, `Dash` is
  `JsonNamingPolicy.KebabCaseLower` and `Underscore` is `JsonNamingPolicy.SnakeCaseLower`, so `HTTPServer` is
  `httpServer`, `http-server` and `http_server`, and `Http2Enabled` is `http2Enabled`, `http2-enabled` and
  `http2_enabled`.
- **`[ConfigurationKeyName]` wins.** An explicit key name is used verbatim, in every style.
- **Derived section names** follow the style, after the `Options`, `Settings`, `Configuration` or `Config` suffix is
  removed: `GreetingsOptions` is `Greetings`, `greetings` and `greetings`; `HttpClientOptions` is `HttpClient`,
  `httpClient`, `http-client` and `http_client`.
- **Case.** Because keys ignore case, `Camel` against `Undefined` or `Pascal` only changes the spelling the schema
  suggests; `maxFileSizeMb` and `MaxFileSizeMb` bind the same value. `Dash` and `Underscore` change the letters the
  binder reads: a value written as `MaxFileSizeMb` no longer binds (`max-file-size-mb` or `max_file_size_mb` does), and the
  schema's `patternProperties` only accept those keys in any case.
- **One spelling per type.** The generated binder exists once for each type, so a type reached from sections that spell
  its keys differently, such as a `RetryOptions` used by a camel section and a dash section, can't be bound correctly.
  The later declaration is reported as [`DT0016`](#diagnostics) and produces nothing, the same way as `DT0015`. The
  comparison is of the effective keys, not of the style names: `Undefined` and `Pascal` do not conflict on ordinary
  names, which spell the same keys, but do on a member declared in lower case. Use the same `KeyStyle` for every section
  that reaches the type, or give the shared type `[ConfigurationKeyName]` attributes (a type whose keys don't depend on
  the style isn't a conflict). Sections with `RegisterOptions = false` bind nothing, so they can't conflict.

### One section per type

A type has exactly one section, so it is declared once. There are two attribute forms:

- `[DovetailConfiguration]` (or `[DovetailConfiguration("Section")]`) on the class. It can't be repeated on a type.
- `[assembly: DovetailConfiguration<T>("Section")]` for a type you don't own. Repeat it for each type; naming the same `T`
  twice is [`DT0015`](#diagnostics) as well.

Declaring a type with both forms is an error ([`DT0015`](#diagnostics)). The diagnostic is reported on the assembly
attribute; the class declaration is the one that produces the schema fragment and the registration, and the assembly
attribute produces nothing.

At run time each library reads only its own section from the application's complete `IConfiguration`, which the service
provider supplies when the options are first created, and creates or binds its options from it. Dovetail doesn't order,
de-duplicate or merge sections. The generated joint registers an assembly's types by section path, then type name, and
the standard options pipeline does the rest.

Every type is created by generated code and registered through an `IOptionsFactory<T>`, so your own `Configure<T>`,
`PostConfigure<T>` and validators run on the created instance, whenever they were registered.

### Registering by hand

The generated joint calls one public extension method on `IServiceCollection`, `AddDovetailConfigurationOptions`. You can
call it yourself, for example for a type declared with `RegisterOptions = false`. It returns the `OptionsBuilder<TOptions>`,
so `Validate` and `ValidateOnStart()` chain as usual.

| Overload                                                                          | Does                                                                                                                                                     |
| --------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `AddDovetailConfigurationOptions<T>(sectionPath, Func<IConfiguration, T> create)` | Creates the default `T` with `create`, from the section at `sectionPath`, and watches the configuration for reloads. Reflection free, so it is AOT safe. |

There is one overload because everything is created through generated create code: for a type with a public parameterless
constructor the generator emits `new T()` followed by the bind, and for the rest the constructor or initializer call. No
registration path uses reflection. To bind a type with reflection, use Microsoft's `Configure<T>(IConfiguration)` or
`AddOptions<T>().Bind(...)` yourself.

- The configuration is the application's complete `IConfiguration`, resolved from the service provider when the options are
  first created. `create` receives that configuration's section at `sectionPath` (such as `Http:Retry`). An empty path
  throws.
- A change token source watches the configuration, so `IOptionsMonitor<T>` and `IOptionsSnapshot<T>` see reloads of the
  default options.
- The call registers an `IOptionsFactory<T>` with `TryAdd`: the first registration for a type wins, and later calls are
  ignored. A factory your application registers itself wins over Dovetail's, so `create` is never called for that type.
- The factory derives from `OptionsFactory<T>`, so configure and post-configure actions and validators still run on the
  instance `create` returns. On .NET 11, don't give such a type asynchronous validators: they fail
  synchronous validation through a derived factory.

### Named options

Dovetail registers the default options only. Reading a name other than the default, such as
`IOptionsMonitor<T>.Get("tenant-a")`, throws an `InvalidOperationException` naming the type and the options name.

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
renames a key (see [Key style](#key-style) for restyling them all), and from binder 11 `[ConfigurationIgnore]` leaves a member out.

### Annotations that reach the schema

| Source                                      | Schema                                                    |
| ------------------------------------------- | --------------------------------------------------------- |
| `[Description]`, else the XML `<summary>`   | `description`. For a record parameter, its `<param>`.     |
| `[Range]`, length attributes                | `minimum`/`maximum`, `minLength`/`maxLength`, item counts |
| `[RegularExpression]`                       | `pattern`, when the expression can be anchored            |
| `[Url]`, `[EmailAddress]`                   | `format`                                                  |
| `[Obsolete]`                                | `deprecated` and a deprecation message                    |
| A constant initializer or parameter default | `default`, except an empty string                         |

Ranges and defaults are also written into the description, since editors don't show them. A default of `""` is
skipped, both as `default` and in the description, because "Default: ." tells nobody anything. The XML summary is read from
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

Types the options pipeline can't create (constructor parameters, `init`-only or `required` members) are registered like any
other type: the generated create code builds the instance from the type's section, and your `Configure<T>` and validators
still run on it.

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

Every project that isn't a host (a library) also writes its own fragments as `$(IntermediateOutputPath)ConfigurationSchema.json`
(permissive, rooted at the configuration root like Aspire's) and packs it at the NuGet package root, so Visual Studio and
other tools that understand Aspire's layout can use it. A multi-targeted library writes one per target framework and
packs the first one's. A library with no sections writes nothing.

### The merged application schema

Hosts are test projects (`IsTestProject`) and applications (detected by Joinery.Dovetail.Sdk, or `OutputType` of `Exe` or
`WinExe`). A host merges its own fragments and those of every referenced assembly into
`$(BaseIntermediateOutputPath)appsettings.schema.json`, normally `obj/appsettings.schema.json`.

- The file is a stable path editors can point at. A host that targets several frameworks writes that file from its first
  target framework, and `$(IntermediateOutputPath)appsettings.schema.json` (for example `obj/Debug/net10.0/`) from the
  others.
- It is copied to `$(OutDir)`, next to `appsettings.json`, and published as `appsettings.schema.json`. A project that
  references the host (a test project referencing an application) doesn't get a second copy.
- Hosts that embed their configuration (`DovetailEmbedConfiguration` is `true`, which Joinery.Dovetail.Sdk sets for MAUI and
  Uno applications) keep the file in `obj/` only: no copy to the output directory and no publishing.
- A hand-made `appsettings.schema.json` (Web SDK content, or one from a reference) is removed from the publish items so
  it doesn't collide with the generated one (NETSDK1152). The generated file is what gets published.

- **It is strict.** Each Dovetail section, and the object types it uses, accepts only its own keys. The check is a
  case-insensitive `propertyNames` pattern rather than `additionalProperties: false`, so `message` is accepted and
  `Mesage` is flagged, with an error that lists the known keys. The root, parents shared between sections, dictionaries
  and sections declared by more than one assembly stay open for keys of other sources.
- **It includes SchemaStore's `appsettings.json` schema**, embedded rather than referenced, so `Logging`, `Kestrel`
  and `ConnectionStrings` complete too, online or offline. The copy ships with Dovetail and is refreshed each release (Apache-2.0).
- **It is deterministic.** Sections compare ignoring case; when libraries spell a section differently, the
  ordinal-smallest spelling is displayed. A section several assemblies declare is merged into one object. The same
  references always produce a byte-identical file.

| Property                                        | Default  | Effect                                                                                                                                                                  |
| ----------------------------------------------- | -------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DovetailConfigurationSchema`                   | on       | `false` writes no schema files at all, and removes the ones an earlier build wrote.                                                                                     |
| `DovetailConfigurationSchemaHost`               | detected | `true` or `false` overrides which projects count as hosts.                                                                                                              |
| `DovetailConfigurationSchemaStrict`             | `true`   | `false` leaves Dovetail's sections open (no `propertyNames`) in the host file.                                                                                          |
| `DovetailConfigurationSchemaIncludeSchemaStore` | `true`   | `false` leaves SchemaStore's schema out of the host file.                                                                                                               |
| `DovetailEmbedConfiguration`                    | detected | `true` for hosts that embed their configuration (Joinery.Dovetail.Sdk sets it for MAUI and Uno): the schema stays in `obj/`, with no copy to the output and no publish. |
| `DovetailConfigurationSchemaRequired`           | `false`  | `true` lists required members in `required`. The generator reads this one, so it changes the fragments in every project; the MSBuild targets don't use it.              |

```xml
<PropertyGroup>
  <!-- Permissive: unknown keys under Dovetail sections aren't flagged -->
  <DovetailConfigurationSchemaStrict>false</DovetailConfigurationSchemaStrict>
</PropertyGroup>
```

> [!NOTE]
> Editors flag a quoted number such as `"Timeout": "30"` as an incorrect type, although the .NET binder accepts it.
> Dovetail keeps the schema strict about types so that ranges are always checked.

The build target runs after `CoreCompile` and skips design-time builds. It writes only when its inputs change: the
references, the project's own assembly and the properties above.
Merging runs in a compiled MSBuild task (`Joinery.Dovetail.Build.Tasks`) that reads the fragments with `System.Reflection.Metadata`,
so no assembly is loaded and nothing is embedded in your application.

### Serilog

`Joinery.Dovetail.Serilog` ships a schema for the `Serilog` section, so a host that references it gets completion and
validation for `Serilog` without any setup. The section is read by hand (see [Serilog](./serilog)), so no options type describes it
and the generator has nothing to emit. `Joinery.Dovetail.Serilog` carries the same `AssemblyMetadata` fragment as a hand-written
attribute instead, and it goes through the pipeline above unchanged: the library's `ConfigurationSchema.json` is packed at the package
root, and a host merges the fragment into its `appsettings.schema.json` like any other.

The schema describes what `Joinery.Dovetail.Serilog` reads, which is the minimum level and nothing else, because the rest of
Serilog.Settings.Configuration's format needs reflection that is not trimming or Native AOT safe. It models one key:

- **`MinimumLevel`**, as a level name (`Verbose`, `Debug`, `Information`, `Warning`, `Error` or `Fatal`; the names are matched
  exactly) or as an object with `Default` and `Override` (a map from a source such as `Microsoft.AspNetCore` to a level). A number,
  or an unknown name, is flagged.

Every other key of the section (`Using`, `WriteTo`, `AuditTo`, `Enrich`, `Filter`, `Destructure`, `Properties`, `LevelSwitches`,
`FilterSwitches` and `MinimumLevel`'s `ControlledBy`, or a level switch given as a value) is flagged by the strict merged schema
rather than accepted, so that configuration copied from Serilog's documentation, which would be ignored, does not look as if it were
working. The level rules follow the `Serilog` definition in SchemaStore's `appsettings.json` schema, the one the merged file embeds
by default, narrowed to the six levels (SchemaStore also lists `Off`, which Serilog has no level for) and with no switches. Both
apply in the merged file, so the fragment is what rejects the keys that are not read.

What else differs from SchemaStore's schema, and why it is harmless:

- **Casing.** Configuration keys ignore case, so the fragment, like every Dovetail fragment, accepts any casing of its keys.
  SchemaStore's schema lists them case-sensitively, and both apply in the merged file, so `minimumlevel` is flagged by
  SchemaStore's half.
- **Flat keys.** SchemaStore's schema accepts the flat forms `"WriteTo:Async"` and `"Enrich:0"`, which Serilog reads. They are
  flagged like the other keys that are not read. Set `DovetailConfigurationSchemaStrict` to `false` to leave the section open.

### Serving the schema

ASP.NET Core applications that reference Joinery.Dovetail.Hosting can serve the merged schema over HTTP at
`GET /.well-known/dovetail/configuration-schema`. It is off by default. Setting `DovetailConfigurationSchemaEndpoint` to
`true` generates a joint that maps it, and the `MapDovetailConfigurationSchema(fallbackDirectory, environments)` extension
on `IEndpointRouteBuilder`, which Joinery.Dovetail.Hosting emits into ASP.NET Core projects, maps it by hand. The properties, the response and the security trade-off are
described in [Editor setup](../guides/editor-setup#the-schema-endpoint).

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

| Id       | Severity | Meaning                                                                                                                                                                          |
| -------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DT0006` | Warning  | The project's C# version is older than 12, so Dovetail generated nothing. Set `LangVersion` to 12 or later.                                                                      |
| `DT0007` | Error    | The type can't be described as configuration: an interface, a static or abstract class, a struct or enum, an open generic type, or a collection.                                 |
| `DT0008` | Error    | The section isn't valid for the type: empty, an empty segment (`A::B`), or a name that is only a suffix such as `Options`.                                                       |
| `DT0009` | Warning  | A member has a type configuration binding doesn't support. It is left out of the schema and the binder.                                                                          |
| `DT0010` | Warning  | Two members bind the same key (keys are case-insensitive). Only the first is described.                                                                                          |
| `DT0011` | Error    | The type can't be created from configuration: no public constructor, several public constructors with parameters and none without, or a parameter that matches no property.      |
| `DT0012` | Warning  | The type graph uses more than 200 distinct types, so the schema stops describing them at the member named.                                                                       |
| `DT0013` | Warning  | An annotation has no JSON Schema equivalent and is left out.                                                                                                                     |
| `DT0014` | Warning  | Schema generation failed unexpectedly for a type. It has no schema; please report it.                                                                                            |
| `DT0015` | Error    | A type is configured by both a class `[DovetailConfiguration]` and an assembly `DovetailConfiguration<T>`. It is reported on the assembly attribute.                             |
| `DT0016` | Error    | A type is bound with more than one key style. The later declaration is reported and produces nothing; use the same `KeyStyle`, or give the shared type `[ConfigurationKeyName]`. |

The generator runs in every project that references a Dovetail package, including those that only reference one
indirectly, so these can fire in transitive consumers. `DT0001` to `DT0005` are described under
[Source Generation](./source-generation).