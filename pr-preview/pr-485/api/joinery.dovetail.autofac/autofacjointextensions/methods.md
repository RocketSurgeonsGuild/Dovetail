# AutofacJointExtensions Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.autofac.md)
- Type: [AutofacJointExtensions](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.autofac/autofacjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Autofac parts.

<a id="applyautofac"></a>
<a id="applyautofac-containerbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyAutofac(ContainerBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyAutofac(ContainerBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/DovetailJoint.Autofac.g.cs#L51-L52)

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
