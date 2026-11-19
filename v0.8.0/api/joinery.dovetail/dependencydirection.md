# DependencyDirection

- Kind: `enum`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/Infrastructure/DependencyDirection.cs)

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
