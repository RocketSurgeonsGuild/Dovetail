# DovetailContextBuilderExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.md)
- Version: `0.7.2-pr.11`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.a1990fe6695c5fa9e9c003a3a333f8ff361aa602/src/Joinery.Dovetail/Infrastructure/DovetailContextBuilderExtensions.cs)

Dovetail Context build extensions.

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public static class DovetailContextBuilderExtensions
{
    // ...
}
```

## Methods

- [CreateServiceProvider(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailcontextbuilderextensions/methods.md#createserviceprovider-idovetailcontext-cancellationtoken) : `ValueTask<IServiceProvider>` `extension` -- Allows creation of a service provider from the convention context. This will apply configuration
