# Dovetail.Autofac

- Package: `Dovetail.Autofac`
- Version: `0.2.3-beta.3`
- Target framework: `net10.0`
- NuGet: [Dovetail.Autofac](https://www.nuget.org/packages/Dovetail.Autofac)

## Namespaces

### Dovetail.Autofac

- [AutofacJoint](/Dovetail/api/dovetail.autofac/autofacjoint.md) -- `class` -- A joint that registers Autofac services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

### Dovetail.Joints

- [AutofacAsyncJointDelegate](/Dovetail/api/dovetail.autofac/autofacasyncjointdelegate.md) -- `delegate` -- Async delegate for the Autofac part.
- [AutofacHostBuilderExtensions](/Dovetail/api/dovetail.autofac/autofachostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Autofac parts to a `DovetailContextBuilder`.
- [AutofacJointDelegate](/Dovetail/api/dovetail.autofac/autofacjointdelegate.md) -- `delegate` -- Delegate for the Autofac part.
- [AutofacJointExtensions](/Dovetail/api/dovetail.autofac/autofacjointextensions.md) -- `class` `static` -- Extension methods for applying Autofac parts.
- [IAutofacAsyncJoint](/Dovetail/api/dovetail.autofac/iautofacasyncjoint.md) -- `interface` -- Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
- [IAutofacJoint](/Dovetail/api/dovetail.autofac/iautofacjoint.md) -- `interface` -- Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`
