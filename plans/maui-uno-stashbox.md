# Plan: MAUI hosting, Uno hosting, and Stashbox support

Status: design agreed (2026-10-22). Nothing is implemented yet.

## Summary

Three new packages, plus shared pieces, delivered in **one PR**:

| Package                 | Kind         | Modeled on                                                                  |
| ----------------------- | ------------ | --------------------------------------------------------------------------- |
| `Dovetail.Stashbox`     | DI container | `Dovetail.DryIoc`                                                           |
| `Dovetail.Hosting.Maui` | Host         | `Dovetail.Hosting` (`MauiAppBuilder` is an `IHostApplicationBuilder`)       |
| `Dovetail.Hosting.Uno`  | Host         | New `IHostBuilder` bridge (Uno.Extensions only exposes the old generic host) |

### Commit order inside the PR

1. Stashbox, including the DryIoc/Autofac SDK-detection fix
2. Shared pieces: embedded-resource config loaders, SDK embedding, sync entry-point template support,
   `InternalsVisibleTo` for the hosting helpers
3. MAUI
4. Uno
5. Workload CI job + samples
6. Docs

---

## How a package plugs in today (what we reuse)

- **Joints**: a `<DovetailJoint Include="X" ParameterType="..." />` item in the package csproj
  generates `IXJoint` / `IXAsyncJoint`, the `ApplyX(context, ct)` extension, and the
  `ConfigureX(...)` context-builder extension (`src/Dovetail/build/Dovetail.Joints.targets`).
