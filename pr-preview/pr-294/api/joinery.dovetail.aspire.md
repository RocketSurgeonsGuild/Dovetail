# Joinery.Dovetail.Aspire

- Package: `Joinery.Dovetail.Aspire`
- Version: `0.4.1-pr.4`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Aspire](https://www.nuget.org/packages/Joinery.Dovetail.Aspire)

## Namespaces

### Joinery.Dovetail

- [DovetailDistributedApplicationHelpers](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/dovetaildistributedapplicationhelpers.md) -- `class` `static` -- Shared helpers used to wire an `Hosting.IDistributedApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Joinery.Dovetail.Joints

- [DistributedApplicationAsyncJointDelegate](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationasyncjointdelegate.md) -- `delegate` -- Async delegate for the DistributedApplication part.
- [DistributedApplicationBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the DistributedApplicationBuilder part.
- [DistributedApplicationBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying DistributedApplicationBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [DistributedApplicationBuilderJointDelegate](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationbuilderjointdelegate.md) -- `delegate` -- Delegate for the DistributedApplicationBuilder part.
- [DistributedApplicationBuilderJointExtensions](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying DistributedApplicationBuilder parts.
- [DistributedApplicationHostBuilderExtensions](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying DistributedApplication parts to a `Dovetail.DovetailContextBuilder`.
- [DistributedApplicationJointDelegate](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationjointdelegate.md) -- `delegate` -- Delegate for the DistributedApplication part.
- [DistributedApplicationJointExtensions](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/distributedapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying DistributedApplication parts.
- [IDistributedApplicationAsyncJoint](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/idistributedapplicationasyncjoint.md) -- `interface` -- Async part interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationBuilderAsyncJoint](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/idistributedapplicationbuilderasyncjoint.md) -- `interface` -- Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationBuilderJoint](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/idistributedapplicationbuilderjoint.md) -- `interface` -- Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationJoint](/Dovetail/pr-preview/pr-294/api/joinery.dovetail.aspire/idistributedapplicationjoint.md) -- `interface` -- Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`
