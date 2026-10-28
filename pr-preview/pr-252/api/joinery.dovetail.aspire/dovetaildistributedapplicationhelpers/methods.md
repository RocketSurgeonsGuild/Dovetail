# DovetailDistributedApplicationHelpers Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire.md)
- Type: [DovetailDistributedApplicationHelpers](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.aspire/dovetaildistributedapplicationhelpers.md)
- Kind: `Methods`
- Members: `1`

Shared helpers used to wire an `Hosting.IDistributedApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-idistributedapplicationbuilder-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)

- Name: `Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<DistributedApplication>`

Populates the context, applies shared configuration, and builds the distributed application host.

```csharp
public static class DovetailDistributedApplicationHelpers
{
    public static ValueTask<DistributedApplication> Configure(
        IDistributedApplicationBuilder builder,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IDistributedApplicationBuilder`)
  The distributed application builder to configure.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<DistributedApplication>` -- The built and configured `Hosting.DistributedApplication`.

## Exceptions

- `ArgumentNullException` -- `builder` or `contextBuilder` is `null`.
