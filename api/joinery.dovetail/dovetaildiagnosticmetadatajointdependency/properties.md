# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/api/joinery.dovetail/dependencydirection.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJointDependency.cs)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
