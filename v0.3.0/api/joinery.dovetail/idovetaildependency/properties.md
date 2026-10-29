# IDovetailDependency Properties

- Package: [Joinery.Dovetail](/Dovetail/v0.3.0/api/joinery.dovetail.md)
- Type: [IDovetailDependency](/Dovetail/v0.3.0/api/joinery.dovetail/idovetaildependency.md)
- Kind: `Properties`
- Members: `2`

A dependency for a given convention

<a id="direction"></a>

## Direction

- Name: `Direction`
- Modifiers: `abstract` `get`
- Returns: [DependencyDirection](/Dovetail/v0.3.0/api/joinery.dovetail/dependencydirection.md)

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
