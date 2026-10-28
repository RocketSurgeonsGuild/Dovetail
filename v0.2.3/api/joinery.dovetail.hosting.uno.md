# Joinery.Dovetail.Hosting.Uno

- Package: `Joinery.Dovetail.Hosting.Uno`
- Version: `0.2.3`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Hosting.Uno](https://www.nuget.org/packages/Joinery.Dovetail.Hosting.Uno)

## Namespaces

### Joinery.Dovetail

- [DovetailUnoHelpers](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/dovetailunohelpers.md) -- `class` `static` -- Shared helpers used to wire an Uno.Extensions `Hosting.IApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Joinery.Dovetail.Hosting.Uno

- [GlobalStaticResources](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/globalstaticresources.md) -- `class` `sealed` -- Contains all the static resources defined for the application

### Joinery.Dovetail.Joints

- [IUnoApplicationBuilderAsyncJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderasyncjoint.md) -- `interface` -- Async part interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IUnoApplicationBuilderJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderjoint.md) -- `interface` -- Joint interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IUnoHostBuilderAsyncJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/iunohostbuilderasyncjoint.md) -- `interface` -- Async part interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IUnoHostBuilderJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/iunohostbuilderjoint.md) -- `interface` -- Joint interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [UnoApplicationBuilderAsyncJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the UnoApplicationBuilder part.
- [UnoApplicationBuilderHostBuilderExtensions](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unoapplicationbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying UnoApplicationBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [UnoApplicationBuilderJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate.md) -- `delegate` -- Delegate for the UnoApplicationBuilder part.
- [UnoApplicationBuilderJointExtensions](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying UnoApplicationBuilder parts.
- [UnoHostBuilderAsyncJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the UnoHostBuilder part.
- [UnoHostBuilderHostBuilderExtensions](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying UnoHostBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [UnoHostBuilderJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate.md) -- `delegate` -- Delegate for the UnoHostBuilder part.
- [UnoHostBuilderJointExtensions](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying UnoHostBuilder parts.
