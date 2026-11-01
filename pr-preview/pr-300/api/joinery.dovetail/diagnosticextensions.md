# DiagnosticExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Version: `0.4.2-pr.13`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`

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

- [GetConnectedTo(Type)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/diagnosticextensions/methods.md#getconnectedto-type) : `IEnumerable<Type>` `static` -- Gets the types of the steps that this joint is connected to
- [GetConnectedToStepNames(Type)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/diagnosticextensions/methods.md#getconnectedtostepnames-type) : `IEnumerable<string>` `static` -- Gets the names of the steps that this joint is connected to
- [GetDiagnosticMetadata(DovetailContextBuilder, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-dovetailcontextbuilder-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
- [GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-idovetailcontext-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
