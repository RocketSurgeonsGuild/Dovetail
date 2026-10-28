# UnoApplicationBuilderJointExtensions Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno.md)
- Type: [UnoApplicationBuilderJointExtensions](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno/unoapplicationbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying UnoApplicationBuilder parts.

<a id="applyunoapplicationbuilder"></a>
<a id="applyunoapplicationbuilder-iapplicationbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyUnoApplicationBuilder(IApplicationBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyUnoApplicationBuilder(IApplicationBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered UnoApplicationBuilder parts.

```csharp
public static class UnoApplicationBuilderJointExtensions
{
    public static ValueTask ApplyUnoApplicationBuilder(
        this IApplicationBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IApplicationBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
