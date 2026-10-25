# Plan: Dovetail configuration → JSON Schema (code-first, merged per application)

Status: design agreed (2026-10-24). PR A is on `fix/dovetail-bugs-and-analyzer-flow`. P0 is done ([results, plan changes and follow-up decisions](configuration-json-schema-p0.md)), which supersede this file where they differ. P1 and [P3](configuration-json-schema-p3.md) are done; P2 is in progress.

## Context

Libraries built with Dovetail ship default configuration (`appsettings.{yaml,json,toml}` renamed to `{Project}.{ext}`, packed, and copied into hosts), but nothing describes the shape of that configuration.

Goals:
- each library describes its configuration as JSON Schema, generated from code;
- the library uses strongly-typed, AOT-safe configuration objects internally;
- each application gets one merged schema covering every referenced library, for editor completion and validation.

**Rejected alternatives:**
- **Corvus.JsonSchema:** schema-first; its types are JSON-document structs that can't bind from `IConfiguration`.
- **JsonSchema.Net.Generation:** builds schemas at runtime, and its packages carry a maintenance-fee EULA.
- **STJ `JsonSchemaExporter`:** reflects serializer semantics, not configuration-binder semantics.

**Prior art:** Aspire's `ConfigurationSchema.json` + `JsonSchemaSegment`. Its generation runs only in Aspire's own repo, and VS's merging is VS-only and undocumented.

**Verdict:** feasible, ~30–40 engineer-days across two PRs, after #209 merges. The highest-risk parts are VS segment and package discovery behaviour, and a compiled MSBuild task on both MSBuild runtimes. Spikes come first.

## Decisions (from the design grilling)

