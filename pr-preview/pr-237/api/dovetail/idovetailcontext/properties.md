# IDovetailContext Properties

- Package: [Dovetail](/Dovetail/pr-preview/pr-237/api/dovetail.md)
- Type: [IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md)
- Kind: `Properties`
- Members: `4`

The base context marker interface to define this as a context

<a id="assembly"></a>

## Assembly

- Name: `Assembly`
- Modifiers: `virtual` `get`
- Returns: `Assembly`

The assembly that is executing the conventions

```csharp
public virtual Assembly Assembly { get; }
```

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `abstract` `get`
- Returns: `ImmutableHashSet<DovetailCategory>`

The categories of the convention context

```csharp
public abstract ImmutableHashSet<DovetailCategory> Categories { get; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `abstract` `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md)

The underlying host type

```csharp
public abstract DovetailHostType HostType { get; }
```

<a id="joints"></a>

## Joints

- Name: `Joints`
- Modifiers: `abstract` `get`
- Returns: `ImmutableList<IDovetailJoint>`

Get the conventions from the context

```csharp
public abstract ImmutableList<IDovetailJoint> Joints { get; }
```
