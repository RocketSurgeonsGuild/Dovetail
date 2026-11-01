# DovetailContextBuilderExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Type: [DovetailContextBuilderExtensions](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilderextensions.md)
- Kind: `Methods`
- Members: `1`

Dovetail Context build extensions.

<a id="createserviceprovider"></a>
<a id="createserviceprovider-idovetailcontext-cancellationtoken"></a>

## CreateServiceProvider(IDovetailContext, CancellationToken)

- Name: `CreateServiceProvider(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<IServiceProvider>`

Allows creation of a service provider from the convention context. This will apply configuration

```csharp
public static class DovetailContextBuilderExtensions
{
    public static ValueTask<IServiceProvider> CreateServiceProvider(
        this IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/idovetailcontext.md))
  The context to build a service provider from.
- `cancellationToken` (`CancellationToken`) `optional`
  The cancellation token used while applying configuration, services, and logging.

## Returns

`ValueTask<IServiceProvider>` -- The `IServiceProvider` built from the context.
