# DovetailJoint

# DovetailJoint

A `DovetailJoint` item declares a **joint kind**: a typed extension point that other code can contribute to without knowing who
applies it. One line in a project file becomes a joint interface, its async twin, the delegates for both, an `Apply` extension
that runs every joint of the kind, and a method on `DovetailContextBuilder` that registers a delegate. Dovetail's own joints
(`ISetupJoint`, `IServiceJoint`, `ILoggingJoint`, `IConfigurationJoint`, `IHostCreatedJoint<THost>`) are declared this way, and so are
those of [Serilog](../../concepts/serilog/), [OpenTelemetry](../../concepts/opentelemetry/) and Aspire.

The item is expanded by MSBuild targets, not by the Roslyn source generator: the C# is written to `obj/` before the compiler
runs, so you can open it in your IDE like any other source file.

## Who declares one, and when

A package or application that owns something other code should be able to configure declares a joint kind for it. Serilog's
package declares `Serilog` for `LoggerConfiguration`, so any library can add a sink; `Sample.Core` declares
`DatabaseConfigurator` for its own `IDatabaseConfigurator`, so other assemblies can add tables and views.

Declare one when:

- you hand an object to other code at one point in startup (a builder, a configuration object) and want every library to be able to
  contribute to it without referencing your application or each other;
- you are prepared to call `Apply{Name}` yourself at that point. A joint kind does not run by itself: `SerilogJoint` calls
  `loggerConfiguration.ApplySerilog(...)` once it has created the `LoggerConfiguration`.

Do not declare one when:

- you only want to run code at startup. Implement an existing joint such as `IServiceJoint` or `ILoggingJoint` instead, as described
  in [Defining Dovetails](../../concepts/defining-joints/);
- Dovetail or one of its packages already declares a joint for the type you have: `Joinery.Dovetail` covers `IServiceCollection`,
  `IConfigurationBuilder` and `ILoggingBuilder`, and `Joinery.Dovetail.Hosting` covers the host builders.

The item is read by the targets that ship in `Joinery.Dovetail`, so the project that declares it must reference that package
(`Joinery.Dovetail.Sdk` adds the reference for you). The generated types are `public`, so anything that references your project
sees them as ordinary API.

## Declaring a joint

The item's `Include` is the joint's name. It becomes part of every generated type name, and of the generated file names, so it has
to be a valid C# identifier, and it must be unique in the project.

```xml
<!-- src/Joinery.Dovetail/Joinery.Dovetail.csproj -->
<DovetailJoint Include="Setup" Namespace="Joinery.Dovetail.Joints" />
```

That is the smallest useful declaration: a joint that is handed only the `IDovetailContext`. Everything else is metadata.

## Metadata

Metadata is optional unless stated. A name the item does not read is ignored without a warning, so a misspelling falls back to the
default (`NMAmespace` leaves `Namespace` at its default). Boolean values are compared with `true` ignoring case; any other
non-empty value counts as false.

