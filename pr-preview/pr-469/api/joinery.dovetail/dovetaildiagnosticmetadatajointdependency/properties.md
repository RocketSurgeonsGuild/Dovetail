# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dependencydirection.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
