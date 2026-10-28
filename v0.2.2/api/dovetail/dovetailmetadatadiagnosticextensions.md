# DovetailMetadataDiagnosticExtensions

- Kind: `class`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Diagnostics`
- Target framework: `net10.0`

Extension methods for [IDovetailJointMetadata](/Dovetail/v0.2.2/api/dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

## Definition

```csharp
namespace Dovetail.Diagnostics;

public static class DovetailMetadataDiagnosticExtensions
{
    // ...
}
```

## Methods

- [get_Assembly(IDovetailJointMetadata)](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-assembly-idovetailjointmetadata) : `Assembly` `static`
- [get_JointName(IDovetailJointMetadata)](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-jointname-idovetailjointmetadata) : `string` `static`
- [GetConnectedTo(IDovetailJointMetadata)](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedto-idovetailjointmetadata) : `IEnumerable<Type>` `extension`
- [GetConnectedToStepNames(IDovetailJointMetadata)](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedtostepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
- [GetStepNames(IDovetailJointMetadata)](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions/methods.md#getstepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
