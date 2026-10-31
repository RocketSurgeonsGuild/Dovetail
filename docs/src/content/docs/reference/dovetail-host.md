---
title: DovetailHost
description: The DovetailHost MSBuild item declares the ConfigureDovetail entry points for one host builder type. Which hosts the SDK declares for you, every metadata value and its default, and what is generated.
tags: [hosting, source-generator]
---

# DovetailHost

A `DovetailHost` item declares the **entry points of one host**: the `ConfigureDovetail` extension methods on its builder type.
Calling one creates the Dovetail context, runs every joint the application imports, and builds the host.

```c#
var host = await Host.CreateApplicationBuilder(args).ConfigureDovetail();
```

The item is expanded by MSBuild targets, not by the Roslyn source generator: the C# is written to `obj/` before the compiler
runs, so you can open it in your IDE like any other source file.

## Who declares one, and when

In most projects, nobody: `Joinery.Dovetail.Sdk` looks at the project and declares the host for you. You declare your own when
the SDK does not, which is one of:

- the project does not use `Joinery.Dovetail.Sdk` but references `Joinery.Dovetail`. `sample/Sample`, the benchmarks and most test
  projects declare their host by hand;
- the host is one the SDK does not detect. Aspire's `IDistributedApplicationBuilder` is declared by hand in
  `test/Joinery.Dovetail.Aspire.Tests`; the SDK has that case commented out.

