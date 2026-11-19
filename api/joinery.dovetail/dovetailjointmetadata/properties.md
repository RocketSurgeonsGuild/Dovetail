# DovetailJointMetadata Properties

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Properties`
- Members: `4`

Container for conventions

<a id="category"></a>

## Category

- Name: `Category`
- Modifiers: `get`
- Returns: [DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L119)

The category of the convention

```csharp
public DovetailCategory Category { get; }
```

<a id="dependencies"></a>

## Dependencies

- Name: `Dependencies`
- Modifiers: `get; set`
- Returns: [IReadOnlyCollection<IDovetailDependency>](/Dovetail/api/joinery.dovetail/idovetaildependency.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs)

The dependencies

```csharp
public IReadOnlyCollection<IDovetailDependency> Dependencies { get; set; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get`
- Returns: [DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L114)

The host type of the convention

```csharp
public DovetailHostType HostType { get; }
```

<a id="joint"></a>

## Joint

- Name: `Joint`
- Modifiers: `get`
- Returns: [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L98)

The convention

```csharp
public IDovetailJoint Joint { get; }
```