- **Host entry points**: a `<DovetailHost>` item expands `src/Dovetail/build/DovetailHost.cs.template`
  into the `ConfigureDovetail(this TBuilder ...)` overloads, which call
  `{HelperName}.Configure(builder, b => b.Build(), contextBuilder, ct)`. The parameterless
  `DovetailContextBuilder.Create()` is generated **inside the app** (it wires that app's imports),
  so any entry point that calls it must be generated in the app too.
- **Helpers**: `DovetailHostApplicationHelpers.Configure<T, THost>` (`src/Dovetail.Hosting/RocketHostApplicationExtensions.cs`)
  and `DovetailWebAssemblyHelpers.Configure` (`src/Dovetail.Hosting.WebAssembly/RocketWebAssemblyExtensions.cs`):
  populate context builder → `DovetailContext.FromAsync` → shared configuration → `ApplyService` →
  `ServiceProviderFactoryAdapter` → host-builder joint → `Build()` → `ApplyHostCreated`.
- **SDK detection**: `src/Dovetail.Sdk/Sdk/Dovetail.HostDetection.targets` (it runs via
  `CustomAfterMicrosoftCommonTargets`) picks one `<When>` per project, then adds the `DovetailHost`
  item and an implicit package reference. The MAUI and Uno stubs there are wrong and get replaced (see below).
  Version properties (`DovetailHostingMauiVersion`, `DovetailHostingUnoVersion`, `DovetailStashboxVersion`)
  come for free: `Dovetail.Sdk.csproj` generates one per `src/` folder.
- **DI containers**: `Dovetail.DryIoc` = a joint (`IDryIocJoint`) + `ConfigureDryIoc : ISetupJoint`
  calling `context.UseServiceProviderFactory<IContainer>(...)` + an options type on the context.

---

## 1. `Dovetail.Stashbox`

### Facts

- Packages: `Stashbox` 5.21.0 and `Stashbox.Extensions.DependencyInjection` 6.0.0 (ships a `net10.0`
  asset). `Stashbox.Extensions.Hosting` 6.0.0 adds `IHostBuilder` / `IHostApplicationBuilder`
  `UseStashbox`, and it turns on lifetime validation automatically in Development.
- `StashboxServiceProviderFactory : IServiceProviderFactory<IStashboxContainer>` has a constructor
  taking `IStashboxContainer?`. `CreateBuilder(services)` fills **that** container, so a container
  created ahead of time is honored.
- `IServiceCollection.UseStashbox(IStashboxContainer)` returns an `IServiceProvider`.
  `AddStashbox(...)` replaces the factory registration.
- AOT/trimming: Stashbox compiles expression trees at runtime and sets neither `IsAotCompatible`
  nor `IsTrimmable`. `Dovetail.DryIoc` and `Dovetail.Autofac` set no AOT/trim properties either.

### Package `src/Dovetail.Stashbox/`

- `Dovetail.Stashbox.csproj`: `net10.0;net11.0`, `RuntimeAsyncBrowserWasmAsset`, `ProjectReference`
  to `Dovetail`, `PackageReference Include="Stashbox.Extensions.DependencyInjection"`. Like DryIoc,
  it sets **no** AOT/trim properties.
  - `<DovetailJoint Include="Stashbox" Namespace="Dovetail.Joints" ParameterType="global::Stashbox.IStashboxContainer" ParameterName="container" />`
- `StashboxJoint.cs`: `[DovetailExport, ConnectsJoint<IStashboxJoint>]`, same shape as `DryIocJoint`.
- `ConfigureStashbox.cs`: `[DovetailExport] internal class ConfigureStashbox : ISetupJoint`
  - `context.UseServiceProviderFactory<IStashboxContainer>(async (ctx, services, ct) => …)`
  - `ctx.GetOrAdd<IStashboxContainer>(() => new StashboxContainer(configure))`. `configure` applies
    `StashboxOptions.ConfigureContainer`, plus `WithLifetimeValidation()` when the environment is
    Development and `ValidateLifetimesInDevelopment` is true.
  - `await container.ApplyStashbox(ctx, ct)`, then return a `file`-scoped factory that delegates to
    `new StashboxServiceProviderFactory(container)`. After the provider is created, it calls
    `container.Validate()` when `ValidateOnBuild` is set.
- `StashboxOptions.cs` + `StashboxOptionsExtensions.cs` (C# 14 `extension(IDovetailContext)`, like DryIoc):
  - `Action<ContainerConfigurator>? ConfigureContainer`: registration behavior, disposable-transient
    tracking, etc. go through here. There is no dedicated "no more registrations" flag.
  - `bool ValidateOnBuild`: full `Validate()`, default `false`.
  - `bool ValidateLifetimesInDevelopment`: default `true`, matching `Stashbox.Extensions.Hosting`.
- `PublicAPI.Shipped/Unshipped.txt` entries (RS0017).

### SDK

- `Dovetail.HostDetection.targets`: add `DovetailEnableStashbox` and
  `<DovetailSdkImplicitPackageReference Include="Dovetail.Stashbox" …>`, conditioned on a
  `PackageReference` to `Stashbox`, `Stashbox.Extensions.DependencyInjection` **or** `Stashbox.Extensions.Hosting`.
- **Fix the existing gap in the same commit**: DryIoc also matches `DryIoc.Microsoft.DependencyInjection`,
  and Autofac also matches `Autofac.Extensions.DependencyInjection`.
- `Directory.Packages.props`: `Stashbox.Extensions.DependencyInjection` 6.0.0.

### Tests

- `test/Dovetail.Stashbox.Tests/`: port the DryIoc suite one-to-one (`DryIocBuilderTests`,
  `DryIocWebApplicationTests`, `DryIocCommandLineTests`, fixtures). Add tests for `ConfigureContainer`,
  `ValidateOnBuild`, and lifetime validation on in Development / off in Production / opted out.
- `test/Dovetail.Sdk.Tests/`: detection for each Stashbox package id, plus the new DryIoc/Autofac
  adapter-package ids (+ verified snapshots).

---

## 2. Shared pieces

### Embedded-resource configuration

On a device there are no `appsettings.*` files on disk (`HostConfigurationPropagationTests` copies
them to the output, which only helps desktop).

- `Dovetail.Configuration.{Json,Yaml,Toml}`: add an `*EmbeddedJoint : IConfigurationJoint` to each
  existing package (no new package). It only activates when the context has an
  `"EmbeddedConfigurationAssembly"` key (set by the MAUI/Uno helpers to the app assembly), and it
  loads the same sequence as `JsonBrowserJoint`: `appsettings`, `{app}`, `.{env}`, `.local`.
- SDK: **every** Dovetail project embeds its appsettings files as `EmbeddedResource`, named
  `{Project}.{ext}` like the copied files (`appsettings.local.*` only with `DovetailEmbedConfiguration=true`,
  so packed libraries never carry it). This goes beyond embedding only for MAUI/Uno apps,
  because each project's *generated* config joint
  (`Dovetail{Json,Yaml,Yml,Toml}Configuration.cs.template`) requires its `{Project}.{ext}` file:
  a library with config used in a MAUI app would otherwise throw on device. In embedded mode
  that generated joint reads its own assembly's embedded copy.
- `DovetailEmbedConfiguration=true` (set later by the MAUI/Uno detection branches) also stops copying
  the files to the output, so the embedded copies are the only source. The disk joints stand down
  in embedded mode, so desktop heads (Windows/Catalyst/Skia) don't load anything twice.
- Implemented in `ed2b8ba` (`DovetailEmbeddedConfiguration` in core: `AssemblyKey`,
  `OpenResource` with exact or `.{fileName}` suffix matching, `GetConventionalFileNames`).
- **Uno**: when Uno.Sdk already embeds a file (its Configuration feature does this for
  `EmbeddedSource<App>()`), don't embed it again. Record its existing `LogicalName` so the embedded
  joint reads it, and only embed the files Uno doesn't (yaml/toml, `{app}.{env}.*`).

### Sync entry point (MAUI only)

- Core `DovetailHost.cs.template` gains a section of synchronous `UseDovetail(this {HostBuilderType} …)`
  overloads (one per `ConfigureDovetail` overload, returning `{HostType}`). They block on the async
  path with `.AsTask().GetAwaiter().GetResult()`. A new name is needed because overloads can't
  differ only by return type.
- The section is gated by **internal** `DovetailHost` metadata (`SyncEntryPoint="true"`) that only the
  MAUI detection branch sets. It is not advertised as a general opt-in.
- Safe because every Dovetail await uses `ConfigureAwait(false)`. Docs state that MAUI joints must not
  post back to the UI thread during startup.

### Hosting helper access

- `DovetailHostApplicationHelpers.SharedHostConfigurationAsync` stays `internal`.
  `Dovetail.Hosting` gains `InternalsVisibleTo` for `Dovetail.Hosting.Maui` and `Dovetail.Hosting.Uno`.
  Trade-off accepted: these packages must be released in lockstep (they already share a version).

---

## 3. `Dovetail.Hosting.Maui`

### Facts

- `MauiAppBuilder` **implements `IHostApplicationBuilder`** (explicitly for `Environment` and
  `Configuration`), and it has `ConfigureContainer(IServiceProviderFactory<T>)`.
- `MauiApp` is **not** an `IHost` (only `IDisposable`/`IAsyncDisposable`).
- `MauiProgram.CreateMauiApp()` is synchronous.
- MAUI's `IHostApplicationBuilder.Environment` is a fixed default.

### Package `src/Dovetail.Hosting.Maui/`

- csproj: `net10.0;net11.0` (plain TFMs), `PackageReference Include="Microsoft.Maui.Core"`,
  `ProjectReference` to `Dovetail.Hosting` and `Dovetail.OpenTelemetry`.
  - `<DovetailJoint Include="MauiAppBuilder" ParameterType="global::Microsoft.Maui.Hosting.MauiAppBuilder" ParameterName="builder" />`
  - `<DovetailJoint Include="MauiApp" ParameterType="global::Microsoft.Maui.Hosting.MauiApp" ParameterName="app" />`
- `DovetailMauiHelpers.Configure(MauiAppBuilder, Func<MauiAppBuilder, MauiApp>, DovetailContextBuilder, ct)`:
  same steps as `DovetailHostApplicationHelpers.Configure`. It runs `ApplyHostApplication` (generic
  joints) **and** `ApplyMauiAppBuilder`, then after `Build()` runs `ApplyMauiApp` + `ApplyHostCreated`.
  - Seeds `EmbeddedConfigurationAssembly`, plus `DeviceInfo.Platform` / idiom under well-known keys.
  - Uses the internal `SharedHostConfigurationAsync` via `InternalsVisibleTo`. It does **not**
    relax `THost : IHost` on the existing helper.
- **EnvironmentName**, in this order:
  1. `DOTNET_ENVIRONMENT` if set
  2. an assembly attribute the SDK stamps at build time: Debug → `Development`, Release → `Production`
  3. the `DovetailEnvironmentName` MSBuild property, when set, overrides the stamped value

### As implemented (`31b71df`)

- The helper takes the **application assembly** explicitly (`Configure(builder, buildHost, applicationAssembly, contextBuilder, ct)`),
  passed by the generated extensions through `BuildExpression=", static b => b.Build(), typeof(GeneratedDovetailMauiExtensions).Assembly"`,
  because `Assembly.GetEntryAssembly()` isn't reliable on Android.
- MAUI's `MauiHostEnvironment` setters throw `NotImplementedException`, so the helper **replaces** the `IHostEnvironment`
  service (and the context entry) with its own carrying Dovetail's environment and application name.
- `Platform` comes from `OperatingSystem.Is*()` (no Essentials/`DeviceInfo` dependency, which throws on plain TFMs); no idiom key.
- The metadata is `SyncEntryPoint` (no underscore), because MSBuild rejects item attributes that start with `_`.
- SDK detection tests **evaluate** the MAUI projects (`dotnet msbuild -getProperty/-getItem`) rather than build them, since
  the MAUI workload isn't installed in the SDK test job.

### SDK

- Replace the MAUI stub with a `<When>` on `'$(UseMaui)' == 'true' and '$(OutputType)' != 'Library'`,
  placed **before** the `OutputType == Exe` branch (MAUI apps are `Exe`). It uses
  `DovetailSdkImplicitPackageReference`, `HelperName="global::Dovetail.DovetailMauiHelpers"`,
  `HostType="global::Microsoft.Maui.Hosting.MauiApp"`, and `SyncEntryPoint="true"`.

### Tests

- `test/Dovetail.Hosting.Maui.Tests/` on plain `net10.0;net11.0` with `MauiApp.CreateBuilder(useDefaults: false)`.
  Covers:
  - services, config and logging flow through
  - `IHostApplicationJoint` and `IMauiAppBuilderJoint` both fire, and `IMauiAppJoint` sees the built app
  - the container factory is honored (DryIoc, Autofac, Stashbox)
  - environment-name precedence
  - embedded config loading
  - `UseDovetail` returns a built app
- `test/Dovetail.Sdk.Tests/HostTypeTests`: `Maui_DetectsHostAndInjectsHostingMaui`,
  `MauiLibrary_NoHostDetected`, sync overloads emitted only for MAUI, appsettings embedded rather than copied.

---

## 4. `Dovetail.Hosting.Uno`

### Facts

- Detection: Uno.Sdk's `Sdk.props` sets `UsingUnoSdk=true`. `UnoFeatures` is a **property only**.
  `Uno.Features.targets` normalizes it to `;lowercase;features;` (whitespace stripped, `skia` added),
  but our detection may evaluate before that. So **normalize it ourselves** (strip whitespace,
  lowercase, wrap in `;`), then test `Contains(';hosting;')`.
- The `net10.0-browserwasm` head is built on `Microsoft.NET.Sdk.WebAssembly`, so
  `UsingMicrosoftNETSdkWebAssembly=true`. Nothing in Uno.Sdk sets the Blazor one.
- `Uno.Extensions.Hosting.IApplicationBuilder` ships in **`Uno.Extensions.Hosting.WinUI`**.
  `CreateBuilder(this Application, LaunchActivatedEventArgs[, Assembly])` is in the same package. Members:
  - `Application App`
  - `LaunchActivatedEventArgs Arguments`
  - `Window Window`
  - `IDictionary<object,object> Properties`
  - `Configure(Action<IHostBuilder>)` and `Configure(Action<IHostBuilder, Window>)`
  - `IHost Build()`, marked `[RequiresDynamicCode]` and `[RequiresUnreferencedCode]`
- `NavigateAsync<TShell>(this IApplicationBuilder, Func<IServiceProvider, INavigator, Task>? = null)`
  returns `Task<IHost>` (`TShell : UIElement, new()`), in **`Uno.Extensions.Navigation.WinUI`**.
- `Build()` goes through the old `IHostBuilder` (`UnoHost.CreateDefaultBuilder`). Its
  `ConfigureServices` and `ConfigureAppConfiguration` callbacks are synchronous and run inside `Build()`.
- Versions:
  - Uno.Extensions stable is **7.3.6**, with a plain `net9.0` asset plus net9 platform TFMs. That is
    what Uno.Sdk 6.7.30 (latest stable) pins.
  - **8.0.0-dev.\*** has `net10.0`, `net10.0-android36.0`, `net10.0-ios26.0` and
    `net10.0-windows10.0.19041`, but no browserwasm/desktop yet, and it needs Uno.WinUI 7 prereleases.
  - Uno.WinUI and Uno.Foundation.\* are stable at 6.7.135, with net9.0 and net10.0 assets.

### Package `src/Dovetail.Hosting.Uno/`

- csproj: `net10.0;net11.0`, `ProjectReference` to `Dovetail.Hosting`. References
  `Uno.Extensions.Hosting.WinUI` and `Uno.Extensions.Navigation.WinUI`, **multi-versioned per TFM**:
  - `net10.0` → **7.3.6** (stable; its net9.0 asset is consumed from net10.0)
  - `net11.0` → **8.0.0-dev.\*** (preview until Uno.Extensions 8.0 is stable)
  - The pins live in their **own conditional `ItemGroup`** in `Directory.Packages.props` (keyed to
    the Uno packages, separate from the generic net11 group), so Renovate can bump the 7.x and 8.x lines independently.
  - API differences go behind `#if NET11_0_OR_GREATER`. The docs mark the net11.0 build as preview.
- Joints:
  - `<DovetailJoint Include="UnoApplicationBuilder" ParameterType="global::Uno.Extensions.Hosting.IApplicationBuilder" ParameterName="builder" />`
  - `<DovetailJoint Include="UnoHostBuilder" ParameterType="global::Microsoft.Extensions.Hosting.IHostBuilder" ParameterName="builder" />`
- **`IHostBuilder` bridge, internal to this package** (`Dovetail.Hosting` does not advertise
  `IHostBuilder` support). All async work runs **before** `Build()`:
  1. `await DovetailContext.FromAsync(...)`.
  2. `await cb.ApplyConfiguration(...)` into a standalone `ConfigurationBuilder`. From
     `ConfigureAppConfiguration`, add it as a `ChainedConfigurationSource` **layered on top** of Uno's
     own sources: Uno's `EmbeddedSource<App>()` entries are kept, and Dovetail's chained source is
     added after them so it wins on conflicts. Files read through Uno's existing resource names
     (see Shared pieces) may be parsed by both. That's harmless because the values are the same.
  3. `await stagingServices.ApplyService(...)` into a staging `ServiceCollection`, then copy the
     descriptors into the real collection in `ConfigureServices`.
  4. Container factory: resolve `ServiceProviderFactoryAdapter` up front against a **forwarding
     `IServiceCollection`**, retargeted to the real collection inside `ConfigureServices`, then
     `UseServiceProviderFactory(...)`. No sync-over-async.
  5. `ApplyHostCreated` after `Build()` / `NavigateAsync`.
  - Documented limitation: service joints run against the staging collection, so they can't see
    Uno's built-in registrations (e.g. `TryAdd` over a framework default).
- `DovetailUnoHelpers`:
  - `Configure(IApplicationBuilder, Func<IApplicationBuilder, IHost>, …)` wraps `Build()`.
  - `ConfigureNavigate<TShell>(IApplicationBuilder, …)` wraps `NavigateAsync<TShell>()`.
  - Both seed the context with `IApplicationBuilder`, `App`, `Window`, `Arguments`, and
    `EmbeddedConfigurationAssembly`, then run `ApplyUnoApplicationBuilder` and
    `builder.Configure(host => bridge(host))`.
- **EnvironmentName: defer to Uno.** Read the environment Uno.Extensions computes and pass it into
  the Dovetail context unchanged. Dovetail does not call `UseEnvironment`.
- Generated entry points: `ConfigureDovetail(...)` (wraps `Build()`) and `ConfigureDovetail<TShell>(...)`
  (wraps `NavigateAsync<TShell>()`), with the full set of context-builder overloads. Both are async;
  `OnLaunched` can be `async void`.

### SDK

- Replace the Uno stub (it targeted `IUnoPlatformHostBuilder`, which is only the platform/window host):
  - `<When>` on `'$(UsingUnoSdk)' == 'true'` **and** the normalized `UnoFeatures` containing
    `;hosting;`, placed **before** both the WebAssembly branch and the `Exe` branch.
  - `HostBuilderType="global::Uno.Extensions.Hosting.IApplicationBuilder"`,
    `HostType="global::Microsoft.Extensions.Hosting.IHost"`, `HelperName="global::Dovetail.DovetailUnoHelpers"`.
  - The template also needs to emit the `<TShell>` overloads for this host (internal metadata,
    the same mechanism as `SyncEntryPoint`).
- Uno.Sdk detected **without** `Hosting`: emit a build warning ("Dovetail needs UnoFeatures=Hosting")
  and inject no host. Plain Uno apps without Extensions are out of scope.

### Tests

- `test/Dovetail.Hosting.Uno.Tests/` on `net10.0;net11.0`: bridge tests against plain
  `Host.CreateDefaultBuilder()`. Covers:
  - config layering order
  - staged services land after the host defaults
  - the forwarding collection with each container (DryIoc, Autofac, Stashbox)
  - `HostCreated` fires
  - environment passthrough
  Plus an `IApplicationBuilder` fake for joint ordering and context keys. Anything that touches the
  real WinUI runtime is covered by the samples, not unit tests.
- `test/Dovetail.Sdk.Tests/HostTypeTests`:
  - Uno + Hosting
  - Uno without Hosting (warning, no host)
  - `UnoFeatures` with whitespace/newlines/mixed case
  - Uno browserwasm head detected as Uno, not WebAssembly
  - an Uno-embedded appsettings file isn't embedded twice

---

### As implemented (`c63e8b7`)

- **Bridge revised (user decision during implementation):** Uno settles its environment (including the app's own
  `UseEnvironment`) only inside `Build()`, so staging config/services up front couldn't know which `appsettings.{env}`
  to load. The context is created up front; configuration and services are applied from inside
  `ConfigureAppConfiguration`/`ConfigureServices`, waiting on each joint's `ValueTask` only if it isn't already complete.
  This removes the staging limitation: service joints see Uno's own registrations. The container factory is a
  deferred `IServiceProviderFactory<object>` that creates the Dovetail factory from the final collection in `CreateBuilder`.
- The application assembly is passed explicitly (same `BuildExpression` approach as MAUI). The tests use Uno's own
  `Uno.Extensions.Hosting.HostBuilder` behind a fake `IApplicationBuilder` (the real one needs a running app).
- `ConfigureDovetail<TShell>` comes from `DovetailHostNavigate.cs.template`, behind the internal `NavigateEntryPoint` metadata.
- An Uno app **without** Hosting gets no host (and the warning) instead of falling through to the generic `Exe` branch.
- Dedupe: `_DovetailRemoveDuplicateConfigurationResources` (in `Dovetail.PackConfiguration.targets`) drops Dovetail's
  embedded copy of any `appsettings*` file another tool already embeds (`MatchOnMetadata="FullPath"`), since Uno.Sdk's
  `EmbeddedResource` items are evaluated after Dovetail's; the joints find Uno's `{RootNamespace}.appsettings*.json` by suffix.
- Not covered by tests: the warning text (the SDK harness doesn't capture build output), `NavigateAsync<TShell>` at
  runtime (needs a real Uno app; the generated overloads are compile-checked), and real Uno.Sdk projects (simulated
  via the properties Uno.Sdk sets; the samples/CI commit covers real ones).

## 5. Samples and CI

- `sample/Sample.Maui`: one project multi-targeting `net10.0-*` and `net11.0-*` heads (android, ios,
  maccatalyst, windows). The .NET 11 SDK's maui workload builds both.
- `sample/Sample.Uno`: `Sdk="Uno.Sdk/6.7.30"` inline, net10 heads (desktop, browserwasm, android, ios, windows).
- `sample/Sample.Uno.Net11`: `Sdk="Uno.Sdk/7.0.0-dev.*"` inline, net11 android/ios/windows heads.
  It shares XAML/code with `Sample.Uno` through a linked folder.
- SDK versions are pinned inline in each project, not in `global.json` `msbuild-sdks`, so the two
  Uno samples can differ and Renovate tracks both.
- All three are listed in `Dovetail.slnx`, but they are left out of the default build (they need workloads).
- **New workload CI job, full matrix, every PR, required check**:
  - **ubuntu**: MAUI android; Uno desktop/browserwasm/android (net10) and android (net11)
  - **macos**: MAUI ios/maccatalyst; Uno ios (net10 and net11)
  - **windows**: MAUI windows; Uno windows (net10 and net11)
  - Installs workloads per leg. Also confirms that net10 Uno heads resolve Uno.Extensions 7.3.6's
    net9.0 platform assets (unverified NuGet nearest-TFM behavior for the `browserwasm`/`desktop` platform TFMs).
  - Expect noticeable PR latency and runner cost from the macOS/Windows legs.

---

### As implemented (`7a9c6c8`)

- Generated from the official templates (`Microsoft.Maui.Templates.net10` 10.0.120, `Uno.Templates` 6.7.30, installed into a
  scratch template hive), then wired to src like the other samples (ProjectReferences + the SDK's `DovetailHost` items).
- **Excluded from the solution build with `<Build Project="false" />` in `Dovetail.slnx`** instead of a
  `DovetailBuildWorkloadSamples` property: it also keeps them out of the solution restore (verified: a `UseMaui` project
  without the workload otherwise fails `dotnet build Dovetail.slnx` with NETSDK1147).
- `DovetailSampleTargetFrameworks` picks the heads per CI leg (`;` passed as `%3B`).
- Uno.Sdk's implicit `Microsoft.Extensions.Logging` packages need `MicrosoftLoggingVersion=10.0.13` under the repo's CPM.
- `Sample.Uno.Net11` links the app code; head files (Platforms/Android, iOS, Windows, assets) are its own copies.
- `hk.pkl` keeps `dotnet format` off the samples (per-platform formatting leaves conflict markers).
- Verified locally (no workloads installed): Sample.Uno desktop head builds and starts (20s, no exceptions), embedded
  resources are only Uno's `Sample.Uno.appsettings*.json` (dedupe works with the real Uno.Sdk); Sample.Uno.Net11
  compiles (checked via a temporary net11.0-desktop head). **Not built locally:** Sample.Maui (no maui workload),
  the Uno browserwasm/android/ios/windows heads. The workflow is their first build.
- **The "required check" is a repository setting** (branch protection/rulesets), not part of the workflow file.

## Cross-cutting checklist

- `PublicAPI.*.txt` updated (RS0017 is an error).
- Versions only in `Directory.Packages.props`.
- `Dovetail.slnx` updated (`/src/`, `/test/`, `/sample/`).
- `prettier --write . && dotnet format`.
- Docs:
  - a DI-containers page (Autofac, DryIoc, Stashbox; AOT caveat)
  - a MAUI host guide (`UseDovetail`, environment precedence, UI-thread caveat)
  - an Uno host guide (Hosting feature requirement, `<TShell>` entry point, the staged-services
    limitation, net11 preview status)
  - an embedded-configuration section
  - a `Readme.md` line for each
- AOT: Stashbox and DryIoc aren't AOT/trim-safe, and neither is Uno's `Build()`. Document this for
  MAUI iOS Native AOT users.

---

## Decision log

| #   | Decision                                                                                                |
| --- | ------------------------------------------------------------------------------------------------------- |
| Q1  | One combined PR, one commit per area                                                                    |
| Q2  | `StashboxOptions`: `ConfigureContainer`, `ValidateOnBuild` (false); lifetime validation auto-on in Development, with opt-out |
| Q3  | Fix DryIoc/Autofac adapter-package detection in the Stashbox commit                                     |
| Q4  | Sync `UseDovetail(...)` from the core template, gated by internal metadata set only by MAUI             |
| Q5  | `InternalsVisibleTo` for the MAUI/Uno packages; no new public API, no constraint relaxation             |
| Q6  | MAUI env: `DOTNET_ENVIRONMENT` → build config → `DovetailEnvironmentName` override. Uno: defer to Uno.Extensions |
| Q7  | Uno.Extensions.Hosting apps only; bridge internal to the Uno package; `Build()` + `NavigateAsync<TShell>` entry points |
| Q7c′ | Revised during implementation: config/service joints run **inside** Uno's build callbacks (not staged up front), because Uno only knows its environment there |
| Q7  | Uno.Extensions multi-versioned: 7.3.6 for net10.0, 8.0.0-dev for net11.0, in a separate props group     |
| Q8  | Embedded-resource joints in the existing config packages; MAUI/Uno embed rather than copy; Uno: reuse Uno's resources and layer on top of its sources |
| Q9  | Samples: one multi-target MAUI sample, two Uno samples (6.7.30 / 7.0 dev); workload CI full matrix on every PR, required |
