# DovetailDiagnosticMetadataJointDependency Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Constructors`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="constructor"></a>
<a id="constructor-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency-dependencydirection"></a>

## DovetailDiagnosticMetadataJointDependency(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)

- Name: `Constructor(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs#L25)

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
- `HostType` ([DovetailHostType](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dovetailhosttype.md))
  The host type of the joint.
- `Category` (`string`)
  The category of the joint.
- `Dependencies` (`ImmutableList<DovetailDiagnosticMetadataJointDependency>`)
  The dependencies of the joint.
- `Direction` ([DependencyDirection](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dependencydirection.md))
  The direction of the dependency.
