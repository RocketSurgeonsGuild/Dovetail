# Configuration with Optionary

# Configuration with Optionary

Dovetail's options binding and configuration schema moved to their own package, [Optionary](https://rocketsurgeonsguild.github.io/Optionary/). Optionary's source generator
reads an options type and produces a reflection-free binder that registers it as `IOptions<T>`, and a JSON Schema fragment that its
build merges into each application's `appsettings.schema.json` for editor completion and validation. Optionary works without
Dovetail; Dovetail registers what Optionary generates for you and tells it which projects are applications.

Dovetail doesn't depend on Optionary. Add it to the projects that declare options:

```xml
<ItemGroup>
  <PackageReference Include="Optionary" Version="0.1.0" />
</ItemGroup>
```

Then put `[Optionary]` on the options type, or `[assembly: Optionary<T>("Section")]` for a type you don't own:

```c#
using Optionary;

/// <summary>Options for the greetings this library prints.</summary>
[Optionary]
public sealed class GreetingsOptions
{
    /// <summary>What the greeting says before the name.</summary>
    public string Message { get; set; } = "Hello";
}
```

Consume it the usual way, with `IOptions<GreetingsOptions>`, `IOptionsSnapshot<T>` or `IOptionsMonitor<T>`. Section names, key styles,
validation, supported types, the schema and editor setup are all described in [Optionary's documentation](https://rocketsurgeonsguild.github.io/Optionary/getting-started/).
The [`SdkSample.Greetings`](https://github.com/RocketSurgeonsGuild/Dovetail/tree/main/sample/SdkSample/SdkSample.Greetings)
project is a working example.

## What Dovetail adds

- **The joint.** Optionary emits `OptionaryConfigurationOptions.AddOptionaryConfigurationOptions(services)` in the project's
  `RootNamespace`. When an assembly declares at least one `[Optionary]` type, Dovetail's generator exports a joint that calls it, listed
  first so options are bound before other joints configure them in code. Every host that references the assembly registers its options,
  so there is nothing to call. Below C# 12 Dovetail emits nothing and reports `DT0006`.
- **Host detection.** An application the [Dovetail SDK](../guides/microsoft-sdks) detects (ASP.NET Core, the generic host, Blazor
  WebAssembly, MAUI, Uno) is an Optionary schema host: Dovetail sets `OptionarySchemaHost` to `true` unless the project set it.
- **Embedded hosts.** A host that embeds its configuration (`DovetailEmbedConfiguration`, which the SDK sets for MAUI and Uno
  applications) keeps the schema in `obj/` for editors: Dovetail sets `OptionarySchemaCopyToOutput` to `false`.
- **ZeroAlloc.** `DovetailEnableZeroAllocValidation=false` also sets `OptionaryZeroAllocValidation` to `false`, so Optionary doesn't call
  `ValidateWithZeroAlloc()` either. See [ZeroAlloc](./zeroalloc#validating-configuration).
- **Serilog.** `Joinery.Dovetail.Serilog` ships a schema fragment for the `Serilog` section, which Optionary merges into an
  application's schema like any other. See [Serilog](./serilog).
- **The schema endpoint**, below.
- **Configuration files.** Shipping a library's `appsettings` files in JSON, YAML or TOML stays in Dovetail: see
  [Configuration Files](./configuration-files).

These hook properties have no effect in a project that doesn't reference Optionary, and Dovetail adds no warnings about it: an
application without Optionary just has no schema.

## The schema endpoint

An application can also serve the schema Optionary writes, for tools that read it over HTTP. This needs an ASP.NET Core application
that references Joinery.Dovetail.Hosting, and is off by default:

```xml
<PropertyGroup>
  <DovetailConfigurationSchemaEndpoint>true</DovetailConfigurationSchemaEndpoint>
  <!-- Optional: serve it only in these environments -->
  <DovetailConfigurationSchemaEndpointEnvironments>Development;Staging</DovetailConfigurationSchemaEndpointEnvironments>
</PropertyGroup>
```

| Property                                            | Default             | Effect                                                                                                                                                                                                                                                         |
| --------------------------------------------------- | ------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DovetailConfigurationSchemaEndpoint`               | off                 | `true` generates a joint that maps the endpoint. Only projects with `OutputType` `Exe` or `WinExe` that reference ASP.NET Core and Joinery.Dovetail.Hosting get it.                                                                                            |
| `DovetailConfigurationSchemaEndpointEnvironments`   | every environment   | A semicolon-separated list of environment names (`Development;Staging`, compared ignoring case). The endpoint is mapped only when the host environment is one of them.                                                                                         |
| `DovetailConfigurationSchemaEndpointSourceFallback` | `true` except below | Whether the project's `obj/` directory is compiled into the application as a place to look for the schema. By default it is, except in publish builds and `ContinuousIntegrationBuild` builds, so a build machine path isn't shipped. `false` never embeds it. |

The route is `GET /.well-known/dovetail/configuration-schema`:

- It looks for `appsettings.schema.json` in the application's output directory first, then in the embedded `obj/` fallback
  directory, which is how it works when run from source with no copy beside the assembly. A published application serves
  the copy published with it.
- The response is `application/schema+json` with a strong `ETag` and `Cache-Control: no-cache`. A request with a matching
  `If-None-Match` gets `304 Not Modified`. The file is re-read when it changes on disk.
- It answers `404` when neither location has the file, which is always the case in an application that doesn't reference
  Optionary.

Point a mapping at the URL, for example `"url": "http://localhost:5000/.well-known/dovetail/configuration-schema"` in
`json.schemas`.

To map it yourself, call the extension on the `WebApplication` (or any `IEndpointRouteBuilder`) instead of setting the
property. Joinery.Dovetail.Hosting emits it into every ASP.NET Core project that references it, as an internal type. Both arguments are optional, and the method returns `null` when `environments` excludes the current
environment:

```c#
using Joinery.Dovetail.Hosting;

app.MapDovetailConfigurationSchema(fallbackDirectory: null, environments: "Development")
   ?.RequireAuthorization();
```

> [!WARNING]
> The schema lists every configuration key your application and its libraries accept, including the section names of
> libraries you may not want to advertise. Unless you restrict it, the endpoint is mapped in every environment, production
> included, and anyone who can reach the application can read it, with no authentication. Set
> `DovetailConfigurationSchemaEndpointEnvironments` to the environments that need it (usually `Development`), map it
> yourself and add authorization, or leave it off.

## Upgrading from Dovetail 0.6

This is a clean break: nothing old keeps working alongside the new names. Add the `Optionary` package to every project that declares
configuration, then rename:

| Dovetail 0.6                                                                | Optionary                                                                                                   |
| --------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `Joinery.Dovetail.Attributes.DovetailConfigurationAttribute`                | `Optionary.OptionaryAttribute`                                                                              |
| `Joinery.Dovetail.Attributes.DovetailConfigurationAttribute<T>`             | `Optionary.OptionaryAttribute<T>`                                                                           |
| `Joinery.Dovetail.Attributes.ConfigurationKeyStyle`                         | `Optionary.ConfigurationKeyStyle` (same values)                                                             |
| `DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions<T>` | `OptionaryServiceCollectionExtensions.AddOptionaryOptions<T>`                                               |
| Assembly metadata prefix `Dovetail.ConfigurationSchema.v1:`                 | `Optionary.Schema.v1:`                                                                                      |
| `DovetailConfigurationSchema`                                               | `OptionarySchema`                                                                                           |
| `DovetailConfigurationSchemaHost`                                           | `OptionarySchemaHost`                                                                                       |
| `DovetailConfigurationSchemaStrict`                                         | `OptionarySchemaStrict`                                                                                     |
| `DovetailConfigurationSchemaIncludeSchemaStore`                             | `OptionarySchemaIncludeSchemaStore`                                                                         |
| `DovetailConfigurationSchemaRequired`                                       | `OptionarySchemaRequired`                                                                                   |
| `DovetailConfigurationSchemaVisualStudio`                                   | `OptionarySchemaVisualStudio`                                                                               |
| `DovetailScaffoldEditorSettings`                                            | `OptionaryScaffoldEditorSettings`                                                                           |
| `DovetailEditorSettingsDirectory`                                           | `OptionaryEditorSettingsDirectory`                                                                          |
| `DovetailConfigurationReferenceDocumentation`                               | `OptionaryReferenceDocumentation`                                                                           |
| `AdditionalFiles` metadata `DovetailReferenceDocumentation`                 | `OptionaryReferenceDocumentation`                                                                           |
| `DT0007`–`DT0016`, `DT0018`                                                 | `OPT0002`–`OPT0012` ([diagnostics](https://rocketsurgeonsguild.github.io/Optionary/reference/diagnostics/)) |

- A library packed with Dovetail 0.6 or earlier carries `Dovetail.ConfigurationSchema.v1:` fragments, which Optionary doesn't read. Its
  sections are missing from the merged schema until it is rebuilt with Optionary. Binding is unaffected for libraries you rebuild:
  `[DovetailConfiguration]` no longer compiles, so nothing is lost silently.
- `DovetailEnableZeroAllocValidation`, `DovetailEmbedConfiguration` and the `DovetailConfigurationSchemaEndpoint*` properties keep their
  names: they are Dovetail's.
- The generated joint is now `OptionaryConfigurationOptionsJoint`; nothing should refer to it by name.