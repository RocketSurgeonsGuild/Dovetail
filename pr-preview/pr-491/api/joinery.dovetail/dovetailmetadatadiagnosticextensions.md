# DovetailMetadataDiagnosticExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs)

Extension methods for [IDovetailJointMetadata](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public static class DovetailMetadataDiagnosticExtensions
{
    // ...
}
```

## Methods

- [get_Assembly(IDovetailJointMetadata)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-assembly-idovetailjointmetadata) : `Assembly` `static`
- [get_JointName(IDovetailJointMetadata)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-jointname-idovetailjointmetadata) : `string` `static`
- [GetConnectedTo(IDovetailJointMetadata)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedto-idovetailjointmetadata) : `IEnumerable<Type>` `extension`
- [GetConnectedToStepNames(IDovetailJointMetadata)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedtostepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
- [GetStepNames(IDovetailJointMetadata)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getstepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
