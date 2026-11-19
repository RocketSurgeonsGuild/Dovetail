# Joinery.Dovetail.Autofac

- Package: `Joinery.Dovetail.Autofac`
- Version: `0.7.2-beta.15`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/tree/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail.Autofac)
- NuGet: [Joinery.Dovetail.Autofac](https://www.nuget.org/packages/Joinery.Dovetail.Autofac)

## Namespaces

### Joinery.Dovetail.Autofac

- [AutofacJoint](/Dovetail/api/joinery.dovetail.autofac/autofacjoint.md) -- `class` -- A joint that registers Autofac services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

### Joinery.Dovetail.Joints

- [AutofacAsyncJointDelegate](/Dovetail/api/joinery.dovetail.autofac/autofacasyncjointdelegate.md) -- `delegate` -- Async delegate for the Autofac part.
- [AutofacHostBuilderExtensions](/Dovetail/api/joinery.dovetail.autofac/autofachostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Autofac parts to a `Dovetail.DovetailContextBuilder`.
- [AutofacJointDelegate](/Dovetail/api/joinery.dovetail.autofac/autofacjointdelegate.md) -- `delegate` -- Delegate for the Autofac part.
- [AutofacJointExtensions](/Dovetail/api/joinery.dovetail.autofac/autofacjointextensions.md) -- `class` `static` -- Extension methods for applying Autofac parts.
- [IAutofacAsyncJoint](/Dovetail/api/joinery.dovetail.autofac/iautofacasyncjoint.md) -- `interface` -- Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
- [IAutofacJoint](/Dovetail/api/joinery.dovetail.autofac/iautofacjoint.md) -- `interface` -- Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
