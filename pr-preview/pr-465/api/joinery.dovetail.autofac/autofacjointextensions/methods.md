# AutofacJointExtensions Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.autofac.md)
- Type: [AutofacJointExtensions](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.autofac/autofacjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Autofac parts.

<a id="applyautofac"></a>
<a id="applyautofac-containerbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyAutofac(ContainerBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyAutofac(ContainerBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/DovetailJoint.Autofac.g.cs#L51-L52)

Applies the registered Autofac parts.

```csharp
public static class AutofacJointExtensions
{
    public static ValueTask ApplyAutofac(
        this ContainerBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`ContainerBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
