# DiagnosticExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.23`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail/Diagnostics/DiagnosticExtensions.cs)

Provides extension methods for retrieving metadata from Dovetail context builders and contexts.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public static class DiagnosticExtensions
{
    // ...
}
```

## Methods

- [GetConnectedTo(Type)](/Dovetail/api/joinery.dovetail/diagnosticextensions/methods.md#getconnectedto-type) : `IEnumerable<Type>` `static` -- Gets the types of the steps that this joint is connected to
- [GetConnectedToStepNames(Type)](/Dovetail/api/joinery.dovetail/diagnosticextensions/methods.md#getconnectedtostepnames-type) : `IEnumerable<string>` `static` -- Gets the names of the steps that this joint is connected to
- [GetDiagnosticMetadata(DovetailContextBuilder, IEnumerable<DovetailCategory>)](/Dovetail/api/joinery.dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-dovetailcontextbuilder-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
- [GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)](/Dovetail/api/joinery.dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-idovetailcontext-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
