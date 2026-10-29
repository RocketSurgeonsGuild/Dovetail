# IDovetailDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-243/api/joinery.dovetail.md)
- Type: [IDovetailDependency](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/idovetaildependency.md)
- Kind: `Properties`
- Members: `2`

A dependency for a given convention

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `abstract` `get`
- Returns: [DependencyDirection](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/dependencydirection.md)

The direction

```csharp
public abstract DependencyDirection Direction { get; }
```

<a id="type"></a>

## Type

- Name: `Type`
- Modifiers: `abstract` `get`
- Returns: `Type`

The type

```csharp
public abstract Type Type { get; }
```
