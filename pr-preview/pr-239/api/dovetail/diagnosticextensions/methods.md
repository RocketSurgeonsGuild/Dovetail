# DiagnosticExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-239/api/dovetail.md)
- Type: [DiagnosticExtensions](/Dovetail/pr-preview/pr-239/api/dovetail/diagnosticextensions.md)
- Kind: `Methods`
- Members: `4`

Provides extension methods for retrieving metadata from Dovetail context builders and contexts.

<a id="getconnectedto"></a>
<a id="getconnectedto-type"></a>

## GetConnectedTo(Type)

- Name: `GetConnectedTo(Type)`
- Modifiers: `static`
- Returns: `IEnumerable<Type>`

Gets the types of the steps that this joint is connected to

```csharp
public static class DiagnosticExtensions
{
    public static IEnumerable<Type> GetConnectedTo(
        Type jointType)
    {
        // ...
    }
}
```

## Parameters

- `jointType` (`Type`)

<a id="getconnectedtostepnames"></a>
<a id="getconnectedtostepnames-type"></a>

## GetConnectedToStepNames(Type)

- Name: `GetConnectedToStepNames(Type)`
- Modifiers: `static`
- Returns: `IEnumerable<string>`

Gets the names of the steps that this joint is connected to

```csharp
public static class DiagnosticExtensions
{
    public static IEnumerable<string> GetConnectedToStepNames(
        Type jointType)
    {
        // ...
    }
}
```

## Parameters

- `jointType` (`Type`)

<a id="getdiagnosticmetadata"></a>
<a id="getdiagnosticmetadata-dovetailcontextbuilder-ienumerable-dovetailcategory"></a>

## GetDiagnosticMetadata(DovetailContextBuilder, IEnumerable<DovetailCategory>)

- Name: `GetDiagnosticMetadata(DovetailContextBuilder, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-239/api/dovetail/dovetaildiagnosticmetadatajoint.md)

```csharp
public static class DiagnosticExtensions
{
    public static IEnumerable<DovetailDiagnosticMetadataJoint> GetDiagnosticMetadata(
        this DovetailContextBuilder builder,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcontextbuilder.md))
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcategory.md))

<a id="getdiagnosticmetadata-idovetailcontext-ienumerable-dovetailcategory"></a>

## GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)

- Name: `GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-239/api/dovetail/dovetaildiagnosticmetadatajoint.md)

```csharp
public static class DiagnosticExtensions
{
    public static IEnumerable<DovetailDiagnosticMetadataJoint> GetDiagnosticMetadata(
        this IDovetailContext builder,
        params IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([IDovetailContext](/Dovetail/pr-preview/pr-239/api/dovetail/idovetailcontext.md))
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-239/api/dovetail/dovetailcategory.md))