| Area | Decision |
|---|---|
| Source of truth | Code-first. Dovetail's Roslyn generator derives schema and binder from one type model; no third-party schema library. |
| Attribute forms | Both, in v1: class-level `[DovetailConfiguration]` (class-only, one per class), and the generic assembly-level `[assembly: DovetailConfiguration<T>("Section")]` (assembly-only, one per `T`, enforced by the compiler) for types you don't own. Both register `IOptions<T>`. |
| Section path | Optional. The default is the type name with an `Options`/`Settings`/`Configuration`/`Config` suffix stripped; no prefix. A name that would become empty is an error. |
| One section per type | A type has exactly one section, so there is no `Order` and no stacking, de-duplication or merging. Configuring a type with both a class attribute and an assembly `DovetailConfiguration<T>` is DT0015 (the class declaration wins). A library reads only its own section from the application's complete `IConfiguration`; the standard options pipeline does the rest. Across assemblies, the public `AddDovetailConfigurationOptions` overloads follow the pipeline's usual rules: each bind registration adds a configure step in registration order, and the first factory wins. |
| Type shapes | Mutable classes use the standard options pipeline (`IConfigureOptions`, so `Configure`/`PostConfigure` keep working). Immutable types (records, `init`, positional constructors) get an **`IOptionsFactory<T>`** (a small `OptionsFactory<T>` subclass) that creates the default instance from its section and still runs configure, post-configure and validators. |
| Binder semantics | Match Microsoft's configuration-binder **source generator** exactly, quirks included (list append, constructor-parameter rules). Pinned per TFM by parity tests. |
| Validation | Auto-register a user-declared `[OptionsValidator]` partial. `ValidateOnStart` is an opt-in attribute property. Never `ValidateDataAnnotations`. |
| Schema draft / strictness | Draft-07. Library fragments are permissive (no `required` unless `DovetailConfigurationSchemaRequired=true`). The **merged application schema is strict**: each Dovetail section and its nested objects are closed with a **case-insensitive `propertyNames`** rule (`additionalProperties:false` isn't used, so non-canonical casing still validates). Root, shared parent nodes, dictionaries and overlapping sections stay open. Opt out with `DovetailConfigurationSchemaStrict=false`. |
| Merged scope | Includes SchemaStore's appsettings schema by default (`allOf` + `$ref https://json.schemastore.org/appsettings.json`). Opt out via property. |
| Which projects merge | Applications (SDK-detected, plus non-SDK Exe) **and test projects**. Override with `DovetailConfigurationSchemaHost`. |
| Where the merge runs | A **compiled MSBuild task** (`src/Dovetail.Build.Tasks`, netstandard2.0, packed into `Dovetail/build/tasks/`). It reads fragments from `@(ReferencePath)` and its own `@(IntermediateAssembly)` metadata. Nothing is embedded in application DLLs. |
| Runtime | No runtime constant or DI service. Optional endpoint `GET /.well-known/dovetail/configuration-schema`: off by default, `DovetailConfigurationSchemaEndpoint=true` auto-maps it through a generated web-host joint (Dovetail.Hosting.Web) in every environment, optionally restricted by `DovetailConfigurationSchemaEndpointEnvironments`. It serves `bin/appsettings.schema.json`, falling back to `obj/` when running from source. |
| Files | Host: `$(BaseIntermediateOutputPath)appsettings.schema.json` (stable editor path), copied to output and **always published**. Library: `obj/.../ConfigurationSchema.json`, packed at the **nupkg root** (Aspire-compatible). |
| VS segments | **Host + per-package.** The host adds a segment for its merged file. Dovetail's targets **discover referenced package folders** that contain a root `ConfigurationSchema.json` and add segments, de-duplicated against segments the package already declares (e.g. Aspire's). No per-package targets files, so no collisions. |
| Editors | Opt-in scaffolding (`DovetailScaffoldEditorSettings=true`) creates or updates **only Dovetail's mappings** in `.vscode/settings.json` (`json.schemas`, `yaml.schemas`, `evenBetterToml.schema.associations`), preserving comments. If the file can't be updated safely: warning + snippet, file untouched. Never adds `$schema` keys or modelines. |
| Transitive consumers | **Flow the analyzer** to projects that reference Dovetail only transitively. Gate generation by honouring `ImportDovetailAssembly`/`ExportDovetailAssembly` (made compiler-visible). Non-SDK defaults: import for Exe/test projects, export everywhere. Below C# 12, the generator skips with a warning. |
| Serilog | Left alone (its section shape conflicts with Serilog's own format). Dogfood via SdkSample. |
| Docs | Rewrite `concepts/managed-configuration.md`, removing the old appsettings-inference design, and add an editor-setup page. |
| Sequencing | Wait for #209 to merge, then branch from upstream `main`. **PR A:** bug fixes + analyzer flow. **PR B:** spikes (go/no-go report first), then the schema feature. |

## PR A: `fix/dovetail-bugs-and-analyzer-flow` (~5–7 d)

1. **Bugs found during research:**
   - The generated `Imports` always passes `DovetailHostType.UnitTest` (`Support/ImportDovetails.cs`), so `TestingJoint` can activate in apps. Use the configured `DovetailHostType`, or leave it to the host helpers' `AddIfMissing(DovetailHostType.Live)`.
   - Under the Web SDK, `appsettings.json` is a `Content` item, so `Dovetail.PackConfiguration.targets` (which only updates `None` items) never renames it, yet the generated JSON joint requires `{Project}.json`. Handle `Content` items too, with a test using a Web SDK scaffold.
   - `ImportDovetailAssembly`/`ExportDovetailAssembly` are set but never read. Add them to `CompilerVisibleProperty` (`src/Dovetail/build/Dovetail.targets`), read them in `Support/MsBuildConfig.cs`, and gate `ExportDovetails`/`ImportDovetails` emission. Defaults: `Dovetail.targets` mirrors `Dovetail.HostDetection.targets` (import for Exe/test projects, export everywhere).
2. **Flow the analyzer:**
   - Change the integration projects' reference to Dovetail so analyzers aren't private. The exclusion comes from NuGet's default `PrivateAssets` on the plain `ProjectReference`s; fix it with an `ItemDefinitionGroup`/metadata in `src/Directory.Build.targets` (`PrivateAssets="contentfiles;build"`).
   - Verify every nuspec (`exclude="Build"` only).
   - LangVersion guard: emit nothing below C# 12 and report a new DT warning.
   - Accept that existing DT0001–0005 errors can now fire in transitive consumers; document it.
3. **Tests (`test/Dovetail.Sdk.Tests`):**
   - New scaffolds that **build** a Hosting-only consumer, both library and Exe, asserting what generates and what doesn't.
   - The Web SDK appsettings.json rename.
   - Host type is Live in apps.
   - A nuspec dependency assertion (`VerifyProjectsAndPackages`).
   - The LangVersion guard.
   - Plus analyzer snapshots for the gating.

## PR B: `feature/configuration-json-schema`

### P0: Spikes + go/no-go report (3–4 d)

- **S1, VS:**
  - pickup of the host segment, including a file created after the first build, and refresh behaviour;
  - discovered package segments in design-time builds;
  - duplicate segments.
- **S2, doc comments:** `GetDocumentationCommentXml` when `GenerateDocumentationFile=false`; the trivia-parse fallback.
- **S3, compiled task:**
  - load, run and cost on `dotnet build` and VS `MSBuild.exe` (net472), with `-m`/`-mt`;
  - System.Text.Json and System.Reflection.Metadata availability without shipping conflicting copies;
  - in-repo `TaskHostFactory` to avoid DLL locks;
  - a comment-preserving JSONC editor approach.
- **S4, editors:** VS Code JSON, yaml-language-server, Taplo and Rider with draft-07, `allOf`, case-insensitive `propertyNames`, and the remote SchemaStore `$ref`.
- **S5, binder capture:** source-gen vs reflection binder on net10/net11 (empty strings, nullable, constructor-parameter binding, complex-with-value, list append).

### P1: Model + fragments (6–8 d)

- `src/Dovetail/Attributes/DovetailConfigurationAttribute.cs`: section path optional; `RegisterOptions`, `ValidateOnStart`; class-only. `DovetailConfigurationAttribute<T>` is the assembly-only form (`AllowMultiple = false`; the same `T` twice is CS0579). `[PublicAPI]`, `[Conditional("CodeGeneration")]`.
- New pipeline in `src/Dovetail.Analyzers/DovetailAttributesGenerator.cs`: `ForAttributeWithMetadataName` for both forms → equatable model (no `ISymbol`/`Location`/`ImmutableArray` downstream).
- New files under `Support/Configuration/`: `ConfigurationModel`, `ModelBuilder`, `JsonSchemaWriter` (hand-written, escaping, invariant, deterministic), `EquatableArray`, `DiagnosticInfo`.
- Supported types:
  - string, bool, all numeric types, enum, TimeSpan, DateTime/DateTimeOffset/DateOnly/TimeOnly, Guid, Version, Uri, byte[];
  - `Nullable<T>`, nested POCOs and records;
  - arrays/lists/sets, and dictionaries with string or enum keys.
  - Anything else gets a warning and is skipped in both schema and binder.
- Annotations:
  - `[Description]`, falling back to XML `<summary>`;
  - `[Range]`, length attributes, anchored `[RegularExpression]`;
  - `[Url]`/`[EmailAddress]` → `format`; `[Obsolete]` → `deprecated`;
  - constant initializers → `default`.
- Fragments are emitted as `[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:<Section>", json)]`, with a structured list of property names per object so the task can apply strict `propertyNames` (the exact encoding is settled in S3).
- Diagnostics DT0006+, recorded in `AnalyzerReleases.Unshipped.md`:
  - invalid type;
  - invalid or empty default section;
  - unsupported member;
  - key collision;
  - ambiguous constructor;
  - recursive type cut;
  - inexpressible annotation;
  - LangVersion (from PR A).
- Delete dead `GetDovetailConfigurationReferences`. Make `GetAssemblyMetadataAttributes` tolerate duplicate keys (it uses `ToImmutableDictionary` today).

### P2: Compiled task + library/host files (6–8 d)

- `src/Dovetail.Build.Tasks` (netstandard2.0). New `src/Dovetail/build/Dovetail.ConfigurationSchema.targets`, imported from `Dovetail.targets`.
- Hook `AfterTargets="CoreCompile"` with a stamp file; skip design-time builds; delete stale outputs.
- **Library:** extract own fragments → `ConfigurationSchema.json` (segment shape). Pack it at the nupkg root from the first TFM only.
- **Host:**
  - merge own + `@(ReferencePath)` fragments into a case-insensitive section tree; `allOf` for overlaps and duplicates;
  - apply strict `propertyNames` closing and the SchemaStore base;
  - write `obj/appsettings.schema.json`, copy to output, include in publish.
- Exclude `appsettings.schema.json` from the `appsettings*.json` globs in `Dovetail.PackConfiguration.targets`.
- In-repo wiring: `src`, `test` and `sample` `Directory.Build.targets` reference the task project via `UsingTask` overrides.

### P3: Binder, factory, joint (8–11 d)

- `BinderEmitter`: MS source-gen semantics for mutable types.
- `FactoryEmitter`: `IOptionsFactory<T>` for immutable types, creating from one section, validators.
- `JointEmitter`: one `DovetailConfigurationOptionsJoint : IServiceJoint` per assembly, appended to `Exported_Dovetails`. Change `ExportDovetails.cs:16`'s early return; `ImportDovetails` self-includes it.
- One registration per type, emitted in path then type name order. `[OptionsValidator]` pickup via `TryAddEnumerable`; `ValidateOnStart`.
- New reflection-free `AddDovetailConfigurationOptions` overload(s) in `src/Dovetail/DovetailConfigurationOptionsExtensions.cs` (`[DynamicallyAccessedMembers]`, change-token source for reload).

### P4: VS segments + editors + endpoint (4–6 d)

- Host `JsonSchemaSegment` for the merged file.
- Package-folder discovery (derived from `@(ReferencePath)` `NuGetPackageId`/path metadata), de-duplicated against existing segments.
- Opt-in `.vscode/settings.json` scaffolding (comment-preserving; warning + snippet fallback).
- Endpoint joint in Dovetail.Hosting.Web, emitted via an MSBuild template when `DovetailConfigurationSchemaEndpoint=true`.

### P5: Dogfood + docs (2–3 d)

- `sample/SdkSample/SdkSample.Greetings`: `GreetingsOptions` with `[DovetailConfiguration]` (default section `Greetings`), consumed through `IOptions<GreetingsOptions>`. Add a record-based options example and a checked-in `.vscode/settings.json`.
- Rewrite `docs/src/content/docs/concepts/managed-configuration.md`; add an editor-setup page; README.

**Risks:**
- Binder drift across .NET versions (parity tests per TFM).
- Comment-preserving JSONC editing (fallback exists).
- Package discovery in design-time builds (S1).
- Strict schema false positives (opt-out exists).
- Analyzer-flow fallout in external consumers (gating + LangVersion guard + release notes).
- One-time Sdk snapshot churn (`AllPropertyNames`, OutputFiles): review, don't blindly accept.

## Verification

- **Analyzers** (`test/Dovetail.Analyzers.Tests`, Verify.SourceGenerators):
  - fragments for both attribute forms, default section naming, registration order, records/init, all diagnostics;
  - doc comments parsed vs not parsed;
  - an incremental caching test (`trackIncrementalGeneratorSteps`);
  - cross-assembly fixtures via the `GenerationHelpers.CreateDeps` pattern;
  - a new `AddConfigurationOptionsReferences()` helper so existing snapshots don't churn.
- **Task unit tests:**
  - merge tree, overlaps, strict `propertyNames` with case-insensitivity, SchemaStore base;
  - malformed and unknown-version fragments skipped;
  - JSONC scaffolding cases, including the fallback;
  - every output validated against the draft-07 meta-schema.
- **Runtime parity** (`test/Dovetail.Configuration.Runtime.Tests`, net10 + net11):
  - Dovetail binder/factory vs reflection `ConfigurationBinder` (called via a delegate) vs Microsoft's source-gen binder (`EnableConfigurationBindingGenerator` project);
  - DT0015 (class plus assembly form); reload; validators; `ValidateOnStart`;
  - an `IsAotCompatible` project with IL2026/IL3050/IL2091 as errors.
- **Sdk** (`test/Dovetail.Sdk.Tests`):
  - the library obj schema and nupkg root `ConfigurationSchema.json` (`VerifyProjectsAndPackages`, no NU5118);
  - host `obj/` + `bin/` + publish files for ProjectReference, PackageReference and **Hosting-only transitive** hosts;
  - a test project merging;
  - incremental no-op; stale-output deletion; multi-TFM;
  - `EvaluateItemIdentities("JsonSchemaSegment")` (host + discovered packages, de-duplicated);
  - endpoint property → generated joint;
  - `VerifyBuildWarnings` empty.
- **End to end:**
  - `mise run sample:sdk` prints the greeting via `IOptions<GreetingsOptions>`;
  - `obj/appsettings.schema.json` contains a strict `Greetings` section plus the SchemaStore base;
  - VS Code + YAML extension completion works with the scaffolded settings;
  - `curl /.well-known/dovetail/configuration-schema` on SdkSample.Web with the property on.
- Full `dotnet test` (Analyzers, Sdk, Runtime) with real ICU.
