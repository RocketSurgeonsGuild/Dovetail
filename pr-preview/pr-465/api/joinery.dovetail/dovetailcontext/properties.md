# DovetailContext Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Type: [DovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/dovetailcontext.md)
- Kind: `Properties`
- Members: `3`

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailcontext.md)

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `get; set`
- Returns: `ImmutableHashSet<DovetailCategory>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/DovetailContext.cs)

The categories of joints the context loads.

```csharp
public ImmutableHashSet<DovetailCategory> Categories { get; set; }
```

## Remarks

The `Core` joints are always loaded, and when no category was given the `Application` joints are loaded as well, so this always holds `Core`, and `Application` too in that case. A joint in any other category is loaded only when its category was given.

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/dovetailhosttype.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/DovetailContext.cs#L49)

The host type

```csharp
public DovetailHostType HostType { get; }
```

<a id="joints"></a>

## Joints

- Name: `Joints`
- Modifiers: `get`
- Returns: `ImmutableList<IDovetailJoint>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/DovetailContext.cs#L42)

Get the conventions from the context

```csharp
public ImmutableList<IDovetailJoint> Joints { get; }
```
