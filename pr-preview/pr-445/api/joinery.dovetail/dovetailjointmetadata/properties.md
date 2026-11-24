# DovetailJointMetadata Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Properties`
- Members: `4`

Container for conventions

<a id="category"></a>

## Category

- Name: `Category`
- Modifiers: `get`
- Returns: [DovetailCategory](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailcategory.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L119)

The category of the convention

```csharp
public DovetailCategory Category { get; }
```

<a id="dependencies"></a>

## Dependencies

- Name: `Dependencies`
- Modifiers: `get; set`
- Returns: [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/idovetaildependency.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs)

The dependencies

```csharp
public IReadOnlyCollection<IDovetailDependency> Dependencies { get; set; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailhosttype.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L114)

The host type of the convention

```csharp
public DovetailHostType HostType { get; }
```

<a id="joint"></a>

## Joint

- Name: `Joint`
- Modifiers: `get`
- Returns: [IDovetailJoint](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/idovetailjoint.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L98)

The convention

```csharp
public IDovetailJoint Joint { get; }
```
