# DovetailContextBuilderExtensions

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

Dovetail Context build extensions.

## Definition

```csharp
namespace Dovetail.Infrastructure;

public static class DovetailContextBuilderExtensions
{
    // ...
}
```

## Methods

- [CreateServiceProvider(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-240/api/dovetail/dovetailcontextbuilderextensions/methods.md#createserviceprovider-idovetailcontext-cancellationtoken) : `ValueTask<IServiceProvider>` `extension` -- Allows creation of a service provider from the convention context. This will apply configuration