| Metadata               | Default                                 | What it changes                                                                                                                                                                                                                                                 |
| ---------------------- | --------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Namespace`            | `$(RootNamespace).Joints`               | The namespace of every generated type, and so the namespace consumers import. Joints of one package usually share one, as `Joinery.Dovetail.Joints` does.                                                                                                       |
| `ProvideConfiguration` | empty (false)                           | `true` adds an `IConfiguration configuration` parameter to `Register`, after the context. The value passed is `context.Configuration`; `Apply{Name}` takes no configuration parameter.                                                                          |
| `ProvideServices`      | empty (false)                           | `true` adds an `IServiceCollection services` parameter to `Register`, after the configuration if there is one. With no `ParameterType` it is also the subject of `Apply{Name}`.                                                                                 |
| `ParameterType`        | empty                                   | The type of one more `Register` parameter, the last one. It becomes the subject of `Apply{Name}`. Written into the generated code exactly as given; see [common mistakes](#common-mistakes).                                                                    |
| `ParameterName`        | `parameter` when `ParameterType` is set | The name of that parameter, as it appears in `Register` and in `Apply{Name}`. Ignored without `ParameterType`.                                                                                                                                                  |
| `BuilderMethodName`    | `Configure{Name}`                       | The name of the extension method on `DovetailContextBuilder` that registers a delegate. `Service` is declared with `BuilderMethodName="ConfigureServices"`.                                                                                                     |
| `GenericParameter`     | empty                                   | A type parameter name that makes the joint generic: `I{Name}Joint<in T>`, `Apply{Name}<T>`, `Configure{Name}<T>`. `ParameterType` may be this name, and then the parameter is typed `T`.                                                                        |
| `GenericConstraint`    | empty                                   | The constraint of that type parameter: `where T : {GenericConstraint}`. Written as given. Ignored without `GenericParameter`.                                                                                                                                   |
| `GenericVariance`      | `in` when `GenericParameter` is set     | The variance modifier on the type parameter of the generated interfaces and delegates. Ignored without `GenericParameter`. When `ParameterType` is the type parameter, only `in` compiles; see [common mistakes](#common-mistakes).                             |
| `IsAsync`              | `true`                                  | `false` generates only the synchronous joint. No `I{Name}AsyncJoint`, `{Name}AsyncJointDelegate` or async `Configure{Name}` overload, no `DovetailJointAsync.{Name}.g.cs`, and `Apply{Name}` returns `void` instead of `ValueTask`.                             |
| `ReturnFirstArgument`  | empty (false)                           | `true` makes `Apply{Name}` return `ValueTask<IDovetailContext>`, the context after the joints ran. It compiles only for a joint whose subject is the context itself: no `ParameterType` and no `ProvideServices`. With `IsAsync` false the result is discarded. |

### The parameter lists

`Register` takes, in this order, only the parameters that apply:

1. `IDovetailContext context`, always;
2. `IConfiguration configuration`, with `ProvideConfiguration`;
3. `IServiceCollection services`, with `ProvideServices`;
4. `{ParameterType} {ParameterName}`, with `ParameterType`;
5. `CancellationToken cancellationToken = default`, in the async twin only.

`Apply{Name}` is an extension method. Its subject (the `this` parameter) is the `ParameterType` if there is one, otherwise the service
collection if `ProvideServices` is set, otherwise the context. After the subject come the context (unless it is the subject), the service
collection (if `ProvideServices` and `ParameterType` are both set) and, last, a `CancellationToken cancellationToken = default`.

## What is generated

Each joint is written to `$(IntermediateOutputPath)`, which is `obj/<Configuration>/<TargetFramework>/`, as ordinary `Compile`
items:

| File                             | Contents                                                                                                        |
| -------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| `DovetailJoint.{Name}.g.cs`      | `I{Name}Joint`, its delegate, `Apply{Name}` and the `Configure{Name}` method for the delegate.                  |
| `DovetailJointAsync.{Name}.g.cs` | `I{Name}AsyncJoint`, its delegate and the `Configure{Name}` method for it. Not written when `IsAsync` is false. |

Everything is declared in `Namespace`:

| Type                          | What it is                                                                                                                                                                                                                                                           |
| ----------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `I{Name}Joint`                | The interface a contributor implements: `void Register(...)`. Mark the class `[DovetailExport]` so it is discovered; see [Defining Dovetails](../../concepts/defining-joints/).                                                                                      |
| `{Name}JointDelegate`         | A delegate with the same parameters, for registering a lambda.                                                                                                                                                                                                       |
| `I{Name}AsyncJoint`           | The async twin: `ValueTask Register(..., CancellationToken cancellationToken = default)`.                                                                                                                                                                            |
| `{Name}AsyncJointDelegate`    | The async delegate.                                                                                                                                                                                                                                                  |
| `{Name}JointExtensions`       | A static partial class holding `Apply{Name}`, which runs every registered `I{Name}Joint` and `I{Name}AsyncJoint` through the executor.                                                                                                                               |
| `{Name}HostBuilderExtensions` | A static partial class with an `extension(DovetailContextBuilder)` block: `Configure{Name}(delegate, priority = 0, category = null, hostType = Undefined)`, once for each delegate type. Generated for every joint, including the ones whose subject is the context. |

The interfaces derive from `IDovetailJoint` and, like the delegates, carry `[JointName("{Name}")]`.

For the Serilog declaration [below](#a-joint-with-a-parameter-services-and-its-own-namespace), the build writes this (abridged from `obj/Debug/net10.0/DovetailJoint.Serilog.g.cs`, which also holds the attributes, the XML comments and the method bodies):

```c#
namespace Joinery.Dovetail.Joints;

public interface ISerilogJoint : global::Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    void Register(global::Joinery.Dovetail.IDovetailContext context, global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::Serilog.LoggerConfiguration loggerConfiguration);
}

public delegate void SerilogJointDelegate(global::Joinery.Dovetail.IDovetailContext context, global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::Serilog.LoggerConfiguration loggerConfiguration);

public static partial class SerilogJointExtensions
{
    public static global::System.Threading.Tasks.ValueTask ApplySerilog(this global::Serilog.LoggerConfiguration loggerConfiguration, global::Joinery.Dovetail.IDovetailContext context, global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::System.Threading.CancellationToken cancellationToken = default);
}

