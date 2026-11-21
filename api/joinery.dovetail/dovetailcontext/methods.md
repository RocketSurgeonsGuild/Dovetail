# DovetailContext Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailContext](/Dovetail/api/joinery.dovetail/dovetailcontext.md)
- Kind: `Methods`
- Members: `1`

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)

<a id="fromasync"></a>
<a id="fromasync-dovetailcontextbuilder-cancellationtoken"></a>

## FromAsync(DovetailContextBuilder, CancellationToken)

- Name: `FromAsync(DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<IDovetailContext>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.65fca43d4a7154e28dbe9531d628c844781169f5/src/Joinery.Dovetail/DovetailContext.cs#L24-L25)

Create a context from a given builder

```csharp
public sealed class DovetailContext
{
    public static ValueTask<IDovetailContext> FromAsync(
        DovetailContextBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md))
  The builder to create the context from.
- `cancellationToken` (`CancellationToken`) `optional`
  The cancellation token used during setup.

## Returns

`ValueTask<IDovetailContext>` -- The newly created and set up [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md).

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
