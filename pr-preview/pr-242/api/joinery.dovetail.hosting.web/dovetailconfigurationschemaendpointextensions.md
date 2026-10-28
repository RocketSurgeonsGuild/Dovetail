# DovetailConfigurationSchemaEndpointExtensions

- Kind: `class`
- Package: [Joinery.Dovetail.Hosting.Web](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.web.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Hosting.Web`
- Target framework: `net10.0`

Serves the application's merged configuration JSON Schema ( `appsettings.schema.json`) over HTTP.

## Definition

```csharp
namespace Dovetail.Hosting.Web;

public static class DovetailConfigurationSchemaEndpointExtensions
{
    // ...
}
```

## Remarks

Applications do not call this directly. Setting the MSBuild property `DovetailConfigurationSchemaEndpoint` to `true` generates a joint that maps the endpoint; `DovetailConfigurationSchemaEndpointEnvironments` restricts it to the listed environments. The implementation uses a plain `Http.RequestDelegate`, so it does not rely on reflection and is trimming and AOT friendly.

## Methods

- [MapDovetailConfigurationSchema(IEndpointRouteBuilder, string?, string?)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.web/dovetailconfigurationschemaendpointextensions/methods.md#mapdovetailconfigurationschema-iendpointroutebuilder-string-string) : `IEndpointConventionBuilder?` `extension` -- Maps `GET /.well-known/dovetail/configuration-schema`, which serves `appsettings.schema.json` from the application's output directory (falling back to `fallbackDirectory`). Responds `404` when no file exists, and supports `ETag` and `If-None-Match`.

## Fields

- [RoutePattern](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.web/dovetailconfigurationschemaendpointextensions/fields.md#routepattern) : `string` `static` -- The route the schema is served from.
- [SchemaFileName](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.web/dovetailconfigurationschemaendpointextensions/fields.md#schemafilename) : `string` `static` -- The name of the schema file, in the application's output directory and in `obj/`.
- [SchemaMediaType](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.web/dovetailconfigurationschemaendpointextensions/fields.md#schemamediatype) : `string` `static` -- The media type the schema is served as.
