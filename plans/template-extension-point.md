# Decision: ship host-model templates through a Dovetail extension point

Status: **accepted (2026-10-30).** Proposed on 2026-10-29 and approved by the owner the next day: option A, and the name and shape of the contract (open question 1 below). Open questions 2 to 5 were not answered individually and do not block this decision. Spike for [#265](https://github.com/RocketSurgeonsGuild/Dovetail/issues/265), part of RocketSurgeonsGuild/LaunchPad#3045. The extension point is implemented in the same PR, so the decision was judged against working code.

## Question

Dovetail's hosting package emits its helpers and joints as `*.cs.template` files, only into projects whose host model and referenced assemblies match, using internal items (`_DovetailHostingHelper` and friends in `Joinery.Dovetail.Hosting.targets`). LaunchPad's rebuilt hosting package (LaunchPad#3058) needs the same, plus a mode that emits only into projects with a **direct** reference to the shipping package, not ones that receive it transitively. Does Dovetail expose that as a supported, documented extension point, or does LaunchPad reimplement the gating?

## Recommendation

**Expose it from `Joinery.Dovetail` as a supported extension point**, with `Joinery.Dovetail.Hosting` as its first user and a `DirectReferenceOnly` mode. The contract is MSBuild items, documented in [Shipping Templates](../docs/src/content/docs/guides/shipping-templates.md) and tested in `TemplateExtensionPointTests`:

| Piece                                            | Role                                                                                                          |
| ------------------------------------------------ | ------------------------------------------------------------------------------------------------------------- |
| `DovetailTemplate` item                          | A package declares a template: `File`, and the gates `Package`, `DirectReferenceOnly`, `References`, `Hosts`. |
| `DovetailResolveTemplates` target                | After the references resolve, applies the gates and produces the `DovetailEmittedTemplate` items.             |
| `DovetailGenerateTemplates` target               | Writes each emitted template to `obj/` (`<OutputPrefix>.<name>.g.cs`) and adds it to the compilation.         |
| `HostName` / `HostType`, `OutputPrefix` metadata | One template emitted once per host model; the generated file name.                                            |

It lives in `src/Joinery.Dovetail/build/Joinery.Dovetail.Templates.targets`, beside `Joinery.Dovetail.Joints.targets` and `Joinery.Dovetail.Hosts.targets`, whose `DovetailJoint` and `DovetailHost` items are already a public MSBuild surface of the same kind.

## Reasons

1. **The gating is the hard part, and it is subtle.** The hosting targets get several things right that are easy to get wrong in a copy: resolving against `ReferencePath` after `ResolveAssemblyReferences` (so the project and package analyzers are known), running before `CoreCompile` _and_ WinUI's `XamlPreCompile`, before the `DovetailHost` targets read their conditions, `WriteOnlyWhenDifferent` plus `FileWrites` for incrementality, `[MSBuild]::Escape` on the written lines, and the no-backslashes rule. A LaunchPad-owned copy would duplicate about 100 lines of this and every later fix would have to land twice.
2. **Direct-reference detection belongs with the reference model.** `buildTransitive/Joinery.Dovetail.targets` already detects "a direct `PackageReference`" to add the implicit `using`, and Dovetail's SDK keys its detection on `PackageReference` items. One shared definition of direct (a `PackageReference`, including the SDK's implicit ones, or a `ProjectReference` to a project named after the package) keeps LaunchPad's SDK (LaunchPad#3072: "detection uses direct package references only") and its templates consistent with Dovetail's.
3. **It is not LaunchPad-specific.** Any package can have the same need, Dovetail's own integration packages (Serilog, OpenTelemetry, the DI containers) and third-party ones alike: ship joints that depend on a library the package must not depend on. The constitution asks to extend existing abstractions rather than add parallel ones, and to keep implementations depending on Dovetail's abstractions, not on each other.
4. **It is proven by moving real code onto it.** `Joinery.Dovetail.Hosting` now declares its helpers as `DovetailTemplate` items and adds its OpenTelemetry joints to the emitted set, and its output is byte-for-byte unchanged (see Verification). That covers references gating, per-host token substitution and the "target adds to the emitted set" hook.

## What `DirectReferenceOnly` is for (a finding that narrows the mode, not the extension point)

NuGet already imports a package's `build/` assets only into projects that reference it directly (a test pins that: a template shipped from `build/` is never even declared in a transitive reference). So for a package consumed from NuGet that ships from `build/`, direct-only comes for free. The mode earns its place where NuGet cannot help:

- **Consumption by `ProjectReference`**, which is how LaunchPad's own samples and tests will consume `src/Hosting` (and how this repository's test projects consume `Joinery.Dovetail.Hosting`). A project reference imports no package assets, so the solution imports the targets into _every_ project, and only the mode keeps a library that the sample happens to reference from receiving them.
- **A package shipped from `buildTransitive/`**, which packages reached through other packages need.

