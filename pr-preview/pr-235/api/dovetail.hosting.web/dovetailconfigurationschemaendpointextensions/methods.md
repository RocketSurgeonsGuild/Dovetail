# DovetailConfigurationSchemaEndpointExtensions Methods

- Package: [Dovetail.Hosting.Web](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.web.md)
- Type: [DovetailConfigurationSchemaEndpointExtensions](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.web/dovetailconfigurationschemaendpointextensions.md)
- Kind: `Methods`
- Members: `1`

Serves the application's merged configuration JSON Schema ( `appsettings.schema.json`) over HTTP.

<a id="mapdovetailconfigurationschema"></a>
<a id="mapdovetailconfigurationschema-iendpointroutebuilder-string-string"></a>

## MapDovetailConfigurationSchema(IEndpointRouteBuilder, string?, string?)

- Name: `MapDovetailConfigurationSchema(IEndpointRouteBuilder, string?, string?)`
- Modifiers: `extension` `nullable`
- Returns: `IEndpointConventionBuilder?`

Maps `GET /.well-known/dovetail/configuration-schema`, which serves `appsettings.schema.json` from the application's output directory (falling back to `fallbackDirectory`). Responds `404` when no file exists, and supports `ETag` and `If-None-Match`.

```csharp
public static class DovetailConfigurationSchemaEndpointExtensions
{
    public static IEndpointConventionBuilder? MapDovetailConfigurationSchema(
        this IEndpointRouteBuilder endpoints,
        string? fallbackDirectory = null,
        string? environments = null)
    {
        // ...
    }
}
```

## Parameters

- `endpoints` (`IEndpointRouteBuilder`)
  The endpoint route builder, typically the `WebApplication`.
- `fallbackDirectory` (`string?`) `optional`
  A directory to look in when the output directory has no schema file, typically the project's `obj/` directory when running from source.
- `environments` (`string?`) `optional`
  Semicolon-separated host environment names. When set, the endpoint is only mapped when the current environment is one of them.

## Returns

`IEndpointConventionBuilder?` -- The endpoint convention builder, or `null` when `environments` excludes the current environment.

## Exceptions

- `ArgumentNullException` -- `endpoints` is `null`.
