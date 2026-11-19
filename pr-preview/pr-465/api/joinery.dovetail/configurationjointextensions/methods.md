# ConfigurationJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Type: [ConfigurationJointExtensions](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/configurationjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Configuration parts.

<a id="applyconfiguration"></a>
<a id="applyconfiguration-iconfigurationbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyConfiguration(IConfigurationBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyConfiguration(IConfigurationBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Configuration.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
