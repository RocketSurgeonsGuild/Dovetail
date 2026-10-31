---
title: DovetailTemplate
description: The DovetailTemplate MSBuild item ships C# source from a package and has Dovetail write it into only the projects that should get it. Every metadata value, its default, the gates, and where the generated file lands.
tags: [hosting, source-generator]
---

# DovetailTemplate

A `DovetailTemplate` item names a `*.cs.template` file that a package ships and says which projects should get a copy. When the
project's references have resolved, Dovetail checks every template's gates, writes the ones that pass to `obj/` and adds them to the
compilation. Nothing needs to be compiled against, so the shipping package depends on none of the assemblies its templates use:
referencing them is what turns a template on.

This page lists what the item accepts. [Shipping templates](/guides/shipping-templates/) is the guide to using it: how to
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
references.

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
| `File`                | none; required     | The full path of the template, usually `$(MSBuildThisFileDirectory)Name.cs.template`. A template without one, or whose file does not exist, fails the build. It must not contain backslashes: MSBuild rewrites them to forward slashes on Linux and macOS when the file is written.                                  |
| `Package`             | empty              | The id of the package that ships the template. The template is only written into a project that references it: the package's assembly, which is named after the package, is among the project's resolved references (or, with `DirectReferenceOnly`, the project declares the reference itself).                     |
| `DirectReferenceOnly` | empty (false)      | `true` limits the template to projects with a direct reference to `Package`: a `PackageReference` to it (the ones the Dovetail SDK adds count), or a `ProjectReference` to a project named after it. A project that only gets the package transitively gets nothing. Requires `Package`; without it the build fails. |
| `References`          | empty              | Semicolon-separated assembly names (file names without extension). Every one has to be among the project's resolved references.                                                                                                                                                                                      |
| `Hosts`               | empty              | Semicolon-separated `DovetailHost` names, at least one of which the project has to declare: the host model, as `Joinery.Dovetail.Sdk` detects it (`HostApplication`, `WebApplication`, `WebAssembly`, `Maui`, `Uno`). These are the hosts declared when the templates resolve, before a package narrows them.        |
| `HostName`            | empty              | Replaces `{HostName}` in the template, but only when `HostType` is set too.                                                                                                                                                                                                                                          |
| `HostType`            | empty              | Replaces `{HostType}` in the template, and switches the `{HostName}` replacement on. Together they let one template be emitted once per host model under different names.                                                                                                                                            |
| `OutputPrefix`        | `DovetailTemplate` | The prefix of the generated file name, `<prefix>.<name>.g.cs`.                                                                                                                                                                                                                                                       |

The gates are applied together, and a template that fails any of them is not written. They are read once the project's references
have resolved, which is right after `ResolveAssemblyReferences` and before the compiler runs.

## What is generated

Each template that passes is written to `$(IntermediateOutputPath)`, which is `obj/<Configuration>/<TargetFramework>/`, as an
ordinary `Compile` item:

| File                   | Contents                                                                                                                                              |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| `<prefix>.<name>.g.cs` | The template's text, with `{HostName}` and `{HostType}` replaced when `HostType` is set. The prefix is `OutputPrefix`, `DovetailTemplate` by default. |

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
<!-- src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets -->
<ItemGroup>
  <_DovetailHostingHelper Include="DovetailHostApplicationHelpers" References="Microsoft.Extensions.Hosting" />
  <_DovetailHostingHelper Include="DovetailMauiHelpers" References="Microsoft.Maui" />
  <_DovetailHostingHelper Include="DovetailUnoHelpers" References="Uno.Extensions.Hosting.WinUI" />
  <_DovetailHostingHelper Include="DovetailWebAssemblyHelpers" References="Microsoft.AspNetCore.Components.WebAssembly" />
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

(Abridged: the real list also has the host lifecycle, Uno navigation and ASP.NET Core schema endpoint helpers.) `References` carries over from
`_DovetailHostingHelper` to each template, so a project gets `DovetailMauiHelpers` only when it references `Microsoft.Maui`.

### One template for several hosts

The OpenTelemetry start joint is one template emitted once per host model, which is what `HostName` and `HostType` are for. The
package adds the instances to `DovetailEmittedTemplate` itself, because which of them a host needs depends on helpers that have
already been resolved:

```xml
<!-- src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets -->
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

- **`Include` is the file name, so it has to be unique across packages.** Two templates with one name write one file.
- **`File` has to be a full path.** Use `$(MSBuildThisFileDirectory)`, which is the directory of the targets file that declares the
  item. A missing `File` fails the build with `the template '<name>' has no File`, and a path that does not exist with
  `names a file that doesn't exist`.
- **`DirectReferenceOnly` without `Package` fails the build** (`is DirectReferenceOnly, so it needs the Package it is direct to`).
- **`Package` is matched against an assembly name.** It is the package's id, and it has to be the name of the package's assembly as
  well, since that is what the project's resolved references are searched for.
- **`HostName` alone does nothing.** `{HostName}` is replaced only when `HostType` is set; set both.
- **`Hosts` names are the SDK's, not yours.** They are matched against the `DovetailHost` items declared when the templates resolve,
  so a host you declare yourself under another name does not satisfy `Hosts="HostApplication"`. See [DovetailHost](/reference/dovetail-host/).
- **A transitive reference declares your items but gets no file under `DirectReferenceOnly`.** Its `buildTransitive/` targets
  still run, so the `DovetailTemplate` items are there; the gate is what leaves the file out.
- **A misspelled metadata name is ignored.** The default applies, with no warning.
- **Templates do not reach projects that skip the generator.** A project without `Joinery.Dovetail.Analyzers` gets none.

## See also

- [Shipping templates](/guides/shipping-templates/): the guide to writing, packing and testing templates.
- [DovetailHost](/reference/dovetail-host/): the hosts that `Hosts` refers to, and the helpers `Joinery.Dovetail.Hosting` ships as templates.
- [DovetailJoint](/reference/dovetail-joint/): the joint kinds a template can declare joints of.
