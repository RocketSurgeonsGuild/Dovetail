# P0: spike results for the configuration JSON Schema plan

Status: S2–S5 done; S1 needs Visual Studio on Windows. See [the plan](configuration-json-schema.md).

All spikes ran on Linux with SDK 11.0.100-rc.2.26504.105 (MSBuild 18.12, Roslyn 5.x), net10.0 and net11.0. The spike code lives outside the repo (`~/.cache/dovetail-spikes/<spike>/`, each with a `REPORT.md`); this file is the summary.

## Verdict

**GO for PR B.** No spike found a blocker. Every risk that could be tested here either held up or has a tested fix; the Visual Studio questions (S1) still need a Windows run. The plan changes below are small and add ~3–4 days (P3's per-version binder rules, P2's embedded SchemaStore). Only P4's Visual Studio work depends on S1, so P1–P3 can start.

Decisions made after P0 (2026-10-24):

1. **Quoted numbers and booleans** (`"30"`, `"true"`): keep strict types. Ranges are always checked, and quoted values get an "Incorrect type" warning even though the binder accepts them.
2. **Values under differently-cased keys**: validate them. Each library fragment gives every key a case-insensitive `patternProperties` entry with the same subschema, so `retries: 99` is checked and hovered in every engine. Taplo reports a bad canonical value twice, and fragments roughly double in size.
3. **Descriptions for third-party types** in the assembly-level form: Dovetail's targets pass the XML doc files of package references to the generator as `AdditionalFiles`, and the generator reads only the file for the type's assembly. This is part of P1.

## S1: Visual Studio schema segments (needs a Windows run)

Established here, from packages:

- Aspire client packages (checked: `Aspire.StackExchange.Redis` 13.6.4) ship `ConfigurationSchema.json` at the package root **and** declare it themselves in `buildTransitive/<tfm>/<id>.targets`: `<JsonSchemaSegment Include="$(MSBuildThisFileDirectory)..\..\ConfigurationSchema.json" FilePathPattern="appsettings\..*json" />`. So de-duplicating discovered package segments against existing `@(JsonSchemaSegment)` items (by full path) is required, not defensive.
- The segment format is a schema rooted at the configuration root (`type: object`, sections nested under `properties`), plus a VS-specific `definitions.logLevel` merge point that `$ref`s `#/definitions/logLevelThreshold`, which the file never defines; VS supplies it. Dovetail's library `ConfigurationSchema.json` should use the same root-anchored shape.
- `Aspire.Hosting.AppHost` declares its segment the same way (`AspireAppHostConfiguration.json`, same `FilePathPattern`).
- Segments only affect Visual Studio's JSON editor; YAML and TOML editing in VS gets nothing from them.

Checklist for a Visual Studio 2026 (18.x) machine, using a console app with:

```xml
<ItemGroup>
  <JsonSchemaSegment Include="$(BaseIntermediateOutputPath)appsettings.schema.json" FilePathPattern="appsettings\..*json" />
</ItemGroup>
<Target Name="WriteSchema" AfterTargets="CoreCompile">
  <WriteLinesToFile File="$(BaseIntermediateOutputPath)appsettings.schema.json" Overwrite="true"
    Lines="{ &quot;type&quot;: &quot;object&quot;, &quot;properties&quot;: { &quot;Greetings&quot;: { &quot;type&quot;: &quot;object&quot;, &quot;properties&quot;: { &quot;Message&quot;: { &quot;type&quot;: &quot;string&quot;, &quot;description&quot;: &quot;v1&quot; } } } } }" />
</Target>
```

