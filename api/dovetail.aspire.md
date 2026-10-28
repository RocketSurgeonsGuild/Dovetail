# Dovetail.Aspire

- Package: `Dovetail.Aspire`
- Version: `0.2.1-beta.8`
- Target framework: `net10.0`
- NuGet: [Dovetail.Aspire](https://www.nuget.org/packages/Dovetail.Aspire)

## Namespaces

### Dovetail

- [DovetailDistributedApplicationHelpers](/Dovetail/api/dovetail.aspire/dovetaildistributedapplicationhelpers.md) -- `class` `static` -- Shared helpers used to wire an `Hosting.IDistributedApplicationBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Dovetail.Joints

- [DistributedApplicationAsyncJointDelegate](/Dovetail/api/dovetail.aspire/distributedapplicationasyncjointdelegate.md) -- `delegate` -- Async delegate for the DistributedApplication part.
- [DistributedApplicationBuilderAsyncJointDelegate](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the DistributedApplicationBuilder part.
- [DistributedApplicationBuilderHostBuilderExtensions](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying DistributedApplicationBuilder parts to a `DovetailContextBuilder`.
- [DistributedApplicationBuilderJointDelegate](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderjointdelegate.md) -- `delegate` -- Delegate for the DistributedApplicationBuilder part.
- [DistributedApplicationBuilderJointExtensions](/Dovetail/api/dovetail.aspire/distributedapplicationbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying DistributedApplicationBuilder parts.
- [DistributedApplicationHostBuilderExtensions](/Dovetail/api/dovetail.aspire/distributedapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying DistributedApplication parts to a `DovetailContextBuilder`.
- [DistributedApplicationJointDelegate](/Dovetail/api/dovetail.aspire/distributedapplicationjointdelegate.md) -- `delegate` -- Delegate for the DistributedApplication part.
- [DistributedApplicationJointExtensions](/Dovetail/api/dovetail.aspire/distributedapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying DistributedApplication parts.
- [IDistributedApplicationAsyncJoint](/Dovetail/api/dovetail.aspire/idistributedapplicationasyncjoint.md) -- `interface` -- Async part interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationBuilderAsyncJoint](/Dovetail/api/dovetail.aspire/idistributedapplicationbuilderasyncjoint.md) -- `interface` -- Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationBuilderJoint](/Dovetail/api/dovetail.aspire/idistributedapplicationbuilderjoint.md) -- `interface` -- Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IDistributedApplicationJoint](/Dovetail/api/dovetail.aspire/idistributedapplicationjoint.md) -- `interface` -- Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`
