# DependencyDirection

- Kind: `enum`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail/Infrastructure/DependencyDirection.cs)

Defines the dependency direction of the given type

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public enum DependencyDirection
```

## Enum Members

| Name | Value | Description |
| --- | --- | --- |
| DependsOn | 0 | Order the convention to be run before this one. |
| DependentOf | 1 | Order the convention to be run after this one. |
