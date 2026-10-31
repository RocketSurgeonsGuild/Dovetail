# StashboxOptions

- Kind: `class`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.stashbox.md)
- Version: `0.3.1-pr.18`
- Namespace: `Joinery.Dovetail.Stashbox`
- Target framework: `net10.0`

Options for building the Stashbox Container

## Definition

```csharp
namespace Joinery.Dovetail.Stashbox;

public class StashboxOptions
{
    // ...
}
```

## Constructors

- [StashboxOptions](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.stashbox/stashboxoptions/constructors.md#constructor)

## Properties

- [ConfigureContainer](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.stashbox/stashboxoptions/properties.md#configurecontainer) : `Action<ContainerConfigurator>` `get; set` -- Configures the container (registration behavior, disposable transient tracking, etc.). Applied when the container is created and again once the service collection has been registered, so these settings win over the defaults set by the Microsoft.Extensions.DependencyInjection adapter.
- [ValidateLifetimesInDevelopment](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.stashbox/stashboxoptions/properties.md#validatelifetimesindevelopment) : `bool` `get; set` -- Enables lifetime validation when the environment is Development (defaults to true)
- [ValidateOnBuild](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.stashbox/stashboxoptions/properties.md#validateonbuild) : `bool` `get; set` -- Validates the whole container once the service provider has been built (defaults to false)
