# Joinery.Dovetail.Hosting.WebAssembly

- Package: `Joinery.Dovetail.Hosting.WebAssembly`
- Version: `0.2.3-beta.4`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Hosting.WebAssembly](https://www.nuget.org/packages/Joinery.Dovetail.Hosting.WebAssembly)

## Namespaces

### Joinery.Dovetail

- [DovetailWebAssemblyContextExtensions](/Dovetail/api/joinery.dovetail.hosting.webassembly/dovetailwebassemblycontextextensions.md) -- `class` `static` -- Extension members for `Dovetail.IDovetailContext` WebAssembly host environment access.
- [DovetailWebAssemblyHelpers](/Dovetail/api/joinery.dovetail.hosting.webassembly/dovetailwebassemblyhelpers.md) -- `class` `static` -- Shared helpers used to wire a `Hosting.WebAssemblyHostBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Joinery.Dovetail.Joints

- [IWebAssemblyHostAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostasyncjoint.md) -- `interface` -- Async part interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostBuilderAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderasyncjoint.md) -- `interface` -- Async part interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostBuilderJoint](/Dovetail/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderjoint.md) -- `interface` -- Joint interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostJoint](/Dovetail/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostjoint.md) -- `interface` -- Joint interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`
- [WebAssemblyHostAsyncJointDelegate](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate.md) -- `delegate` -- Async delegate for the WebAssemblyHost part.
- [WebAssemblyHostBuilderAsyncJointDelegate](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the WebAssemblyHostBuilder part.
- [WebAssemblyHostBuilderHostBuilderExtensions](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying WebAssemblyHostBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [WebAssemblyHostBuilderJointDelegate](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate.md) -- `delegate` -- Delegate for the WebAssemblyHostBuilder part.
- [WebAssemblyHostBuilderJointExtensions](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying WebAssemblyHostBuilder parts.
- [WebAssemblyHostHostBuilderExtensions](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhosthostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying WebAssemblyHost parts to a `Dovetail.DovetailContextBuilder`.
- [WebAssemblyHostJointDelegate](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostjointdelegate.md) -- `delegate` -- Delegate for the WebAssemblyHost part.
- [WebAssemblyHostJointExtensions](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostjointextensions.md) -- `class` `static` -- Extension methods for applying WebAssemblyHost parts.
