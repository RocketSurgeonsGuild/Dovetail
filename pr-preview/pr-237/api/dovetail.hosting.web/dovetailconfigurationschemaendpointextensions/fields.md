# DovetailConfigurationSchemaEndpointExtensions Fields

- Package: [Dovetail.Hosting.Web](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.web.md)
- Type: [DovetailConfigurationSchemaEndpointExtensions](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.web/dovetailconfigurationschemaendpointextensions.md)
- Kind: `Fields`
- Members: `3`

Serves the application's merged configuration JSON Schema ( `appsettings.schema.json`) over HTTP.

<a id="routepattern"></a>

## RoutePattern

- Name: `RoutePattern`
- Modifiers: `static` `const`
- Returns: `string`

The route the schema is served from.

```csharp
public const static string RoutePattern
```

<a id="schemafilename"></a>

## SchemaFileName

- Name: `SchemaFileName`
- Modifiers: `static` `const`
- Returns: `string`

The name of the schema file, in the application's output directory and in `obj/`.

```csharp
public const static string SchemaFileName
```

<a id="schemamediatype"></a>

## SchemaMediaType

- Name: `SchemaMediaType`
- Modifiers: `static` `const`
- Returns: `string`

The media type the schema is served as.

```csharp
public const static string SchemaMediaType
```
