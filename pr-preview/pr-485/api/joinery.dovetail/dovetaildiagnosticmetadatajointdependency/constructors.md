# DovetailDiagnosticMetadataJointDependency Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Constructors`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="constructor"></a>
<a id="constructor-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency-dependencydirection"></a>

## DovetailDiagnosticMetadataJointDependency(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)

- Name: `Constructor(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs#L25)

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public DovetailDiagnosticMetadataJointDependency(
        string JointName,
        string AssemblyName,
        ImmutableList<string> ImplementsJointTypes,
        ImmutableList<string> ConnectsToJointTypes,
        DovetailHostType HostType,
        string Category,
        ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies,
        DependencyDirection Direction)
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
- `HostType` ([DovetailHostType](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dovetailhosttype.md))
  The host type of the joint.
- `Category` (`string`)
  The category of the joint.
- `Dependencies` (`ImmutableList<DovetailDiagnosticMetadataJointDependency>`)
  The dependencies of the joint.
- `Direction` ([DependencyDirection](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/dependencydirection.md))
  The direction of the dependency.
