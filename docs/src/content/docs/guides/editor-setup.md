---
title: Editor Setup
description: Get completion, hover documentation and validation for your Dovetail configuration in VS Code, Visual Studio and Rider.
tags: [configuration, json-schema, editors]
---

# Editor setup

Every application built with Dovetail gets one JSON Schema for its configuration, merged from the
[`[DovetailConfiguration]`](../concepts/managed-configuration) sections of every library it references. This guide
points your editor at it, so `appsettings.json`, YAML and TOML files complete section and key names, show the
documentation you wrote on the options classes, and flag unknown keys and out-of-range values.

## Where the schema comes from

A build of an application or a test project writes the merged schema to:

```text
<project>/obj/appsettings.schema.json
```

That path is stable, so editor settings can point at it and be checked in. Build the project once before expecting
completion: the file doesn't exist before then.

- A project that targets several frameworks writes that file from its first target framework. The others write
  `$(IntermediateOutputPath)appsettings.schema.json`, for example `obj/Debug/net10.0/appsettings.schema.json`.
- The build also copies the file next to the application's `appsettings.json` in the output directory and publishes it
  as `appsettings.schema.json`.
- Applications that embed their configuration (MAUI and Uno, where `DovetailEmbedConfiguration` is `true`) keep the file
  in `obj/` only. It is not copied to the output directory and not published, so editors still use the `obj/` path.
