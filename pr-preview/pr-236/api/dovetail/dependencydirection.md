# DependencyDirection

- Kind: `enum`
- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

Defines the dependency direction of the given type

## Definition

```csharp
namespace Dovetail.Infrastructure;

public enum DependencyDirection
```

## Enum Members

| Name | Value | Description |
| --- | --- | --- |
| DependsOn | 0 | Order the convention to be run before this one. |
| DependentOf | 1 | Order the convention to be run after this one. |
