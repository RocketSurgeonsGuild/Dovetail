# IDovetailJointDelegate Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.md)
- Type: [IDovetailJointDelegate](/Dovetail/pr-preview/pr-246/api/joinery.dovetail/idovetailjointdelegate.md)
- Kind: `Properties`
- Members: `2`

A marker interface to indicate a type is a joint delegate

<a id="assembly"></a>

## Assembly

- Name: `Assembly`
- Modifiers: `abstract` `get`
- Returns: `Assembly`

The assembly of the joint delegate

```csharp
public abstract Assembly Assembly { get; }
```

<a id="expression"></a>

## Expression

- Name: `Expression`
- Modifiers: `abstract` `get`
- Returns: `string`

The expression of the joint delegate

```csharp
public abstract string Expression { get; }
```
