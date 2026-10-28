# Dovetail.Hosting.Maui

- Package: `Dovetail.Hosting.Maui`
- Version: `0.2.2-pr.3`
- Target framework: `net10.0`
- NuGet: [Dovetail.Hosting.Maui](https://www.nuget.org/packages/Dovetail.Hosting.Maui)

## Namespaces

### Dovetail

- [DovetailMauiHelpers](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/dovetailmauihelpers.md) -- `class` `static` -- Shared helpers used to wire a `Hosting.MauiAppBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Dovetail.Joints

- [IMauiAppAsyncJoint](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/imauiappasyncjoint.md) -- `interface` -- Async part interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppBuilderAsyncJoint](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/imauiappbuilderasyncjoint.md) -- `interface` -- Async part interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppBuilderJoint](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/imauiappbuilderjoint.md) -- `interface` -- Joint interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppJoint](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/imauiappjoint.md) -- `interface` -- Joint interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`
- [MauiAppAsyncJointDelegate](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappasyncjointdelegate.md) -- `delegate` -- Async delegate for the MauiApp part.
- [MauiAppBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the MauiAppBuilder part.
- [MauiAppBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying MauiAppBuilder parts to a `DovetailContextBuilder`.
- [MauiAppBuilderJointDelegate](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappbuilderjointdelegate.md) -- `delegate` -- Delegate for the MauiAppBuilder part.
- [MauiAppBuilderJointExtensions](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying MauiAppBuilder parts.
- [MauiAppHostBuilderExtensions](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiapphostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying MauiApp parts to a `DovetailContextBuilder`.
- [MauiAppJointDelegate](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappjointdelegate.md) -- `delegate` -- Delegate for the MauiApp part.
- [MauiAppJointExtensions](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.maui/mauiappjointextensions.md) -- `class` `static` -- Extension methods for applying MauiApp parts.
