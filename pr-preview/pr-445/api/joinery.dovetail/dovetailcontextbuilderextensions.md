# DovetailContextBuilderExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/Infrastructure/DovetailContextBuilderExtensions.cs)

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

- [CreateServiceProvider(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailcontextbuilderextensions/methods.md#createserviceprovider-idovetailcontext-cancellationtoken) : `ValueTask<IServiceProvider>` `extension` -- Allows creation of a service provider from the convention context. This will apply configuration
