# DovetailContextBuilderExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/Infrastructure/DovetailContextBuilderExtensions.cs)

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

- [CreateServiceProvider(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/dovetailcontextbuilderextensions/methods.md#createserviceprovider-idovetailcontext-cancellationtoken) : `ValueTask<IServiceProvider>` `extension` -- Allows creation of a service provider from the convention context. This will apply configuration
