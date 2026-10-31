# DovetailJointMetadata Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-281/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Properties`
- Members: `4`

Container for conventions

<a id="category"></a>

## Category

- Name: `Category`
- Modifiers: `get`
- Returns: [DovetailCategory](/Dovetail/pr-preview/pr-281/api/joinery.dovetail/dovetailcategory.md)

The category of the convention

```csharp
public DovetailCategory Category { get; }
```

<a id="dependencies"></a>

## Dependencies

- Name: `Dependencies`
- Modifiers: `get; set`
- Returns: [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-281/api/joinery.dovetail/idovetaildependency.md)

The dependencies

```csharp
public IReadOnlyCollection<IDovetailDependency> Dependencies { get; set; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-281/api/joinery.dovetail/dovetailhosttype.md)

The host type of the convention

```csharp
public DovetailHostType HostType { get; }
```

<a id="joint"></a>

## Joint

- Name: `Joint`
- Modifiers: `get`
- Returns: [IDovetailJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail/idovetailjoint.md)

The convention

```csharp
public IDovetailJoint Joint { get; }
```
