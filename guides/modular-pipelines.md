# ModularPipelines

# ModularPipelines

`Joinery.Dovetail.ModularPipelines` makes a [ModularPipelines](https://github.com/thomhurst/ModularPipelines) pipeline a Dovetail
host. Joints add the pipeline's modules, options and services, and the pipeline project only builds and runs it. The package targets
`net11.0` only, as ModularPipelines 4 does.

## Getting the package

With `Joinery.Dovetail.Sdk`, a project that references `ModularPipelines` on `net11.0` or later gets `Joinery.Dovetail.ModularPipelines`
added for it (`DovetailEnableModularPipelines=false` opts out). Without the SDK, reference the package yourself, next to `ModularPipelines`.

## Building the pipeline

The package declares a host for `PipelineBuilder`, so an application (a project that imports joints, as the SDK makes every `Exe`)
gets `ConfigureDovetail` extension methods on it, generated into the project's root namespace like those of every other host:

```csharp
// sample/ModularPipelinesSample/Program.cs
using ModularPipelines;
using ModularPipelinesSample;

// ConfigureDovetail (generated into the project's root namespace) applies the joints, BuildJoint among them, then builds the pipeline.
await using var pipeline = await Pipeline.CreateBuilder(args).ConfigureDovetail();
await pipeline.RunAsync();
```

`ConfigureDovetail` returns the built `IPipeline`, ready to run. Before building it, it applies, in this order:

1. the configuration joints, whose sources go in front of the pipeline's environment variables and command line, so those still win;
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
consumed.

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

## Without ConfigureDovetail

`DovetailPipelinePlugin` applies the same joints as a ModularPipelines plugin, for a project that builds the pipeline itself:

```csharp
var builder = Pipeline.CreateBuilder(args).AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()));
builder.AddModule<ReleaseModule>();
await builder.RunAsync();
```

The joints run when the plugin is added, so what the project adds afterwards comes after them. The `HostCreated` joints do not run, as
the plugin never sees the built pipeline. `DovetailContextBuilder.Create()` is generated alongside a host's entry points, so a project
that uses the plugin keeps the host (`DovetailModularPipelinesHost=false` leaves it out), even if it never calls `ConfigureDovetail`.