1. Open `appsettings.json` before the first build (segment file missing): any error? After building, does completion offer `Greetings`/`Message` without reopening the file or reloading the project?
2. Change `v1` to `v2`, rebuild: does hover update, and when?
3. Add the same segment item twice: duplicate completions or errors?
4. Move the item into a target (`<Target Name="AddSegment" BeforeTargets="ResolveAssemblyReferences"><ItemGroup><JsonSchemaSegment … /></ItemGroup></Target>`): does VS still see it, i.e. do design-time builds run it? This decides whether package discovery can work from `@(ReferencePath)` or must be computed at evaluation time.
5. Reference `Aspire.StackExchange.Redis` and add a second, Dovetail-discovered item for its `ConfigurationSchema.json`: confirm the de-duplication prevents doubled completions.

## S2: doc comments in the generator (GO, with a design change)

Generator compiled against Roslyn 5.0.0; consumer built with `GenerateDocumentationFile` false and true.

| | `GenerateDocumentationFile=false` (the default) | `=true` |
|---|---|---|
| `DocumentationMode` | `None` | `Diagnose` |
| `GetDocumentationCommentXml()`, source type | empty | full XML |
| `///` and `/** */` as trivia | ordinary comment trivia | structured doc trivia |
| Parsing the comment text from trivia | every summary recovered | identical result |
| `GetDocumentationCommentXml()`, type from a package | empty | empty |

