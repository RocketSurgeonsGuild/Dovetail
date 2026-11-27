# DovetailContextBuilder Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.md)
- Type: [DovetailContextBuilder](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailcontextbuilder.md)
- Kind: `Properties`
- Members: `1`

Builder that can be used to create a context.

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `get`
- Returns: `ImmutableHashSet<DovetailCategory>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail/DovetailContextBuilder.cs#L73)

The categories of joints the context loads.

```csharp
public ImmutableHashSet<DovetailCategory> Categories { get; }
```

## Remarks

The `Core` joints are always loaded, and when no category was given to `Create` the `Application` joints are loaded as well, so this always holds `Core`, and `Application` too in that case. A joint in any other category is loaded only when its category was given.
