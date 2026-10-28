# DiagnosticExtensions

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Diagnostics`
- Target framework: `net10.0`

Provides extension methods for retrieving metadata from Dovetail context builders and contexts.

## Definition

```csharp
namespace Dovetail.Diagnostics;

public static class DiagnosticExtensions
{
    // ...
}
```

## Methods

- [GetConnectedTo(Type)](/Dovetail/pr-preview/pr-228/api/dovetail/diagnosticextensions/methods.md#getconnectedto-type) : `IEnumerable<Type>` `static` -- Gets the types of the steps that this joint is connected to
- [GetConnectedToStepNames(Type)](/Dovetail/pr-preview/pr-228/api/dovetail/diagnosticextensions/methods.md#getconnectedtostepnames-type) : `IEnumerable<string>` `static` -- Gets the names of the steps that this joint is connected to
- [GetDiagnosticMetadata(DovetailContextBuilder, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-228/api/dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-dovetailcontextbuilder-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-228/api/dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
- [GetDiagnosticMetadata(IDovetailContext, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-228/api/dovetail/diagnosticextensions/methods.md#getdiagnosticmetadata-idovetailcontext-ienumerable-dovetailcategory) : [IEnumerable<DovetailDiagnosticMetadataJoint>](/Dovetail/pr-preview/pr-228/api/dovetail/dovetaildiagnosticmetadatajoint.md) `extension`
