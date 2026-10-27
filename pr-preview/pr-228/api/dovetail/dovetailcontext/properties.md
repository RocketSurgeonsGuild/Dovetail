# DovetailContext Properties

- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Type: [DovetailContext](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcontext.md)
- Kind: `Properties`
- Members: `3`

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailcontext.md)

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
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailhosttype.md)

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
