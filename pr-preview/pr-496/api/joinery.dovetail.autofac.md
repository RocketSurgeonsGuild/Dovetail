# Joinery.Dovetail.Autofac

- Package: `Joinery.Dovetail.Autofac`
- Version: `0.8.2-pr.19`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/tree/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail.Autofac)
- NuGet: [Joinery.Dovetail.Autofac](https://www.nuget.org/packages/Joinery.Dovetail.Autofac)

## Namespaces

### Joinery.Dovetail.Autofac

- [AutofacJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofacjoint.md) -- `class` -- A joint that registers Autofac services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

### Joinery.Dovetail.Joints

- [AutofacAsyncJointDelegate](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofacasyncjointdelegate.md) -- `delegate` -- Async delegate for the Autofac part.
- [AutofacHostBuilderExtensions](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofachostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Autofac parts to a `Dovetail.DovetailContextBuilder`.
- [AutofacJointDelegate](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofacjointdelegate.md) -- `delegate` -- Delegate for the Autofac part.
- [AutofacJointExtensions](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/autofacjointextensions.md) -- `class` `static` -- Extension methods for applying Autofac parts.
- [IAutofacAsyncJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/iautofacasyncjoint.md) -- `interface` -- Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
- [IAutofacJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.autofac/iautofacjoint.md) -- `interface` -- Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
