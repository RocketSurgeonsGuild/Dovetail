---
title: ModularPipelines
description: Apply Dovetail joints to a ModularPipelines PipelineBuilder, so joints add the modules, options and services of a build pipeline.
---

# ModularPipelines

`Joinery.Dovetail.ModularPipelines` makes a [ModularPipelines](https://github.com/thomhurst/ModularPipelines) pipeline a Dovetail
host. Joints add the pipeline's modules, options and services, and the pipeline project only builds and runs it. The package targets
`net11.0` only, as ModularPipelines 4 does.

## Getting the package

A build script names one SDK, `Joinery.Dovetail.Sdk.ModularPipelines`, and nothing else:

```csharp
#:sdk Joinery.Dovetail.Sdk.ModularPipelines

using Build;

await using var pipeline = await Pipeline.CreateBuilder(args).ConfigureDovetail();
await pipeline.RunAsync();
```

It chains `Joinery.Dovetail.Sdk` and adds `ModularPipelines` itself, at the version the SDK was built with
(`DovetailModularPipelinesVersion` picks another; a project that references `ModularPipelines` keeps its own reference). A project file
uses it the same way, `<Project Sdk="Joinery.Dovetail.Sdk.ModularPipelines">`, and needs no `OutputType`: the SDK makes it an `Exe`.
Pin its version in `global.json` under `msbuild-sdks`, as for the other Dovetail SDKs.

With `Joinery.Dovetail.Sdk` alone, a project that references `ModularPipelines` on `net11.0` or later gets the same: the SDK adds
`Joinery.Dovetail.ModularPipelines` and `ModularPipelines.OpenTelemetry`, and declares the ModularPipelines host for an application
(`DovetailEnableModularPipelines=false` opts out of all of it). Without either SDK, reference `Joinery.Dovetail.ModularPipelines`
yourself, next to `ModularPipelines`: its build targets declare the host.

`Joinery.Dovetail.ModularPipelines` holds the `PipelineBuilder` joint kind (`IPipelineBuilderJoint`, `IPipelineBuilderAsyncJoint`), which a
library of joints references. The code the host's entry points call, `DovetailPipelineHelpers`, is not in it: `Joinery.Dovetail.Hosting`,
which the package depends on, emits it into the application, as it does the helpers of every other host, once the application references
`ModularPipelines` and `Joinery.Dovetail.ModularPipelines`. It is internal to the application.

## OpenTelemetry

`ModularPipelines.OpenTelemetry` traces and measures a pipeline's modules and commands. The SDK adds it next to `ModularPipelines`, at
the same version (the package pins `ModularPipelines` to its own), and `Joinery.Dovetail.Hosting` emits a `PipelineBuilder` joint into the
application that calls its `AddOpenTelemetry()`. Exporting is up to the application, as for the rest of its OpenTelemetry: an
`IOpenTelemetryJoint`, or a `PipelineBuilder` joint that calls `AddOpenTelemetry(z => z.AddOtlpExporter())` itself.
`DovetailEnableOpenTelemetry=false` leaves the package and the joint out.

## Building the pipeline

The host is declared for `PipelineBuilder`, so an application (a project that imports joints, as the SDK makes every `Exe`) gets
`ConfigureDovetail` extension methods on it, generated into the project's root namespace like those of every other host:

```csharp
// sample/ModularPipelinesSample/Program.cs
using ModularPipelines;
using ModularPipelinesSample;

// ConfigureDovetail (generated into the project's root namespace) applies the joints, BuildJoint among them, then builds the pipeline.
await using var pipeline = await Pipeline.CreateBuilder(args).ConfigureDovetail();
await pipeline.RunAsync();
```

`ConfigureDovetail` returns the built `IPipeline`, ready to run. Before building it, it applies, in this order:

1. the configuration joints, whose sources go in front of the pipeline's environment variables and command line, so those still win.
   The pipeline's configuration is registered as the `IConfiguration` first, so the one a service joint may add with `TryAdd` (as
   OpenTelemetry's `AddOpenTelemetry()` does, which reads only the environment variables) does not take its place;
2. the service joints, and with them the logging joints, to `builder.Services`;
3. the `PipelineBuilder` joints, to the builder itself.

After building it, it hands the pipeline to the `HostCreated` joints (`IHostCreatedJoint<IPipeline>`). The overloads that take an
action configure the `DovetailContextBuilder` first, as for the other hosts:

```csharp
await using var pipeline = await Pipeline
                                .CreateBuilder(args)
                                .ConfigureDovetail(z => z.ConfigurePipelineBuilder((_, builder) => builder.AddModule<ReleaseModule>()));
```

Configure what has to come before the joints on the builder before calling `ConfigureDovetail`: once it returns, the builder is
consumed. That includes where configuration files are read from: the configuration joints resolve their files (`appsettings.json` and
the rest) against the file provider of the pipeline's configuration, so a pipeline whose build scripts live in their own folder points
it there first:

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Configuration.SetBasePath(Path.Combine(Environment.CurrentDirectory, "build"));
await using var pipeline = await builder.ConfigureDovetail();
```

## Writing a PipelineBuilder joint

The `PipelineBuilder` joint is handed the builder, so it can do anything the pipeline project would: add modules, register services, set
options, add plugins. A joint that adds the module of a sample pipeline and the service the module uses:

```csharp
// sample/ModularPipelinesSample/BuildJoint.cs
[DovetailExport]
public sealed class BuildJoint : IPipelineBuilderJoint
{
    public void Register(IDovetailContext context, PipelineBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
        builder.AddModule<GreetingModule>();
    }
}
```

`IPipelineBuilderAsyncJoint` is the asynchronous kind. A joint that only registers services can be an `IServiceJoint` instead, which
every host applies. Joints run in the order of their dependencies and priorities, so a library of modules can export its joints and
order them against the application's.

Add modules with `AddModule<TModule>()`: it registers the module by its type, which is what keeps the pipeline trimmable and Native AOT
compatible. Dovetail finds the joints at build time, so nothing is scanned at run time either.
