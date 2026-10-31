# DovetailContext Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.md)
- Type: [DovetailContext](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/dovetailcontext.md)
- Kind: `Properties`
- Members: `3`

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/idovetailcontext.md)

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `get; set`
- Returns: `ImmutableHashSet<DovetailCategory>`

The categories of the convention context

```csharp
public ImmutableHashSet<DovetailCategory> Categories { get; set; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-224/api/joinery.dovetail/dovetailhosttype.md)

The host type

```csharp
public DovetailHostType HostType { get; }
```

<a id="joints"></a>

## Joints

- Name: `Joints`
- Modifiers: `get`
- Returns: `ImmutableList<IDovetailJoint>`

Get the conventions from the context

```csharp
public ImmutableList<IDovetailJoint> Joints { get; }
```
