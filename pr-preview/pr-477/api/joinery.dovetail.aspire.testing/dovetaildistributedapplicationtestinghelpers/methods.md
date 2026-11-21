# DovetailDistributedApplicationTestingHelpers Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-477/api/joinery.dovetail.aspire.testing.md)
- Type: [DovetailDistributedApplicationTestingHelpers](/Dovetail/pr-preview/pr-477/api/joinery.dovetail.aspire.testing/dovetaildistributedapplicationtestinghelpers.md)
- Kind: `Methods`
- Members: `1`

Shared helpers used to wire an `Testing.IDistributedApplicationTestingBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-idistributedapplicationtestingbuilder-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(IDistributedApplicationTestingBuilder, DovetailContextBuilder, CancellationToken)

- Name: `Configure(IDistributedApplicationTestingBuilder, DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<DistributedApplication>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-477-merge.Sha.36056a013c51cd3b8c842fdef8810ae3835a17f4/src/Joinery.Dovetail.Aspire.Testing/RocketDistributedApplicationTestingExtensions.cs#L36-L58)

Populates the context, applies shared configuration, and builds the distributed application test host.

```csharp
public static class DovetailDistributedApplicationTestingHelpers
{
    public static ValueTask<DistributedApplication> Configure(
        IDistributedApplicationTestingBuilder builder,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IDistributedApplicationTestingBuilder`)
  The distributed application testing builder to configure.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<DistributedApplication>` -- The built and configured `Hosting.DistributedApplication` test host.

## Exceptions

- `ArgumentNullException` -- `builder` or `contextBuilder` is `null`.
