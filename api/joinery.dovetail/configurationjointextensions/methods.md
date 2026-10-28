# ConfigurationJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [ConfigurationJointExtensions](/Dovetail/api/joinery.dovetail/configurationjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Configuration parts.

<a id="applyconfiguration"></a>
<a id="applyconfiguration-iconfigurationbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyConfiguration(IConfigurationBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyConfiguration(IConfigurationBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered Configuration parts.

```csharp
public static class ConfigurationJointExtensions
{
    public static ValueTask ApplyConfiguration(
        this IConfigurationBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IConfigurationBuilder`)
- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