public static partial class SerilogHostBuilderExtensions
{
    extension(DovetailContextBuilder container)
    {
        public global::Joinery.Dovetail.DovetailContextBuilder ConfigureSerilog(SerilogJointDelegate @delegate, int priority = 0, DovetailCategory? category = null, DovetailHostType hostType = DovetailHostType.Undefined, [CallerArgumentExpression(nameof(@delegate))] string? expression = null);
    }
}
```

## Examples

### Minimal

The `Setup` declaration [above](#declaring-a-joint) generates `ISetupJoint`, `SetupJointDelegate`, `ISetupAsyncJoint`,
`SetupAsyncJointDelegate`, `SetupJointExtensions.ApplySetup(this IDovetailContext context, CancellationToken cancellationToken = default)`
and `ConfigureSetup(...)`, all in `Joinery.Dovetail.Joints`.

### Leaving out `Namespace`

Left out, `Namespace` is `$(RootNamespace).Joints`. This project sets `RootNamespace` to `Joinery.Dovetail`, so the joint's types are
in `Joinery.Dovetail.Joints`:

```xml
<!-- src/Joinery.Dovetail.Aspire/Joinery.Dovetail.Aspire.csproj -->
<DovetailJoint Include="DistributedApplicationBuilder" ParameterType="global::Aspire.Hosting.IDistributedApplicationBuilder" ParameterName="builder" />
```

### A joint with a parameter, services and its own namespace

`Joinery.Dovetail.Serilog` hands every Serilog joint the context, the service collection and the logger configuration:

```xml
<!-- src/Joinery.Dovetail.Serilog/Joinery.Dovetail.Serilog.csproj -->
<DovetailJoint Include="Serilog" Namespace="Joinery.Dovetail.Joints" ProvideServices="true" ParameterType="global::Serilog.LoggerConfiguration" ParameterName="loggerConfiguration" />
```

The package applies the kind once it has built the `LoggerConfiguration` (`src/Joinery.Dovetail.Serilog/SerilogJoint.cs`):

```c#
var loggerConfiguration = new global::Serilog.LoggerConfiguration();
await loggerConfiguration.ApplySerilog(context, services, cancellationToken).ConfigureAwait(false);
```

and anyone contributes by implementing `ISerilogJoint` or calling `ConfigureSerilog`, as shown in [Serilog](../../concepts/serilog/#the-serilog-joint).

### A joint in an application

`Sample.Core` declares a joint kind for the object it hands to other assemblies, and applies it from a joint of its own:

```xml
<!-- sample/Sample.Core/Sample.Core.csproj -->
<DovetailJoint Include="DatabaseConfigurator" Namespace="Joinery.Dovetail.Joints" ParameterType="Sample.Core.Databases.IDatabaseConfigurator" ParameterName="builder" />
```

```c#
// sample/Sample.Core/Databases/DatabaseServiceDovetail.cs
[DovetailExport]
public class DatabaseServiceJoint : IServiceAsyncJoint
{
    public async ValueTask Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default)
    {
        var configurator = new DatabaseConfigurator();
        await configurator.ApplyDatabaseConfigurator(context, cancellationToken: cancellationToken);
    }
}
```

### A generic joint

`Joinery.Dovetail.Hosting` declares one joint for every host builder. `ParameterType` is the generic parameter, and, with no
`GenericVariance`, the interface is contravariant, so `IHostApplicationJoint<IHostApplicationBuilder>` runs for every host whose builder is one:

```xml
<!-- src/Joinery.Dovetail.Hosting/Joinery.Dovetail.Hosting.csproj -->
<DovetailJoint
    Include="HostApplication"
    Namespace="Joinery.Dovetail.Joints"
    GenericParameter="TBuilder"
    ParameterType="TBuilder"
    ParameterName="builder"
