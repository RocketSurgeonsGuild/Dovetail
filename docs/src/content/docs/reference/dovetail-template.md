---
title: DovetailTemplate
description: The DovetailTemplate MSBuild item ships C# source from a package and has Dovetail write it into only the projects that should get it. Every metadata value, its default, the gates, the tokens metadata replaces, and where the generated file lands.
tags: [hosting, msbuild]
---

# DovetailTemplate

A `DovetailTemplate` item names a `*.cs.template` file that a package ships and says which projects should get a copy. When the
project's references have resolved, Dovetail checks every template's gates, writes the ones that pass to `obj/` and adds them to the
compilation. Nothing needs to be compiled against, so the shipping package depends on none of the assemblies its templates use:
referencing them is what turns a template on.

This page lists what the item accepts. [Shipping templates](../../guides/shipping-templates/) is the guide to using it: how to
write a template, how to pack the targets that declare it, and how to test the gates.

The item is expanded by MSBuild targets (`Joinery.Dovetail.Templates.targets`), not by the Roslyn source generator.

## Who declares one, and when

A package that wants to put C# into the projects that use it declares its templates from its `build/` or `buildTransitive/`
targets. `Joinery.Dovetail.Hosting` is the main user: its ASP.NET Core, MAUI, Uno and Blazor WebAssembly helpers and its
OpenTelemetry joints are all templates, which is why that package has no dependency on any of those frameworks. A solution can declare
templates in a `Directory.Build.targets` to the same effect.

Declare one when the code you ship has to be compiled into the consumer, because it uses types the consumer references and you do not.
Do not declare one when ordinary compiled code will do: a library the consumer references is simpler, and a template is only
worth it for what needs to be gated on the project.

A project that does not run the Dovetail source generator (`Joinery.Dovetail.Analyzers`) gets no templates, however they are gated,
since a template is meant to hold joints and `[Embedded]` helpers that the generator discovers.

## Declaring a template