- A library (any project that isn't a host) doesn't write `appsettings.schema.json`. It writes
  `$(IntermediateOutputPath)ConfigurationSchema.json` and packs it at the package root.

If a project that isn't detected as an application should merge a schema, or an application shouldn't, set
`DovetailConfigurationSchemaHost` to `true` or `false`. The other properties (`DovetailConfigurationSchema`,
`DovetailConfigurationSchemaStrict`, `DovetailConfigurationSchemaIncludeSchemaStore`, `DovetailEmbedConfiguration` and
`DovetailConfigurationSchemaRequired`) are described under [the schema](../concepts/managed-configuration#the-schema).

## Visual Studio Code

VS Code needs one mapping per file type: its JSON support reads `json.schemas`, and the YAML and Even Better TOML
extensions read `yaml.schemas` and `evenBetterToml.schema.associations`. Put them in `.vscode/settings.json`, which allows
comments:

```jsonc
{
    "json.schemas": [
        {
            "fileMatch": ["/MyApp/appsettings*.json"],
            "url": "./MyApp/obj/appsettings.schema.json",
        },
    ],
    "yaml.schemas": {
        "./MyApp/obj/appsettings.schema.json": ["MyApp/appsettings*.yaml", "MyApp/appsettings*.yml"],
    },
    "evenBetterToml.schema.associations": {
        "^.+/MyApp/appsettings.*\\.toml$": "./MyApp/obj/appsettings.schema.json",
    },
}
```

The [SdkSample](https://github.com/RocketSurgeonsGuild/Dovetail/blob/main/sample/SdkSample/.vscode/settings.json) checks
in a working file for a solution with two applications.

Notes on the three extensions:

- **JSON** reports unknown keys, completes the canonical names and shows descriptions on hover.
- **YAML** (the Red Hat extension) does the same. Its `fileMatch` globs don't match files under a dot-folder.
- **TOML** needs the explicit association above: Even Better TOML can't use the SchemaStore catalog, and it reports an
  unknown key on the table header rather than on the key. It ignores `deprecated`.
- Keys are matched ignoring case, as the configuration system does: `message` is accepted, `Mesage` is flagged.

### Let Dovetail write the settings

Set `DovetailScaffoldEditorSettings` to `true` in the application's project file:

```xml
<PropertyGroup>
  <DovetailScaffoldEditorSettings>true</DovetailScaffoldEditorSettings>
</PropertyGroup>
```

On build, Dovetail creates or updates **only its own** entries in `.vscode/settings.json` (`json.schemas`,
`yaml.schemas` and `evenBetterToml.schema.associations`), pointing at the application's `obj/appsettings.schema.json`.
Comments, trailing commas, indentation, line endings and a byte-order mark are preserved, other settings and other
entries in those three maps are untouched, and re-running changes nothing (the file isn't even rewritten, so its
timestamp stays). Dovetail never adds `$schema` keys or `# yaml-language-server` modelines to your configuration files.

Each application gets entries of its own, keyed by its schema path, so several applications in one repository can
share one settings file. Dovetail recognises its entries by that path: an entry that already maps the application's
schema only gets whatever is missing (a file pattern, a glob), and anything you added to it stays. If you move a
project, delete its old entries by hand.

The file goes in the folder that holds `.vscode`: the nearest ancestor of the project that contains a `.git` entry, or
the project folder when there is none. Set `DovetailEditorSettingsDirectory` to use another folder (a relative path is
relative to the project). The folder must contain the project, because VS Code resolves the schema path relative to the
folder it has open. The file and the `.vscode` folder are created only when they don't exist.

Only applications write the file, once per build (from the first target framework of a project that targets several),
never in design-time builds, and not when `DovetailConfigurationSchema` is `false`.

If the file can't be updated safely, because it isn't valid JSON with comments, isn't UTF-8, has an unexpected shape
(say `json.schemas` isn't an array) or is read-only, Dovetail leaves it as it is and the build reports a warning with
the entries to paste. Fix the file, or set `DovetailScaffoldEditorSettings` to `false`, to get rid of the warning.

| Property                          | Default  | Effect                               |
| --------------------------------- | -------- | ------------------------------------ |
| `DovetailScaffoldEditorSettings`  | off      | `true` writes the mappings on build. |
| `DovetailEditorSettingsDirectory` | git root | The folder that holds `.vscode`.     |

It is off by default because it edits a file that is probably checked in.

## Visual Studio

Visual Studio's JSON editor reads schemas from `JsonSchemaSegment` items, with no per-file settings. Dovetail adds, with the
file pattern `appsettings\..*json` (the one Aspire's packages use):

- a segment for the application's (or test project's) merged `obj/appsettings.schema.json`;
- a segment for each referenced package that ships a `ConfigurationSchema.json` at its root, which is where libraries built
  with Dovetail pack theirs. A package that declares its own segment for the file (Aspire's client packages do) is skipped,
  so completions aren't doubled. Referenced projects need no segment: their sections are in the merged file.

Nothing needs configuring. Dovetail finds the packages from the build's resolved references, in the builds Visual Studio
runs when it loads the project too, not only in full builds. `DovetailConfigurationSchemaVisualStudio=false` turns the
segments off and keeps the schema files; `DovetailConfigurationSchema=false` turns off both. If completion doesn't appear
after the first build, close and reopen the file.

> [!NOTE]
> Segments only affect Visual Studio's JSON editor. YAML and TOML files get nothing from them in Visual Studio.

The repository's tests check the items MSBuild produces, including in a design-time build. Whether Visual Studio itself picks
up items a target adds, and how it merges two segments that describe the same section, has not been verified in Visual Studio.

## JetBrains Rider

Rider uses its own JSON Schema mappings rather than the VS Code settings or the segments:

1. Open **Settings | Languages & Frameworks | Schemas and DTDs | JSON Schema Mappings**.
2. Add a mapping, choose `obj/appsettings.schema.json` as the schema file, set the version to Draft 7, and add the
   application's `appsettings*.json` files as the mapped files.
3. Rider applies JSON Schema mappings to YAML files too. Add the `appsettings*.yaml` files the same way.

## TOML and YAML in other editors

Any editor with a language server for the format can use the schema file:

- **yaml-language-server** reads a `yaml.schemas` setting like VS Code's, or a `# yaml-language-server: $schema=...`
  modeline in the file.
- **Taplo** takes the schema from `.taplo.toml` or, in VS Code, `evenBetterToml.schema.associations`.

## The schema endpoint

An application can also serve its schema, for tools that read it over HTTP. This needs an ASP.NET Core application that references
Joinery.Dovetail.Hosting, and is off by default:

```xml
<PropertyGroup>
  <DovetailConfigurationSchemaEndpoint>true</DovetailConfigurationSchemaEndpoint>
  <!-- Optional: serve it only in these environments -->
  <DovetailConfigurationSchemaEndpointEnvironments>Development;Staging</DovetailConfigurationSchemaEndpointEnvironments>
</PropertyGroup>
```

| Property                                            | Default             | Effect                                                                                                                                                                                                                                                         |
| --------------------------------------------------- | ------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DovetailConfigurationSchemaEndpoint`               | off                 | `true` generates a joint that maps the endpoint. Only projects with `OutputType` `Exe` or `WinExe` that reference ASP.NET Core and Joinery.Dovetail.Hosting get it.                                                                                            |
| `DovetailConfigurationSchemaEndpointEnvironments`   | every environment   | A semicolon-separated list of environment names (`Development;Staging`, compared ignoring case). The endpoint is mapped only when the host environment is one of them.                                                                                         |
| `DovetailConfigurationSchemaEndpointSourceFallback` | `true` except below | Whether the project's `obj/` directory is compiled into the application as a place to look for the schema. By default it is, except in publish builds and `ContinuousIntegrationBuild` builds, so a build machine path isn't shipped. `false` never embeds it. |

The route is `GET /.well-known/dovetail/configuration-schema`:

- It looks for `appsettings.schema.json` in the application's output directory first, then in the embedded `obj/` fallback
  directory, which is how it works when run from source with no copy beside the assembly. A published application serves
  the copy published with it.
- The response is `application/schema+json` with a strong `ETag` and `Cache-Control: no-cache`. A request with a matching
  `If-None-Match` gets `304 Not Modified`. The file is re-read when it changes on disk.
- It answers `404` when neither location has the file.

Point a mapping at the URL, for example `"url": "http://localhost:5000/.well-known/dovetail/configuration-schema"` in
`json.schemas`.

To map it yourself, call the extension on the `WebApplication` (or any `IEndpointRouteBuilder`) instead of setting the
property. Joinery.Dovetail.Hosting emits it into every ASP.NET Core project that references it, as an internal type. Both arguments are optional, and the method returns `null` when `environments` excludes the current
environment:

```c#
using Joinery.Dovetail.Hosting;

app.MapDovetailConfigurationSchema(fallbackDirectory: null, environments: "Development")
   ?.RequireAuthorization();
```

> [!WARNING]
> The schema lists every configuration key your application and its libraries accept, including the section names of
> libraries you may not want to advertise. Unless you restrict it, the endpoint is mapped in every environment, production
> included, and anyone who can reach the application can read it, with no authentication. Set
> `DovetailConfigurationSchemaEndpointEnvironments` to the environments that need it (usually `Development`), map it
> yourself and add authorization, or leave it off.

## Troubleshooting

- **No completion at all.** Build the project: the file is written after compilation. Then check the path in the mapping
  is relative to the folder VS Code opened, and that the file name matches `appsettings*`.
- **A key is flagged as unknown.** Check its spelling against the options class. Keys can't be added to a section that
  `[DovetailConfiguration]` declares; set `DovetailConfigurationSchemaStrict` to `false` to stop the check.
- **A section is missing from completion.** The application's schema only covers libraries it references. Check the
  library has `[DovetailConfiguration]` on its options type and that the application builds against it.
- **Quoted numbers are flagged.** `"Timeout": "30"` is an incorrect type to the editor even though the binder accepts it.
  Write the number without quotes.
