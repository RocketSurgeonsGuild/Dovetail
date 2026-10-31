# DovetailContextBuilder Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.md)
- Type: [DovetailContextBuilder](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailcontextbuilder.md)
- Kind: `Properties`
- Members: `1`

Builder that can be used to create a context.

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `get`
- Returns: `ImmutableHashSet<DovetailCategory>`

The categories the context is limited to, as given to `Create`.

```csharp
public ImmutableHashSet<DovetailCategory> Categories { get; }
```

## Remarks

Only the joints whose category is in this set are loaded, and an empty set loads every joint. A set that names only categories no joint has loads no joint.
