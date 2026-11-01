# IDovetailContext Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Type: [IDovetailContext](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/idovetailcontext.md)
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

The categories of joints the context loads.

```csharp
public abstract ImmutableHashSet<DovetailCategory> Categories { get; }
```

## Remarks

The `Core` joints are always loaded, and when no category was given the `Application` joints are loaded as well, so this always holds `Core`, and `Application` too in that case. A joint in any other category is loaded only when its category was given.

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `abstract` `get`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailhosttype.md)

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
