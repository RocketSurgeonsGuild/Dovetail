# DovetailMetadataDiagnosticExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.23`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs)

Extension methods for [IDovetailJointMetadata](/Dovetail/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public static class DovetailMetadataDiagnosticExtensions
{
    // ...
}
```

## Methods

- [get_Assembly(IDovetailJointMetadata)](/Dovetail/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-assembly-idovetailjointmetadata) : `Assembly` `static`
- [get_JointName(IDovetailJointMetadata)](/Dovetail/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-jointname-idovetailjointmetadata) : `string` `static`
- [GetConnectedTo(IDovetailJointMetadata)](/Dovetail/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedto-idovetailjointmetadata) : `IEnumerable<Type>` `extension`
- [GetConnectedToStepNames(IDovetailJointMetadata)](/Dovetail/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedtostepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
- [GetStepNames(IDovetailJointMetadata)](/Dovetail/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getstepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
