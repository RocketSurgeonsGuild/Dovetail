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
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.6690600a1d8dc3e5d18fb6e69136cd6b75569d71/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Configuration.g.cs#L51-L52)

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
