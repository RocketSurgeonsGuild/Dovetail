# DiagnosticExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DiagnosticExtensions](/Dovetail/api/joinery.dovetail/diagnosticextensions.md)
- Kind: `Methods`
- Members: `4`

Provides extension methods for retrieving metadata from Dovetail context builders and contexts.

<a id="getconnectedto"></a>
<a id="getconnectedto-type"></a>

## GetConnectedTo(Type)

- Name: `GetConnectedTo(Type)`
- Modifiers: `static`
- Returns: `IEnumerable<Type>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail/Diagnostics/DiagnosticExtensions.cs#L29-L31)

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
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail/Diagnostics/DiagnosticExtensions.cs#L37-L39)

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
- Returns: [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail/Diagnostics/DiagnosticExtensions.cs)

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

- `builder` ([DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md))
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/api/joinery.dovetail/dovetailcategory.md))

<a id="getdiagnosticmetadata-idovetailcontext-ienumerable-dovetailcategory"></a>

## GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)

- Name: `GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)`
- Modifiers: `extension`
- Returns: [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail/Diagnostics/DiagnosticExtensions.cs)

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

- `builder` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/api/joinery.dovetail/dovetailcategory.md))
