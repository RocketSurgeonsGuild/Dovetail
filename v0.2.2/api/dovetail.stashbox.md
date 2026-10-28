# Dovetail.Stashbox

- Package: `Dovetail.Stashbox`
- Version: `0.2.2`
- Target framework: `net10.0`
- NuGet: [Dovetail.Stashbox](https://www.nuget.org/packages/Dovetail.Stashbox)

## Namespaces

### Dovetail.Joints

- [IStashboxAsyncJoint](/Dovetail/v0.2.2/api/dovetail.stashbox/istashboxasyncjoint.md) -- `interface` -- Async part interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`
- [IStashboxJoint](/Dovetail/v0.2.2/api/dovetail.stashbox/istashboxjoint.md) -- `interface` -- Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`
- [StashboxAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxasyncjointdelegate.md) -- `delegate` -- Async delegate for the Stashbox part.
- [StashboxHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Stashbox parts to a `DovetailContextBuilder`.
- [StashboxJointDelegate](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointdelegate.md) -- `delegate` -- Delegate for the Stashbox part.
- [StashboxJointExtensions](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointextensions.md) -- `class` `static` -- Extension methods for applying Stashbox parts.

### Dovetail.Stashbox

- [StashboxJoint](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjoint.md) -- `class` -- A joint that registers Stashbox services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [StashboxOptions](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxoptions.md) -- `class` -- Options for building the Stashbox Container
- [StashboxOptionsExtensions](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxoptionsextensions.md) -- `class` `static` -- Extension members for accessing [StashboxOptions](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxoptions.md) from the convention context.
