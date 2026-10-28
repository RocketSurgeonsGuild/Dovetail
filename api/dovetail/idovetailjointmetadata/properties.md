# IDovetailJointMetadata Properties

- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Type: [IDovetailJointMetadata](/Dovetail/api/dovetail/idovetailjointmetadata.md)
- Kind: `Properties`
- Members: `4`

Declares a convention result with it's dependencies pre-computed

<a id="category"></a>

## Category

- Name: `Category`
- Modifiers: `abstract` `get`
- Returns: [DovetailCategory](/Dovetail/api/dovetail/dovetailcategory.md)

The category of the convention

```csharp
public abstract DovetailCategory Category { get; }
```

<a id="dependencies"></a>

## Dependencies

- Name: `Dependencies`
- Modifiers: `abstract` `get`
- Returns: [IReadOnlyCollection<IDovetailDependency>](/Dovetail/api/dovetail/idovetaildependency.md)

The dependencies

```csharp
public abstract IReadOnlyCollection<IDovetailDependency> Dependencies { get; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `abstract` `get`
- Returns: [DovetailHostType](/Dovetail/api/dovetail/dovetailhosttype.md)

The host type of the convention

```csharp
public abstract DovetailHostType HostType { get; }
```

<a id="joint"></a>

## Joint

- Name: `Joint`
- Modifiers: `abstract` `get`
- Returns: [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

The convention

```csharp
public abstract IDovetailJoint Joint { get; }
```