/>
```

`Joinery.Dovetail` uses the same shape for `HostCreated`, with `GenericParameter="THost"`.

### The rarely needed values

Nothing in `src/` or `sample/` sets `ProvideConfiguration`, `GenericConstraint`, `IsAsync` or `ReturnFirstArgument`, so these
declarations are compiled, and the signatures they generate checked, by
`test/Joinery.Dovetail.Sdk.Tests/DovetailItemReferenceTests.cs` (it declares exactly these items):

```xml
<DovetailJoint Include="Minimal" />
<DovetailJoint Include="Wired" ProvideConfiguration="true" ProvideServices="true" />
<DovetailJoint Include="Builder" Namespace="Custom.Joints" ProvideServices="true" ParameterType="global::Microsoft.Extensions.Logging.ILoggingBuilder" ParameterName="builder" BuilderMethodName="ConfigureTheBuilder" />
<DovetailJoint Include="Generic" GenericParameter="TBuilder" GenericConstraint="global::Microsoft.Extensions.Hosting.IHostApplicationBuilder" ParameterType="TBuilder" ParameterName="builder" />
<DovetailJoint Include="Covariant" GenericParameter="T" GenericVariance="out" />
<DovetailJoint Include="SyncOnly" IsAsync="false" />
<DovetailJoint Include="Chained" ReturnFirstArgument="true" />
<DovetailJoint Include="ChainedSync" ReturnFirstArgument="true" IsAsync="false" />
<DovetailJoint Include="Unprefixed" ParameterType="Microsoft.Extensions.Logging.ILoggingBuilder" ParameterName="builder" />
<DovetailJoint Include="Ignored" ParameterName="thing" GenericConstraint="class" GenericVariance="out" NMAmespace="Custom.Typo" />
<DovetailJoint Include="Shouting" IsAsync="False" ProvideServices="TRUE" />
<DovetailJoint Include="NotTrue" IsAsync="no" />
```

- `Minimal` is handed only the context, and its types are in `worker.Joints` (the test project's `RootNamespace` is `worker`).
- `Wired` registers `Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)`, and its
  `ApplyWired` is an extension on `IServiceCollection`.
- `Builder` is applied with `logging.ApplyBuilder(context, services)` and registered with `ConfigureTheBuilder(...)`.
- `Generic` declares `IGenericJoint<in TBuilder> where TBuilder : IHostApplicationBuilder`.
- `Covariant` has a type parameter that `Register` does not take, so `out` compiles, and an `ICovariantJoint<string>` is an
  `ICovariantJoint<object>`.
- `SyncOnly` has no async twin, and `context.ApplySyncOnly()` returns `void` but still takes a `CancellationToken`.
- `Chained.ApplyChained` returns the context. `ChainedSync` has no async twin and `ApplyChainedSync` returns `void`: the context the
  executor returns is discarded.
- `Unprefixed` is written into the generated code as `Microsoft.Extensions.Logging.ILoggingBuilder`, with nothing in front of it.
- `Ignored` generates the same code as `Minimal`: `ParameterName` without a `ParameterType`, `GenericConstraint` and `GenericVariance`
  without a `GenericParameter`, and the misspelled `NMAmespace` all do nothing, so its types are in `worker.Joints`.
- `Shouting` and `NotTrue` are synchronous: `False` and `no` are not `true`. `Shouting` also takes the service collection, because
  `TRUE` is `true` in any case.

## Common mistakes

- **`ParameterType` and `GenericConstraint` are written exactly as you give them.** Dovetail puts nothing in front of them, so
  write the type fully qualified with `global::`, as every package in this repository does:
  `global::Serilog.LoggerConfiguration`. A bare `Serilog.LoggerConfiguration` compiles too, until a namespace that encloses the
  generated code has a member called `Serilog`, which then captures the name. A project that declares `Custom.Microsoft` does that to a
  bare `Microsoft.Extensions.Logging.ILoggingBuilder` in a joint in `Custom.Joints`, and the build fails with CS0234.
- **A generic joint's `ParameterType` may be its `GenericParameter`**, and then must be written bare, as `TBuilder`: a `global::`
  in front of it names a type in the global namespace, not the type parameter.
- **`GenericVariance="out"` does not compile when the type parameter is the `ParameterType`** (CS1961): it is an input of `Register`,
  so a covariant one is invalid, and `in`, the default, is the only value that compiles. With a `GenericParameter` and no
  `ParameterType`, `Register` does not take the type parameter, and `out` compiles.
- **`ReturnFirstArgument` only suits a joint whose subject is the context.** With a `ParameterType` or `ProvideServices` the
  generated `Apply{Name}` tries to return the `IDovetailContext` as a `ValueTask<ILoggingBuilder>` (or whatever the subject is),
  which fails with CS0029.
- **`ParameterName` without a `ParameterType`, and `GenericConstraint` or `GenericVariance` without a `GenericParameter`, do
  nothing.** The metadata is ignored, with no warning.
- **A misspelled metadata name is ignored too.** `src/Joinery.Dovetail.Aspire.Testing` writes `NMAmespace`, so its namespace is the
  default, which happens to be the one it meant.
- **`IsAsync="false"` leaves contributors no way to do async work for the kind**, since there is no `I{Name}AsyncJoint` to implement.
  `Apply{Name}` still takes a `CancellationToken`, which the synchronous call ignores.
- **The name is part of every generated file and type name**, so declare each joint once.

## See also

- [Defining Dovetails](../../concepts/defining-joints/): implementing, ordering and filtering the joints of a kind.
- [DovetailHost](../dovetail-host/): the entry points that run the joints.
- [Serilog](../../concepts/serilog/) and [OpenTelemetry](../../concepts/opentelemetry/): two packages built on joint kinds.