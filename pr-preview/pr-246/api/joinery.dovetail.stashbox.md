# Joinery.Dovetail.Stashbox

- Package: `Joinery.Dovetail.Stashbox`
- Version: `0.2.4-pr.6`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Stashbox](https://www.nuget.org/packages/Joinery.Dovetail.Stashbox)

## Namespaces

### Joinery.Dovetail.Joints

- [IStashboxAsyncJoint](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/istashboxasyncjoint.md) -- `interface` -- Async part interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`
- [IStashboxJoint](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/istashboxjoint.md) -- `interface` -- Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`
- [StashboxAsyncJointDelegate](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate.md) -- `delegate` -- Async delegate for the Stashbox part.
- [StashboxHostBuilderExtensions](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Stashbox parts to a `Dovetail.DovetailContextBuilder`.
- [StashboxJointDelegate](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxjointdelegate.md) -- `delegate` -- Delegate for the Stashbox part.
- [StashboxJointExtensions](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxjointextensions.md) -- `class` `static` -- Extension methods for applying Stashbox parts.

### Joinery.Dovetail.Stashbox

- [StashboxJoint](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxjoint.md) -- `class` -- A joint that registers Stashbox services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [StashboxOptions](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxoptions.md) -- `class` -- Options for building the Stashbox Container
- [StashboxOptionsExtensions](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxoptionsextensions.md) -- `class` `static` -- Extension members for accessing [StashboxOptions](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.stashbox/stashboxoptions.md) from the convention context.
