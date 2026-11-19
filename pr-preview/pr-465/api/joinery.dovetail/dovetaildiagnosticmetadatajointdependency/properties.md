# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/dependencydirection.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
