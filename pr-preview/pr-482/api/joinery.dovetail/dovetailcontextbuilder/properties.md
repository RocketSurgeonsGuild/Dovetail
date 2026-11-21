# DovetailContextBuilder Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Type: [DovetailContextBuilder](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/dovetailcontextbuilder.md)
- Kind: `Properties`
- Members: `1`

Builder that can be used to create a context.

<a id="categories"></a>

## Categories

- Name: `Categories`
- Modifiers: `get`
- Returns: `ImmutableHashSet<DovetailCategory>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/DovetailContextBuilder.cs#L73)

The categories of joints the context loads.

```csharp
public ImmutableHashSet<DovetailCategory> Categories { get; }
```

## Remarks

The `Core` joints are always loaded, and when no category was given to `Create` the `Application` joints are loaded as well, so this always holds `Core`, and `Application` too in that case. A joint in any other category is loaded only when its category was given.