Two details worth knowing: the .NET SDK also lists a referenced project's own project references as `ProjectReference` items (marked with `NuGetPackageId`), which must not count as direct (found by the first test run, and now covered); and the Dovetail SDK's implicit package references count as direct, since they are `PackageReference` items of the project (by construction, not separately tested).

## Trade-offs (option A, the recommendation)

- **A new public contract.** Item, metadata and target names are API, under semantic versioning like `DovetailJoint` and `DovetailHost`. Changing them later is a breaking change for third-party packages. Mitigation: the surface is small, documented and tested; the only target names documented as hooks are `DovetailResolveTemplates` and `DovetailGenerateTemplates`.
- **Version coupling.** A package that declares `DovetailTemplate` needs the Joinery.Dovetail version that introduced it. LaunchPad.Hosting depends on Joinery.Dovetail already, so this is a minimum-version bump, not a new dependency.
- **Evaluation cost.** One more import in every project that gets Dovetail's build assets. The resolver target only runs when a `DovetailTemplate` is declared, so unaffected projects pay for parsing the file and nothing else.
- **Deliberate limits.** Substitution is `{HostName}` and `{HostType}` only (what hosting needs); a package needing other tokens writes its own target, as the schema endpoint joint still does. `Hosts` sees the hosts declared when templates resolve, before a package narrows them (hosting's `_DovetailHostingGateHosts`). Direct `ProjectReference` detection relies on the project file being named after the package id. A project that does not run the Dovetail generator gets no templates, as hosting's helpers never did.

## Option B: LaunchPad-owned gating

LaunchPad copies the mechanism into `Rocket.Surgery.LaunchPad.Hosting` and adds the direct-reference check there.

- For: no new public contract in Dovetail, and LaunchPad can shape the mechanism freely, which matters most while LaunchPad's needs are still being discovered (MAUI and Uno are explicitly out of scope for now).
- Against: the duplication and drift above; a second, subtly different definition of "direct reference" and "host model"; every later package that needs it solves it again; and when Dovetail's gating improves, LaunchPad has to notice.
- Reversible: because option A is additive, LaunchPad can still choose to own its gating later, without Dovetail having to carry anything it will not use. The reverse (adopting Dovetail's later) is a rewrite of each LaunchPad template declaration.

## What changed for Joinery.Dovetail.Hosting

Nothing observable. It keeps its `_DovetailHostingHelper` list (so a helper added by a sibling change is picked up), declares each as a `DovetailTemplate`, and keeps its default (transitive-inclusive) gating. Making hosting `DirectReferenceOnly` would be a behaviour change (a library that only receives the package would stop getting its helpers) and is **not** part of this decision.

## Verification

- All 42 generated `Dovetail*.g.cs` files the in-repo hosting, ASP.NET Core, OpenTelemetry and WebAssembly test projects produce under `obj/` (helpers, the OpenTelemetry joint, services joint and the token-substituted WebAssembly start joint) are identical before and after, compared with `diff -r`.
- `HostingHelperTests` (every helper and OpenTelemetry joint gate, built against packed packages on net10.0 and net11.0) pass unchanged.
- `TemplateExtensionPointTests`: a template with `DirectReferenceOnly` reaches projects with a direct reference and nothing reaches a project that only receives the package transitively (which does import the targets and reference the assembly); the other gates behave as documented; a project reference counts as direct.

## Open questions for the maintainers

1. **Answered 2026-10-30: approved.** Confirm option A, and the name and shape of the contract (`DovetailTemplate`, `DirectReferenceOnly`, `Hosts`, the two hook targets).
2. Is `{HostName}`/`{HostType}` enough, or should arbitrary tokens be part of the contract before LaunchPad needs them?
3. Should `Joinery.Dovetail.Environment.targets` (context extensions, gated on references and C# 14) and the property-driven schema endpoint joint move onto it? Not done here: their gating differs, and this spike keeps hosting's output unchanged.
4. Should hosting itself later become `DirectReferenceOnly`, now that it can?
5. Should `DovetailHost` narrowing by packages (hosting drops hosts whose helper was not emitted) run before `Hosts` is evaluated, so `Hosts` reflects only hosts that will have entry points?