Do not declare a host the SDK already did. A second host on the same builder type makes `ConfigureDovetail` ambiguous (see
[common mistakes](#common-mistakes)) and gives you two copies of the entry points.

### The hosts the SDK declares

`Joinery.Dovetail.Sdk` declares at most one host, the first of these that matches the project (`Joinery.Dovetail.HostDetection.targets`).
A library matches none, and gets no entry points. Each also adds the `Joinery.Dovetail.Hosting` package, which emits the helpers
the entry points call.

| Host name         | The project is                                                  | `HostBuilderType`                                                            | `HostType`                                                            | Helper                                     |
| ----------------- | --------------------------------------------------------------- | ---------------------------------------------------------------------------- | --------------------------------------------------------------------- | ------------------------------------------ |
| `Maui`            | `UseMaui` and an `Exe` or `WinExe`                              | `Microsoft.Maui.Hosting.MauiAppBuilder`                                      | `Microsoft.Maui.Hosting.MauiApp`                                      | `DovetailMauiHelpers`                      |
| `Uno`             | an Uno.Sdk application whose `UnoFeatures` include `Hosting`    | `Uno.Extensions.Hosting.IApplicationBuilder`                                 | `Microsoft.Extensions.Hosting.IHost`                                  | `DovetailUnoHelpers`                       |
| `WebAssembly`     | a Blazor WebAssembly or `Microsoft.NET.Sdk.WebAssembly` project | `Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder` | `Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost` | `DovetailWebAssemblyHelpers` (the default) |
| `WebApplication`  | a `Microsoft.NET.Sdk.Web` or `Microsoft.NET.Sdk.Razor` project  | `Microsoft.AspNetCore.Builder.WebApplicationBuilder`                         | `Microsoft.AspNetCore.Builder.WebApplication`                         | `DovetailHostApplicationHelpers`           |
| `HostApplication` | a `Microsoft.NET.Sdk.Worker` project, or any other `Exe`        | `Microsoft.Extensions.Hosting.HostApplicationBuilder`                        | `Microsoft.Extensions.Hosting.IHost`                                  | `DovetailHostApplicationHelpers`           |

An Uno application without `Hosting` has no host and gets a build warning. MAUI is matched first, because a MAUI application is also
an `Exe`.

## Declaring a host

The item's `Include` is the host's name. It becomes part of the generated class and file names, and, unless you set `HelperName`, of
the name of the helper the code calls, so it has to be a valid C# identifier and it must be unique in the project.

```xml
<!-- test/Joinery.Dovetail.Hosting.Tests/Joinery.Dovetail.Hosting.Tests.csproj -->
<DovetailHost
    Include="HostApplication"
    HostBuilderType="Microsoft.Extensions.Hosting.HostApplicationBuilder"
    HostBuilderName="builder"
    HostType="Microsoft.Extensions.Hosting.IHost"
/>
```

`HostBuilderType` and `HostType` are all a host needs, and the project needs a helper of the default name (`HostApplication` is named for
the one `Joinery.Dovetail.Hosting` emits for the generic host).

## Metadata

Metadata is optional unless stated. A name the item does not read is ignored without a warning, so a misspelling falls back to the
default. Types are written into the generated code exactly as you give them, so write them with `global::`, as the SDK does for builder types. Boolean
values are compared with `true` ignoring case; any other non-empty value counts as false.

| Metadata             | Default                   | What it changes                                                                                                                                                                                                                                                       |
| -------------------- | ------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Namespace`          | `$(RootNamespace)`        | The namespace of the generated class, and so the namespace a caller imports for `ConfigureDovetail`.                                                                                                                                                                  |
| `HostBuilderType`    | none; required            | The type the `ConfigureDovetail` extension methods extend (and `Task` of it). Written into every signature.                                                                                                                                                           |
| `HostType`           | none; required            | The type the methods return, inside a `ValueTask<>`.                                                                                                                                                                                                                  |
| `HostBuilderName`    | empty                     | Read and handed to the template, which has no use for it. It changes nothing in the generated code today; the SDK sets it to `builder`.                                                                                                                               |
| `HelperName`         | `Dovetail{Name}Helpers`   | The static class whose `Configure` method the generated code calls, written as given. `Joinery.Dovetail.Hosting` emits `DovetailHostApplicationHelpers`, `DovetailMauiHelpers`, `DovetailUnoHelpers` and `DovetailWebAssemblyHelpers`; any other host brings its own. |
| `BuildExpression`    | `, static b => b.Build()` | The arguments spliced into that call right after `builder`, so it begins with a comma. The default is the delegate that builds the host. MAUI and Uno add their application assembly.                                                                                 |
| `HasBuildExpression` | `true`                    | `false` calls `Configure` without the `BuildExpression`, even one you set, for helpers that build the host themselves.                                                                                                                                                |
| `SyncEntryPoint`     | empty (false)             | Internal. `true` also emits blocking `UseDovetail` overloads. Only the SDK's MAUI detection sets it.                                                                                                                                                                  |
| `NavigateEntryPoint` | empty (false)             | Internal. `true` also emits `ConfigureDovetail<TShell>` overloads that navigate. Only the SDK's Uno detection sets it.                                                                                                                                                |

### Internal metadata

`SyncEntryPoint` and `NavigateEntryPoint` are set by the SDK's MAUI and Uno detection. They are documented so the generated files make
sense, not as a supported way to extend a host.

- `SyncEntryPoint="true"` adds `UseDovetail` (from `DovetailHostSync.cs.template`) next to `ConfigureDovetail`, for hosts whose bootstrap
  method cannot be async (`MauiProgram.CreateMauiApp`). It blocks on `ConfigureDovetail`, which is safe only because Dovetail never
  resumes on the calling thread's context: joints must not post back to the UI thread during startup.
- `NavigateEntryPoint="true"` adds `ConfigureDovetail<TShell>` (from `DovetailHostNavigate.cs.template`), for Uno.Extensions.Navigation
  applications that start with `NavigateAsync<TShell>()` instead of `Build()`. `Joinery.Dovetail.Hosting` sets it back to `false` when
  the project does not reference `Uno.Extensions.Navigation`.

## What is generated

Each host is written to `$(IntermediateOutputPath)`, which is `obj/<Configuration>/<TargetFramework>/`, as ordinary `Compile` items:

| File                                | Contents                                                                                                                                           |
| ----------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DovetailHost.{Name}.g.cs`          | The `ConfigureDovetail` entry points.                                                                                                              |
| `DovetailHost.{Name}.Sync.g.cs`     | The `UseDovetail` entry points. Only with `SyncEntryPoint`.                                                                                        |
| `DovetailHost.{Name}.Navigate.g.cs` | The `ConfigureDovetail<TShell>` entry points. Only with `NavigateEntryPoint`.                                                                      |
| `DovetailContextBuilder.g.cs`       | `DovetailContextBuilder.Create()`, once for the project as soon as it declares any host: a builder populated with the joints the assembly imports. |

All of them are in `Namespace`, in `internal static partial class GeneratedDovetail{Name}Extensions`. They are internal, so each assembly
that hosts an application declares its own, and a test project that hosts one needs its own `DovetailHost` too.

`ConfigureDovetail` has the same five forms on the builder and on a `Task` of it (so a builder you are still awaiting can be chained),
each returning `ValueTask<{HostType}>`:

```c#
ConfigureDovetail(this {HostBuilderType} builder, CancellationToken cancellationToken = default)
ConfigureDovetail(this {HostBuilderType} builder, Action<DovetailContextBuilder> action, CancellationToken cancellationToken = default)
ConfigureDovetail(this {HostBuilderType} builder, Func<DovetailContextBuilder, ValueTask> action, CancellationToken cancellationToken = default)
ConfigureDovetail(this {HostBuilderType} builder, Func<DovetailContextBuilder, CancellationToken, ValueTask> action, CancellationToken cancellationToken = default)
ConfigureDovetail(this {HostBuilderType} builder, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken = default)
```

Every one ends in the same call, which is where `HelperName`, `BuildExpression` and `HasBuildExpression` apply:

```c#
return await {HelperName}.Configure(builder{BuildExpression}, contextBuilder, cancellationToken);
```

`UseDovetail` has the same forms on the builder only and returns the `{HostType}` itself. `ConfigureDovetail<TShell>` has them on the builder
only, with `where TShell : Microsoft.UI.Xaml.UIElement, new()`.

## Examples

### A host the SDK declares

For a MAUI application the SDK declares this, and `UseDovetail` is the blocking entry point it adds for `MauiProgram.CreateMauiApp`
(`src/Joinery.Dovetail.Sdk/Sdk/Joinery.Dovetail.HostDetection.targets`):

```xml
<DovetailHost Include="Maui"
              HostBuilderType="global::Microsoft.Maui.Hosting.MauiAppBuilder"
              HostBuilderName="builder"
              HostType="global::Microsoft.Maui.Hosting.MauiApp"
              HelperName="global::Joinery.Dovetail.DovetailMauiHelpers"
              BuildExpression=", static b => b.Build(), typeof(GeneratedDovetailMauiExtensions).Assembly"
              SyncEntryPoint="true" />
```

The generated call is `await global::Joinery.Dovetail.DovetailMauiHelpers.Configure(builder, static b => b.Build(), typeof(GeneratedDovetailMauiExtensions).Assembly, contextBuilder, cancellationToken)`.
The Uno declaration has the same shape with `DovetailUnoHelpers` and `NavigateEntryPoint="true"` in place of `SyncEntryPoint`. The other three
set only the three types, and a `HelperName` for `WebApplication` and `HostApplication`.

### A project without the SDK

`sample/Sample` references `Joinery.Dovetail.Hosting` directly and declares the generic host itself:

```xml
<!-- sample/Sample/Sample.csproj -->
<DovetailHost
    Include="HostApplication"
    HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder"
    HostBuilderName="builder"
    HostType="global::Microsoft.Extensions.Hosting.IHost"
    HelperName="global::Joinery.Dovetail.DovetailHostApplicationHelpers"
/>
```

```c#
// sample/Sample/Program.cs
await builder
     .ConfigureDovetail(z => z.ConfigureCommandLine(configurator => configurator.AddCommand<Dump>("dump")))
     .RunConsoleAppAsync();
```

### A host of your own: Aspire

The Aspire helper builds the host itself, so its `Configure` takes no build delegate, and its tests declare the host with
`HasBuildExpression="false"`. The helper is `DovetailDistributedApplicationHelpers`, which is the default for a host named
`DistributedApplication`, so there is no `HelperName`:

```xml
<!-- test/Joinery.Dovetail.Aspire.Tests/Joinery.Dovetail.Aspire.Tests.csproj -->
<DovetailHost
    Include="DistributedApplication"
    Namespace="Joinery.Dovetail.Joints"
    HostBuilderType="global::Aspire.Hosting.IDistributedApplicationBuilder"
    HostBuilderName="builder"
    HostType="global::Aspire.Hosting.DistributedApplication"
    HasBuildExpression="false"
/>
```

```c#
// src/Joinery.Dovetail.Aspire/RocketDistributedApplicationExtensions.cs
public static async ValueTask<DistributedApplication> Configure(
    IDistributedApplicationBuilder builder,
    DovetailContextBuilder contextBuilder,
    CancellationToken cancellationToken
)
```

### Your own helper

When nothing ships a helper for your builder, write one with the shape the call expects. The three declarations below are compiled by
`test/Joinery.Dovetail.Sdk.Tests/DovetailItemReferenceTests.cs`, together with these helpers:

```xml
<DovetailHost Include="Mine" HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder" HostType="global::Microsoft.Extensions.Hosting.IHost" />
<DovetailHost Include="Custom" Namespace="Hosts.Custom" HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder" HostType="global::Microsoft.Extensions.Hosting.IHost" HelperName="global::worker.CustomHelper" BuildExpression=", static b => b.Build(), &quot;extra&quot;" />
<DovetailHost Include="NoBuild" Namespace="Hosts.NoBuild" HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder" HostType="global::Microsoft.Extensions.Hosting.IHost" HelperName="global::worker.NoBuildHelper" HasBuildExpression="false" />
```

```c#
internal static class DovetailMineHelpers // the default HelperName of a host named Mine
{
    public static ValueTask<IHost> Configure(HostApplicationBuilder builder, Func<HostApplicationBuilder, IHost> build, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
        => ValueTask.FromResult(build(builder));
}

internal static class CustomHelper // BuildExpression adds the string between the delegate and the context builder
{
    public static ValueTask<IHost> Configure(HostApplicationBuilder builder, Func<HostApplicationBuilder, IHost> build, string extra, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
        => ValueTask.FromResult(build(builder));
}

internal static class NoBuildHelper // HasBuildExpression="false" drops the delegate
{
    public static ValueTask<IHost> Configure(HostApplicationBuilder builder, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
        => ValueTask.FromResult(builder.Build());
}
```

`Mine` generates `GeneratedDovetailMineExtensions` in `$(RootNamespace)`, and the other two use a namespace each, so their
`ConfigureDovetail` methods do not collide.

## Common mistakes

- **A host with no `HelperName` needs a class named `Dovetail{Name}Helpers`.** A host named `Orphan` calls `DovetailOrphanHelpers.Configure`,
  and the build fails with CS0103 in the generated file unless you wrote that class or named another with `HelperName`. The default
  fits the hosts `Joinery.Dovetail.Hosting` emits helpers for (`HostApplication`, `Maui`, `Uno`, `WebAssembly`) and Aspire's
  `DistributedApplication`; for any other name, set `HelperName`.
- **Two hosts on the same builder type in the same namespace make `ConfigureDovetail` ambiguous** (CS0121). An application built with
  the SDK already has `HostApplication` in `$(RootNamespace)`, so a second host on `HostApplicationBuilder` needs a `Namespace` of its
  own, and the caller imports only the one it wants.
- **`HostBuilderType` and `HostType` have no default.** Leave one out and the generated file is not valid C# (CS1001).
- **`BuildExpression` is spliced in as text.** It goes straight after `builder`, so it starts with a comma, and the `Configure` method it
  calls must take exactly those arguments. `HasBuildExpression="false"` removes it, including a `BuildExpression` you set.
- **`HostBuilderName` changes nothing.** The template does not use it today.
- **Types are written as given.** `HostBuilderType`, `HostType` and `HelperName` go into the generated code unchanged, so use
  `global::` as the SDK does; a bare name can be captured by a namespace of your project with the same first name.
- **`SyncEntryPoint` and `NavigateEntryPoint` are the SDK's.** `NavigateEntryPoint` emits calls to `{HelperName}.Navigate<TShell>`, which only
  `DovetailUnoHelpers` has; leave both alone on a host of your own.
- **A misspelled metadata name is ignored.** The default applies, with no warning.
- **When `Joinery.Dovetail.Hosting` is referenced, a host whose framework is missing is dropped.** If the helper a host names is one of
  the package's but the project does not reference the framework it compiles against (a `Microsoft.NET.Sdk.WebAssembly` app
  without Blazor, say), the host is removed from the build, with a message at normal verbosity, and `ConfigureDovetail` does not
  exist for it.

## See also

- [DovetailJoint](/reference/dovetail-joint/): the joint kinds that `ConfigureDovetail` runs.
- [DovetailTemplate](/reference/dovetail-template/): how `Joinery.Dovetail.Hosting` ships the helpers a host calls, and gates them on the host's framework.
- [Host Lifecycle](/concepts/host-lifecycle/): code that runs as the host starts and stops.
- [Defining Dovetails](/concepts/defining-joints/): the host type a joint can be filtered by.
