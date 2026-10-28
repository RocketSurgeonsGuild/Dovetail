# DovetailContext Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-244/api/dovetail.md)
- Type: [DovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/dovetailcontext.md)
- Kind: `Methods`
- Members: `1`

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md)

<a id="fromasync"></a>
<a id="fromasync-dovetailcontextbuilder-cancellationtoken"></a>

## FromAsync(DovetailContextBuilder, CancellationToken)

- Name: `FromAsync(DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<IDovetailContext>`

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

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-244/api/dovetail/dovetailcontextbuilder.md))
  The builder to create the context from.
- `cancellationToken` (`CancellationToken`) `optional`
  The cancellation token used during setup.

## Returns

`ValueTask<IDovetailContext>` -- The newly created and set up [IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md).

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
