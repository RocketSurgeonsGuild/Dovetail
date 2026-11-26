# DependencyDirection

- Kind: `enum`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail/Infrastructure/DependencyDirection.cs)

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
