# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-464/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/pr-preview/pr-464/api/joinery.dovetail/dependencydirection.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
