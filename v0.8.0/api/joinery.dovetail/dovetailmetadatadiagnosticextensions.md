# DovetailMetadataDiagnosticExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs)

Extension methods for [IDovetailJointMetadata](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public static class DovetailMetadataDiagnosticExtensions
{
    // ...
}
```

## Methods

- [get_Assembly(IDovetailJointMetadata)](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-assembly-idovetailjointmetadata) : `Assembly` `static`
- [get_JointName(IDovetailJointMetadata)](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#get-jointname-idovetailjointmetadata) : `string` `static`
- [GetConnectedTo(IDovetailJointMetadata)](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedto-idovetailjointmetadata) : `IEnumerable<Type>` `extension`
- [GetConnectedToStepNames(IDovetailJointMetadata)](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getconnectedtostepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
- [GetStepNames(IDovetailJointMetadata)](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailmetadatadiagnosticextensions/methods.md#getstepnames-idovetailjointmetadata) : `IEnumerable<string>` `extension`
