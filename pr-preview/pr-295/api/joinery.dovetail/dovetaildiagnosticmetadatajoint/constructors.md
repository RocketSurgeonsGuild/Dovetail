# DovetailDiagnosticMetadataJoint Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md)
- Kind: `Constructors`
- Members: `1`

Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

<a id="constructor"></a>
<a id="constructor-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency"></a>

## DovetailDiagnosticMetadataJoint(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)

- Name: `Constructor(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)`

Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public DovetailDiagnosticMetadataJoint(
        string JointName,
        string AssemblyName,
        ImmutableList<string> ImplementsJointTypes,
        ImmutableList<string> ConnectsToJointTypes,
        DovetailHostType HostType,
        string Category,
        ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies)
    {
        // ...
    }
}
```

## Parameters

- `JointName` (`string`)
  The name of the joint.
- `AssemblyName` (`string`)
  The assembly name of the joint.
- `ImplementsJointTypes` (`ImmutableList<string>`)
  The types of the joint.
- `ConnectsToJointTypes` (`ImmutableList<string>`)
  The types of the joint that this joint connects to.
- `HostType` ([DovetailHostType](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailhosttype.md))
  The host type of the joint.
- `Category` (`string`)
  The category of the joint.
- `Dependencies` (`ImmutableList<DovetailDiagnosticMetadataJointDependency>`)
  The dependencies of the joint.
