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

That path is stable, so editor settings can point at it and be checked in. The build also copies the file next to the
application's `appsettings.json` in the output directory and always publishes it. Build the project once before expecting
completion: the file doesn't exist before then.

If a project that isn't detected as an application should merge a schema, or an application shouldn't, set
`DovetailConfigurationSchemaHost` to `true` or `false`. The other properties (`DovetailConfigurationSchemaStrict`,
`DovetailConfigurationSchemaIncludeSchemaStore`, `DovetailConfigurationSchema`) are described under
[the schema](../concepts/managed-configuration#the-schema).

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
`yaml.schemas` and `evenBetterToml.schema.associations`). Comments, trailing commas, indentation and a byte-order mark
are preserved, other settings are untouched, and re-running changes nothing. If the file can't be updated safely, for
example because it isn't valid JSON, the build reports a warning with the snippet to paste and leaves the file as it
was. Dovetail never adds `$schema` keys or `# yaml-language-server` modelines to your configuration files.

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

An application can also serve its schema, for tools that read it over HTTP. This needs Dovetail.Hosting.Web and is off by
default:

```xml
<PropertyGroup>
  <DovetailConfigurationSchemaEndpoint>true</DovetailConfigurationSchemaEndpoint>
  <!-- Optional: serve it only in these environments -->
  <DovetailConfigurationSchemaEndpointEnvironments>Development;Staging</DovetailConfigurationSchemaEndpointEnvironments>
</PropertyGroup>
```

The build then generates a joint that maps `GET /.well-known/dovetail/configuration-schema`. It serves
`appsettings.schema.json` from the application's directory, and falls back to the project's `obj/` folder when run from
source. Point a mapping at the URL, for example `"url": "http://localhost:5000/.well-known/dovetail/configuration-schema"`
in `json.schemas`.

> [!WARNING]
> The schema lists every configuration key your application and its libraries accept. With no environment restriction the
> endpoint is mapped everywhere, including production. Restrict it, or leave it off.

## Troubleshooting

- **No completion at all.** Build the project: the file is written after compilation. Then check the path in the mapping
  is relative to the folder VS Code opened, and that the file name matches `appsettings*`.
- **A key is flagged as unknown.** Check its spelling against the options class. Keys can't be added to a section that
  `[DovetailConfiguration]` declares; set `DovetailConfigurationSchemaStrict` to `false` to stop the check.
- **A section is missing from completion.** The application's schema only covers libraries it references. Check the
  library has `[DovetailConfiguration]` on its options type and that the application builds against it.
- **Quoted numbers are flagged.** `"Timeout": "30"` is an incorrect type to the editor even though the binder accepts it.
  Write the number without quotes.
