# Dovetail.Hosting.WebAssembly

- Package: `Dovetail.Hosting.WebAssembly`
- Version: `0.2.2-beta.2`
- Target framework: `net10.0`
- NuGet: [Dovetail.Hosting.WebAssembly](https://www.nuget.org/packages/Dovetail.Hosting.WebAssembly)

## Namespaces

### Dovetail

- [DovetailWebAssemblyContextExtensions](/Dovetail/api/dovetail.hosting.webassembly/dovetailwebassemblycontextextensions.md) -- `class` `static` -- Extension members for `IDovetailContext` WebAssembly host environment access.
- [DovetailWebAssemblyHelpers](/Dovetail/api/dovetail.hosting.webassembly/dovetailwebassemblyhelpers.md) -- `class` `static` -- Shared helpers used to wire a `Hosting.WebAssemblyHostBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

### Dovetail.Joints

- [IWebAssemblyHostAsyncJoint](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostasyncjoint.md) -- `interface` -- Async part interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostBuilderAsyncJoint](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostbuilderasyncjoint.md) -- `interface` -- Async part interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostBuilderJoint](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostbuilderjoint.md) -- `interface` -- Joint interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IWebAssemblyHostJoint](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostjoint.md) -- `interface` -- Joint interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`
- [WebAssemblyHostAsyncJointDelegate](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate.md) -- `delegate` -- Async delegate for the WebAssemblyHost part.
- [WebAssemblyHostBuilderAsyncJointDelegate](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the WebAssemblyHostBuilder part.
- [WebAssemblyHostBuilderHostBuilderExtensions](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying WebAssemblyHostBuilder parts to a `DovetailContextBuilder`.
- [WebAssemblyHostBuilderJointDelegate](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate.md) -- `delegate` -- Delegate for the WebAssemblyHostBuilder part.
- [WebAssemblyHostBuilderJointExtensions](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointextensions.md) -- `class` `static` -- Extension methods for applying WebAssemblyHostBuilder parts.
- [WebAssemblyHostHostBuilderExtensions](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhosthostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying WebAssemblyHost parts to a `DovetailContextBuilder`.
- [WebAssemblyHostJointDelegate](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostjointdelegate.md) -- `delegate` -- Delegate for the WebAssemblyHost part.
- [WebAssemblyHostJointExtensions](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostjointextensions.md) -- `class` `static` -- Extension methods for applying WebAssemblyHost parts.
