# Shipping Templates

# Shipping templates

A package can ship C# source that is written into a consuming project at build time, only when that project matches. This is how
`Joinery.Dovetail.Hosting` works: its ASP.NET Core, MAUI, Uno and Blazor WebAssembly helpers and its OpenTelemetry joints are all
templates, so the package depends on none of those frameworks. Referencing a framework is what turns its template on.

The mechanism is a supported extension point. Any package can declare templates with `<DovetailTemplate>` items and get the same
gating that `Joinery.Dovetail.Hosting` uses, including a mode that emits only into projects that reference your package
themselves.

## Declare a template

Put the source in a `*.cs.template` file next to your package's targets, and declare it from the targets (`build/` or
`buildTransitive/`):

```xml
<Project>
  <ItemGroup>
    <DovetailTemplate
      Include="HealthCheckJoint"
      File="$(MSBuildThisFileDirectory)HealthCheckJoint.cs.template"
      Package="Contoso.Hosting"
      DirectReferenceOnly="true"
      References="Microsoft.Extensions.Diagnostics.HealthChecks"
      Hosts="HostApplication;WebApplication" />
  </ItemGroup>
</Project>
```

Pack both files, under `build/` (imported by NuGet into projects that reference the package directly) or `buildTransitive/`
(imported into every project that gets it, which is what a package reached through other packages needs):
`<None Include="build/*.*" Pack="true" PackagePath="buildTransitive" />`.

| Metadata               | Meaning                                                                                                                                                                                                 |
| ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Include`              | The template's name. It must be unique across packages, and names the generated file, `DovetailTemplate.<name>.g.cs` in `obj/`.                                                                         |
| `File`                 | Required. The full path of the template.                                                                                                                                                                |
| `Package`              | The id of the package that ships the template. It is only emitted into a project that references it: the package's assembly (named after the package) is among the resolved references.                 |
| `DirectReferenceOnly`  | `true` limits the template to projects with a direct reference to `Package`. Requires `Package`.                                                                                                        |
| `References`           | Semicolon-separated assembly names (file names without extension). Every one must be among the project's resolved references.                                                                           |
| `Hosts`                | Semicolon-separated host names. At least one must be declared by the project: the host model that `Joinery.Dovetail.Sdk` detects (`HostApplication`, `WebApplication`, `WebAssembly`, `Maui` or `Uno`). |
| `HostName`, `HostType` | When `HostType` is set, `{HostName}` and `{HostType}` in the template are replaced, so one template can be declared once per host model under different names.                                          |
| `OutputPrefix`         | The generated file is `<prefix>.<name>.g.cs`; the default prefix is `DovetailTemplate`.                                                                                                                 |

Every gate you set has to pass. A project that doesn't run the Dovetail source generator gets no templates.

## Direct references only

By default a template reaches every project that has your package's assembly among its references, which includes projects that
only get the package because a library they use references it. `DirectReferenceOnly="true"` narrows that to projects that declare
the reference themselves:

- a `PackageReference` to `Package`, including the ones `Joinery.Dovetail.Sdk` adds for you, or
- a `ProjectReference` to a project named after `Package`, which is how a solution consumes what it ships.

A project that only receives the package transitively still imports your `buildTransitive/` targets, so your items are declared
there, but it gets no generated file. Use it for wiring that an application should opt into by referencing your package, and that a
library reaching the application through a project reference should not impose on it.

A package that ships from `build/` already gets this from NuGet for package references. The mode is for what NuGet can't express:
a package shipped from `buildTransitive/`, and a solution that consumes your project by `ProjectReference`. A project reference
imports no package assets at all, so the solution imports your targets itself, into every project (this repository does that with
`Joinery.Dovetail.Hosting.targets` for its test projects). `Package` is then what limits the templates to projects that reference
yours, and `DirectReferenceOnly` to the ones that reference it themselves.

## Write the template

- The generated code is part of the consuming project, compiled against whatever it references. Reference-gate everything it needs
  (`References`), and your package then depends on none of it.
- Make the types `internal` and `[Embedded]`, so an assembly never sees the copy a referenced assembly emitted for itself. A
  template may also declare joints (`[DovetailExport]`), which the Dovetail source generator picks up like any other.
- A template must not contain backslashes: MSBuild rewrites them to forward slashes on Linux and macOS when it writes the file.

## When the metadata isn't enough

The templates that pass their gates are the `DovetailEmittedTemplate` items, with the metadata above carried over. A target can add
to them, or remove from them, after `DovetailResolveTemplates` and before `DovetailGenerateTemplates`:

```xml
<Target Name="ChooseMyJoint" AfterTargets="DovetailResolveTemplates" BeforeTargets="DovetailGenerateTemplates">
  <ItemGroup>
    <DovetailEmittedTemplate
      Include="MyJoint"
      File="$(MSBuildThisFileDirectory)MyJoint.cs.template"
      Condition="@(DovetailEmittedTemplate->AnyHaveMetadataValue('Identity', 'MyHelpers')) == 'true'" />
  </ItemGroup>
</Target>
```

`Joinery.Dovetail.Hosting` does this to choose which of its OpenTelemetry joints a host gets, depending on which helpers were
emitted.

## Test it

The Dovetail SDK tests show how to prove a template's gates: pack a package that ships the template, reference it from a project
that declares the reference and from one that only receives it transitively, build, and read which `DovetailTemplate.*.g.cs` files
landed in each project's `obj/` (`test/Joinery.Dovetail.Sdk.Tests/TemplateExtensionPointTests.cs`).