The item's `Include` is the template's name. It names the generated file, so it has to be unique across every package a project
references. It is also what the gates and the generated file are keyed on, so a different `OutputPrefix` does not keep two templates
with one name apart. A package that declares templates from names its consumers choose, such as one per client, should prefix them
with its own name (`Include="Chirograph.RocketsClient"`) and pass the bare name as [metadata](#tokens).

```xml
<!-- a package's declaration, built by test/Joinery.Dovetail.Sdk.Tests/TemplateExtensionPointTests.cs -->
<DovetailTemplate Include="Plain" File="$(MSBuildThisFileDirectory)Plain.cs.template" Package="DovetailTemplateShipper" />
```

`File` is the only metadata that is required. Leave out `Package`, `References` and `Hosts` and the template goes into every project
that runs the generator.

## Metadata

Metadata is optional unless stated. A name the item does not read is ignored without a warning, so a misspelling falls back to the
default. Every gate you set has to pass; a gate you leave out does not restrict anything. Boolean values are compared with `true`
ignoring case; any other non-empty value counts as false.

| Metadata              | Default            | What it changes                                                                                                                                                                                                                                                                                                      |
| --------------------- | ------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `File`                | none; required     | The full path of the template, usually `$(MSBuildThisFileDirectory)Name.cs.template`. A template without one, or whose file does not exist, fails the build. Backslashes in it are written as they are.                                                                                                              |
| `Package`             | empty              | The id of the package that ships the template. The template is only written into a project that references it: the package's assembly, which is named after the package, is among the project's resolved references (or, with `DirectReferenceOnly`, the project declares the reference itself).                     |
| `DirectReferenceOnly` | empty (false)      | `true` limits the template to projects with a direct reference to `Package`: a `PackageReference` to it (the ones the Dovetail SDK adds count), or a `ProjectReference` to a project named after it. A project that only gets the package transitively gets nothing. Requires `Package`; without it the build fails. |
| `References`          | empty              | Semicolon-separated assembly names (file names without extension). Every one has to be among the project's resolved references.                                                                                                                                                                                      |
| `Hosts`               | empty              | Semicolon-separated `DovetailHost` names, at least one of which the project has to declare: the host model, as `Joinery.Dovetail.Sdk` detects it (`HostApplication`, `WebApplication`, `WebAssembly`, `Maui`, `Uno`). These are the hosts declared when the templates resolve, before a package narrows them.        |
| `HostName`            | empty              | Replaces `{HostName}` and `__HostName__` in the template, but only when `HostType` is set too.                                                                                                                                                                                                                       |
| `HostType`            | empty              | Replaces `{HostType}` and `__HostType__` in the template, and switches the `HostName` replacement on. Together they let one template be emitted once per host model under different names.                                                                                                                           |
| `OutputPrefix`        | `DovetailTemplate` | The prefix of the generated file name, `<prefix>.<name>.g.cs`.                                                                                                                                                                                                                                                       |

The gates are applied together, and a template that fails any of them is not written. They are read once the project's references
have resolved, which is right after `ResolveAssemblyReferences` and before the compiler runs.

## Tokens

Every metadata value of the item replaces two tokens in the template: `{Name}` and `__Name__`, where `Name` is the metadata's name.
That holds for the metadata above (`{OutputPrefix}`, `__Package__`, ...) and for any metadata you add yourself, which the table does not
list because the item reads it only here:

```xml
<!-- the declaration test/Joinery.Dovetail.Sdk.Tests/TemplateExtensionPointTests.cs builds, less the metadata it adds to cover more tokens -->
<DovetailTemplate Include="Chirograph.RocketsClient" File="$(MSBuildThisFileDirectory)Client.cs.template" ClientName="RocketsClient" Namespace="MyApp.Rockets" />
```

```csharp
// Client.cs.template: written as DovetailTemplate.Chirograph.RocketsClient.g.cs
namespace __Namespace__;

internal static class __ClientName__ { }
// {ClientName} in {Namespace}
```

- **Use `__Name__` where C# needs an identifier.** `__ClientName__` is a valid class name, so the template stays a C# file an editor
  can read; `{ClientName}` is for comments, strings and anywhere else an identifier is not needed.
- **A token no metadata fills is left as it is.** So is the token of a metadata whose value is empty, and `{HostName}` while
  `HostType` is empty. Metadata names compare ignoring case, as MSBuild's do.
- **Values are written as they are.** Nothing is escaped for C#: a value that ends up inside a string literal has to be valid there.
- **The replacement is one pass.** A value that contains a token is not expanded again.

Declaring one `File` under several `Include`s, each with its own metadata, writes one file per `Include`; that is how a package emits
one joint per client from a single template.

### Knowing that tokens are expanded

`Joinery.Dovetail` sets `DovetailTemplateMetadataTokens` to `true` in the projects it reaches. A version of Dovetail without it only
replaces `{HostName}` and `{HostType}`, so a package that depends on other tokens should declare those templates only when the
property is set:

```xml
<ItemGroup Condition="'$(DovetailTemplateMetadataTokens)' == 'true'">
  <DovetailTemplate Include="RocketsClient" File="$(MSBuildThisFileDirectory)Client.cs.template" ClientName="RocketsClient" />
</ItemGroup>
```

The property is set from Dovetail's `.props`, so it is there before any package's `.targets` declare their items.

## What is generated

Each template that passes is written to `$(IntermediateOutputPath)`, which is `obj/<Configuration>/<TargetFramework>/`, as an
ordinary `Compile` item:

| File                   | Contents                                                                                                               |
| ---------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| `<prefix>.<name>.g.cs` | The template's text, with its [tokens](#tokens) replaced. The prefix is `OutputPrefix`, `DovetailTemplate` by default. |

The item adds no namespace, type or method of its own: the template is the C#. For `Joinery.Dovetail.Hosting`, which sets
`OutputPrefix="DovetailHosting"`, a generic-host application gets `obj/Debug/net10.0/DovetailHosting.DovetailHostApplicationHelpers.g.cs`.

The templates that passed are the `DovetailEmittedTemplate` items, with the metadata above carried over. A target can add to them
or remove from them between `DovetailResolveTemplates` and `DovetailGenerateTemplates`; the guide shows how.

## Examples

### Minimal

The declaration above is the one the extension-point tests build. They pack a package that ships it, reference it from projects that
declare the reference themselves and from one that only receives it transitively, and read which `DovetailTemplate.*.g.cs` files land in
each `obj/`.

### A package that ships many

`Joinery.Dovetail.Hosting` lists its helpers with the assemblies each needs, then declares every one as a template for the package:

```xml
<!-- src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets (verbatim) -->
  <ItemGroup>
    <_DovetailHostingHelper Include="DovetailHostApplicationHelpers" References="Microsoft.Extensions.Hosting" />
    <_DovetailHostingHelper Include="DovetailHostLifecycleHelpers" References="Microsoft.Extensions.Hosting" />
    <_DovetailHostingHelper Include="DovetailMauiHelpers" References="Microsoft.Maui" />
    <_DovetailHostingHelper Include="DovetailUnoHelpers" References="Uno.Extensions.Hosting.WinUI" />
    <_DovetailHostingHelper Include="DovetailUnoNavigationHelpers" References="Uno.Extensions.Hosting.WinUI;Uno.Extensions.Navigation.UI" />
    <_DovetailHostingHelper Include="DovetailWebAssemblyHelpers" References="Microsoft.AspNetCore.Components.WebAssembly" />
    <_DovetailHostingHelper Include="DovetailConfigurationSchemaEndpointExtensions" References="Microsoft.AspNetCore" />
    <_DovetailHostingHelper Include="DovetailPipelineHelpers" References="ModularPipelines;Joinery.Dovetail.ModularPipelines" />
  </ItemGroup>
  <ItemGroup>
    <DovetailTemplate
      Include="@(_DovetailHostingHelper)"
      File="$(MSBuildThisFileDirectory)%(Identity).cs.template"
      Package="Joinery.Dovetail.Hosting"
      OutputPrefix="DovetailHosting"
    />
  </ItemGroup>
```

`References` carries over from
`_DovetailHostingHelper` to each template, so a project gets `DovetailMauiHelpers` only when it references `Microsoft.Maui`.

### One template for several hosts

The OpenTelemetry start joint is one template emitted once per host model, which is what `HostName` and `HostType` are for. The
package adds the instances to `DovetailEmittedTemplate` itself, because which of them a host needs depends on helpers that have
already been resolved:

```xml
<!-- src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets: one of the items of an ItemGroup, verbatim apart from the indentation -->
<DovetailEmittedTemplate
  Include="DovetailOpenTelemetryMauiStartJoint"
  Condition="'$(_DovetailHostingMaui)' == 'true'"
  File="$(MSBuildThisFileDirectory)DovetailOpenTelemetryStartJoint.cs.template"
  OutputPrefix="DovetailHosting"
  HostName="Maui"
  HostType="global::Microsoft.Maui.Hosting.MauiApp" />
```

### The gates, built

`test/Joinery.Dovetail.Sdk.Tests/DovetailItemReferenceTests.cs` builds a project that declares the following, and checks which files
land in `obj/` (the project is a generic-host application that references `Microsoft.Extensions.Hosting`; its templates are `Plain.cs.template`, and so on):

```xml
<DovetailTemplate Include="Plain" File="$(MSBuildThisFileDirectory)Plain.cs.template" />
<DovetailTemplate Include="Prefixed" File="$(MSBuildThisFileDirectory)Prefixed.cs.template" OutputPrefix="Contoso" />
<DovetailTemplate Include="Referenced" File="$(MSBuildThisFileDirectory)Referenced.cs.template" References="Microsoft.Extensions.Hosting" />
<DovetailTemplate Include="MissingReference" File="$(MSBuildThisFileDirectory)MissingReference.cs.template" References="Microsoft.Extensions.Hosting;Not.Referenced.Anywhere" />
<DovetailTemplate Include="ForThisHost" File="$(MSBuildThisFileDirectory)ForThisHost.cs.template" Hosts="Maui;HostApplication" />
<DovetailTemplate Include="ForOtherHosts" File="$(MSBuildThisFileDirectory)ForOtherHosts.cs.template" Hosts="Maui;Uno" />
<DovetailTemplate Include="PerHost" File="$(MSBuildThisFileDirectory)PerHost.cs.template" HostName="Mine" HostType="global::System.Object" />
<DovetailTemplate Include="NameOnly" File="$(MSBuildThisFileDirectory)NameOnly.cs.template" HostName="Mine" />
```

`Plain`, `Referenced`, `ForThisHost`, `PerHost` and `NameOnly` are written, `Prefixed` as `Contoso.Prefixed.g.cs`; `MissingReference`
and `ForOtherHosts` are not. In `PerHost` `{HostName}` and `{HostType}` are replaced; in `NameOnly` the text `{HostName}` is left as it is.

## Common mistakes

- **`Include` is the file name, so it has to be unique across packages.** Two templates with one name write one file, whatever
  their `OutputPrefix`; prefix the names your package makes up with the package's name.
- **`File` has to be a full path.** Use `$(MSBuildThisFileDirectory)`, which is the directory of the targets file that declares the
  item. A missing `File` fails the build with `the template '<name>' has no File`, and a path that does not exist with
  `names a file that doesn't exist`.
- **`DirectReferenceOnly` without `Package` fails the build** (`is DirectReferenceOnly, so it needs the Package it is direct to`).
- **`Package` is matched against an assembly name.** It is the package's id, and it has to be the name of the package's assembly as
  well, since that is what the project's resolved references are searched for.
- **`HostName` alone does nothing.** `{HostName}` is replaced only when `HostType` is set; set both.
- **`Hosts` names are `DovetailHost` names.** Each is matched, by name, against the `DovetailHost` items declared when the templates
  resolve, which are the ones `Joinery.Dovetail.Sdk` detected (`HostApplication`, `WebApplication`, `WebAssembly`, `Maui`, `Uno`) plus
  any you declared. See [DovetailHost](../dovetail-host/).
- **A transitive reference declares your items but gets no file under `DirectReferenceOnly`.** Its `buildTransitive/` targets
  still run, so the `DovetailTemplate` items are there; the gate is what leaves the file out.
- **A misspelled metadata name is ignored.** The default applies, with no warning, and its tokens stay in the file.
- **A token in a template that does not set its metadata is not an error.** `{Unknown}` is written as it is; if it is in code, the
  compiler is what complains.
- **Templates do not reach projects that skip the generator.** A project without `Joinery.Dovetail.Analyzers` gets none.

## See also

- [Shipping templates](../../guides/shipping-templates/): the guide to writing, packing and testing templates.
- [DovetailHost](../dovetail-host/): the hosts that `Hosts` refers to, and the helpers `Joinery.Dovetail.Hosting` ships as templates.
- [DovetailJoint](../dovetail-joint/): the joint kinds a template can declare joints of.
