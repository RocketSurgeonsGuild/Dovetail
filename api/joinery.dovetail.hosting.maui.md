# Joinery.Dovetail.Hosting.Maui

- Package: `Joinery.Dovetail.Hosting.Maui`
- Version: `0.2.3-beta.4`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Hosting.Maui](https://www.nuget.org/packages/Joinery.Dovetail.Hosting.Maui)

## Namespaces

### Joinery.Dovetail

- [DovetailMauiHelpers](/Dovetail/api/joinery.dovetail.hosting.maui/dovetailmauihelpers.md) -- `class` `static` -- Shared helpers used to wire a `Hosting.MauiAppBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Joinery.Dovetail.Joints

- [IMauiAppAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappasyncjoint.md) -- `interface` -- Async part interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppBuilderAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappbuilderasyncjoint.md) -- `interface` -- Async part interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppBuilderJoint](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappbuilderjoint.md) -- `interface` -- Joint interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IMauiAppJoint](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappjoint.md) -- `interface` -- Joint interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`
- [MauiAppAsyncJointDelegate](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappasyncjointdelegate.md) -- `delegate` -- Async delegate for the MauiApp part.
- [MauiAppBuilderAsyncJointDelegate](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the MauiAppBuilder part.
- [MauiAppBuilderHostBuilderExtensions](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying MauiAppBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [MauiAppBuilderJointDelegate](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderjointdelegate.md) -- `delegate` -- Delegate for the MauiAppBuilder part.
- [MauiAppBuilderJointExtensions](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying MauiAppBuilder parts.
- [MauiAppHostBuilderExtensions](/Dovetail/api/joinery.dovetail.hosting.maui/mauiapphostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying MauiApp parts to a `Dovetail.DovetailContextBuilder`.
- [MauiAppJointDelegate](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappjointdelegate.md) -- `delegate` -- Delegate for the MauiApp part.
- [MauiAppJointExtensions](/Dovetail/api/joinery.dovetail.hosting.maui/mauiappjointextensions.md) -- `class` `static` -- Extension methods for applying MauiApp parts.
