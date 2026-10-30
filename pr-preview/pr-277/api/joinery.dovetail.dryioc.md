# Joinery.Dovetail.DryIoc

- Package: `Joinery.Dovetail.DryIoc`
- Version: `0.3.1-pr.10`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.DryIoc](https://www.nuget.org/packages/Joinery.Dovetail.DryIoc)

## Namespaces

### Joinery.Dovetail.DryIoc

- [DryIocJoint](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocjoint.md) -- `class` -- A joint that registers DryIoc services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [DryIocOptions](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocoptions.md) -- `class` -- Options for building the DryIoc Container
- [DryIocOptionsExtensions](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocoptionsextensions.md) -- `class` `static` -- Extension members for accessing [DryIocOptions](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocoptions.md) from the convention context.

### Joinery.Dovetail.Joints

- [DryIocAsyncJointDelegate](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate.md) -- `delegate` -- Async delegate for the DryIoc part.
- [DryIocHostBuilderExtensions](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiochostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying DryIoc parts to a `Dovetail.DovetailContextBuilder`.
- [DryIocJointDelegate](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocjointdelegate.md) -- `delegate` -- Delegate for the DryIoc part.
- [DryIocJointExtensions](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/dryiocjointextensions.md) -- `class` `static` -- Extension methods for applying DryIoc parts.
- [IDryIocAsyncJoint](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/idryiocasyncjoint.md) -- `interface` -- Async part interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`
- [IDryIocJoint](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.dryioc/idryiocjoint.md) -- `interface` -- Joint interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`
