# UnoHostBuilderJointExtensions Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.uno.md)
- Type: [UnoHostBuilderJointExtensions](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.uno/unohostbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying UnoHostBuilder parts.

<a id="applyunohostbuilder"></a>
<a id="applyunohostbuilder-ihostbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyUnoHostBuilder(IHostBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyUnoHostBuilder(IHostBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered UnoHostBuilder parts.

```csharp
public static class UnoHostBuilderJointExtensions
{
    public static ValueTask ApplyUnoHostBuilder(
        this IHostBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IHostBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
