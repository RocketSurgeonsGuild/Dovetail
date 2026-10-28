# DovetailDiagnosticMetadataJointDependency Properties

- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/api/dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Properties`
- Members: `1`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `get; init`
- Returns: [DependencyDirection](/Dovetail/api/dovetail/dependencydirection.md)

The direction of the dependency.

```csharp
public DependencyDirection Direction { get; init; }
```