- Parse the comment text from the declaration's leading trivia **as the only path** for source types, not as a fallback: it works in both modes and gives identical results (`<see cref>`, `<paramref>`, entities, `/** */`), so descriptions don't depend on a library opting into `GenerateDocumentationFile` (and its CS1591 noise). `<inheritdoc/>` and `<include>` yield no description.
- The command-line compiler attaches no documentation to references, so the assembly-level form gets no descriptions for third-party types from Roslyn. Packages do ship the XML (`lib/net10.0/Microsoft.Extensions.Hosting.xml` covers `HostOptions`). Either ship v1 without descriptions for those types, or have the targets pass package references' `.xml` files as `AdditionalFiles` and read only the one matching the type's assembly (analyzers can't read files themselves, RS1035).

## S3: compiled MSBuild task (GO)

A netstandard2.0 task reads `Dovetail.ConfigurationSchema.v1:<Section>` metadata from `@(ReferencePath)` with System.Reflection.Metadata (no assembly loading), merges a case-insensitive section tree with System.Text.Json, and writes `obj/appsettings.schema.json`; a second task edits `.vscode/settings.json` in place.

| Mode | Where it ran | STJ / SRM bound to | Task time, 170 references |
|---|---|---|---|
| `dotnet build` | in-proc (`MSBuildLoadContext`) | 11.0.0.0 shared framework | 16–29 ms |
| `-m:2` | in-proc node | 11.0.0.0 | 29 ms |
| `-mt` | out-of-proc sidecar (task not marked multithreadable) | 11.0.0.0 | 29 ms |
| `TaskHostFactory` | out-of-proc task host | 11.0.0.0 | 16 ms |

Visual Studio's .NET Framework `MSBuild.exe` couldn't run here; its binding redirects (`MSBuild.exe.config` in Microsoft.Build.Runtime) cap what a task may compile against:

| MSBuild | System.Text.Json | System.Reflection.Metadata |
|---|---|---|
| 17.8 | 7.0.0.3 | 7.0.0.0 |
| 17.11 | 8.0.0.5 | 8.0.0.0 |
| 17.14, 18.0 | 9.0.0.0 | 9.0.0.0 |
| 18.11 | 10.0.0.10 | 10.0.0.10 |

- Compile the task against System.Text.Json and System.Reflection.Metadata **9.0.0**, compile-only, shipping no copies. That loads on MSBuild 17.14 and every 18.x. Dovetail already needs more than that: its packages target net10.0 and net11.0 (the .NET 10 SDK), and its analyzer ships only `roslyn5.x` folders (Roslyn 5.0). Use 18.x MSBuild compile references (17.11.31 trips NU1903).
- Pass only absolute paths into the task: MSBuild 18.12 has `MSBuildMultiThreadableTaskAttribute`/`TaskEnvironment` for running in-proc under `-mt`, where the process working directory isn't the project's.
- When the same section comes from several assemblies, its casing is currently whichever comes first in `@(ReferencePath)`; define a deterministic rule (e.g. ordinal).
- JSONC: splicing text at `Utf8JsonReader` byte offsets preserved comments, trailing commas, indentation (spaces or tabs) and the BOM. It was idempotent, and invalid files were left byte-identical with a warning and the snippet. No comment-preserving JSON library is needed. `yaml.schemas` and `evenBetterToml.schema.associations` are objects rather than arrays and need an object insert.
- Still unverified (Windows): running inside Visual Studio, the DLL-lock behaviour `TaskHostFactory` avoids during in-repo development, and design-time builds.

## S4: editors (GO, with schema-writer changes)

These ran headlessly through the real engines: VS Code's JSON service (vscode-json-languageservice 6.0.0-next.4, the version VS Code 1.143.1 ships), yaml-language-server 2.0.0 (Red Hat YAML 2.0.0), and Taplo (@taplo/lsp 0.8.0, as bundled by Even Better TOML). Visual Studio and Rider are manual checks.

| | JSON | YAML | TOML (Taplo) |
|---|---|---|---|
| unknown key (`Mesage`) flagged | yes | yes | yes, but on the table header (the CLI reports 21 diagnostics for one typo) |
| differently-cased key (`message`) accepted | yes; its value isn't checked | same | same |
| canonical names completed | yes | yes, plus a junk item unless `doNotSuggest` is set | yes |
| hover shows `description` | yes | yes | Dovetail keys only |
| SchemaStore `$ref` while offline | all validation skipped | nothing works | nothing works, silently |
| `allOf` of two strict branches | rejects every key | same | same |
| `deprecated` / `deprecationMessage` | warns / warns and hides | ignored / warns and hides | both ignored |

Design changes:

- Embed SchemaStore's appsettings schema (under `definitions`, refs rewritten) instead of `$ref`-ing it. Online and offline results then match in all three engines, and Taplo gains SchemaStore completion (22 root items become 47). The copy ships with Dovetail and is refreshed per release. SchemaStore is Apache-2.0, so keep its notice.
- Overlapping sections: keep the branches open, and put one combined `propertyNames` and `"type": "object"` on the merged object. The plan left overlapping sections open; this keeps them strict.
- Every `propertyNames` gets a `patternErrorMessage` (`Unknown key in section "Greetings". Known keys (case-insensitive): …`) and `"doNotSuggest": true`.
- Emit `deprecated: true` and `deprecationMessage` together. Give each section a `title` and drop the root one. Put ranges, defaults and units into `description`, since no engine shows `default`, `minimum` or `format` on hover.
- Keep the character-class regexes; `(?i)` works in these engines but not in Node's `RegExp`.
- Taplo can't use the SchemaStore catalog, so the scaffolded `evenBetterToml.schema.associations` entry is required for TOML, not optional.

Two behaviours need a decision (below): values under differently-cased keys aren't validated, and quoted numbers are flagged although the binder accepts them.

## S5: configuration binder behaviour (GO, with three scope decisions)

One harness binds the same data through `Bind`, `Get<T>` and `Configure<T>` + `IOptions<T>` in four variants: reflection and source-gen (`EnableConfigurationBindingGenerator`) × Microsoft.Extensions.* 10.0.13 and 11.0.0-rc.2 (a 340-row matrix, ~800 outcomes per variant). Interception is proven: 184 of 190 call sites, and a run-time probe attributes every read to the binder that made it. Rebuilding SG10 with SDK 10.0.402 changed nothing, so behaviour follows the **Binder package version**, not the SDK.

What source-gen does in both versions, and a mimicking generator must reproduce:

- `""` on a non-nullable value type leaves the current value (reflection throws or converts).
- Invalid collection elements and dictionary keys throw (reflection skips them).
- `IEnumerable`/`IReadOnlyList`/`IReadOnlyCollection` properties become `List<T>`. Only declared property types are bound, and `Get<T>()` on a section with only a value returns an instance.
- `ErrorOnUnknownConfiguration` checks `ConfigurationKeyName` names.
- Append and merge for lists, arrays, sets and dictionaries match reflection.

Where SG10 and SG11 differ:

| | SG10 | SG11 |
|---|---|---|
| `Nullable<T>` set to `""` | value kept | null |
| value-type ctor parameter set to `""` | default | throws |
| missing string or nullable ctor parameter | throws | null |
| settable properties re-bound after the ctor | yes | no |

Where source-gen gives nothing to copy:

- It never binds init-only properties of classes; `Bind` on an existing record reads nothing. Only constructor-based creation sets init-only properties (in the object initializer).
- `required` members make the generated `Get<T>()` fail to compile (CS9035).
- `BindNonPublicProperties=true`, and `Get<int?>()` on a `""` section, throw `NotSupportedException`.
- Calls it can't resolve statically, such as generic helpers (SYSLIB1104), silently fall back to reflection. Calls in another generator's output are invisible to it as well, so Dovetail has to emit its own binder rather than call Microsoft's.

Decisions:

1. Pin parity per **Binder major version**, detected from the compilation's reference to `Microsoft.Extensions.Configuration.Binder`, not per target framework; a net10.0 app can reference Binder 11. Parity tests run against both.
2. Immutable types and `required` members are Dovetail-specific by necessity. The generated `IOptionsFactory<T>` uses source-gen's constructor rules for the detected Binder version, and sets init-only and `required` members in the object initializer (as reflection does). This is documented as a deliberate difference.
3. Don't reproduce source-gen's `NotSupportedException`s, and don't offer `BindNonPublicProperties`.

Cost: P3 grows by ~2–3 days for the per-version rules and the second parity run.

## Changes to the plan

- P1: XML doc descriptions come from trivia parsing, not `GetDocumentationCommentXml()`. Third-party types in the assembly-level form get descriptions from package XML doc files passed as `AdditionalFiles`; +~1–2 days (S2, decision 3).
- P1: fragments give every key a case-insensitive `patternProperties` entry (decision 2). The task derives each object's key list from the fragments' `properties`, so fragments need no separate name list (S3).
- P1 (from the code review): each object type is described once, under the fragment's `definitions` (no `type` keyword, so nullable uses still accept `null`). Every use of it is `{"type": …, annotations, "allOf": [{"$ref": "#/definitions/<Name>"}]}`. Inlining had made fragments grow exponentially with nesting (every key also has a case-insensitive twin), and recursion is now a `$ref` instead of a cut-off. P2 moves the definitions to the merged document's root, rewrites the references, and closes every Dovetail object with `properties`, definitions included.
- P1 (from the code review): types follow Microsoft's binding source generator rather than the plan's original list. That adds `object`/`IConfigurationSection` (any value), `CultureInfo`, `Half`, `Int128`/`UInt128`, collection classes such as `SortedDictionary` or a `List<T>` subclass, and integer dictionary keys. Types with a `[TypeConverter]` are described as objects, as the generator ignores converters. Dates and times use loose ISO 8601 patterns instead of `format`, which flagged values the binder accepts. Enums accept any casing.
- P1/P2: the schema writer embeds SchemaStore, closes overlapping sections once, adds `patternErrorMessage` and `doNotSuggest`, pairs `deprecated` with `deprecationMessage`, titles sections, and puts ranges and defaults into descriptions; +~1 day (S4).
- P2: the task compiles against System.Text.Json / System.Reflection.Metadata 9.0.0 compile-only, takes absolute paths only, and resolves section casing deterministically (S3).
- P3: binder semantics are pinned per Binder major version (10 vs 11), immutable types and `required` members follow documented Dovetail rules, and source-gen's `NotSupportedException`s aren't reproduced; +2–3 days (S5).
- P4: package-segment de-duplication is required (Aspire packages declare their own segments) (S1); TOML scaffolding is required because Taplo can't use the SchemaStore catalog (S4).
