# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/v0.2.2/api/dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/v0.2.2/api/dovetail/dependencydirection.md)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
