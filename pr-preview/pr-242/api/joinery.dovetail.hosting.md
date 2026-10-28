# Joinery.Dovetail.Hosting

- Package: `Joinery.Dovetail.Hosting`
- Version: `0.2.3-pr.8`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Hosting](https://www.nuget.org/packages/Joinery.Dovetail.Hosting)

## Namespaces

### Joinery.Dovetail

- [DovetailHostApplicationHelpers](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/dovetailhostapplicationhelpers.md) -- `class` `static` -- Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an `Hosting.IHostApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.
- [DovetailHostingContextExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/dovetailhostingcontextextensions.md) -- `class` `static` -- Extension members for `Dovetail.IDovetailContext` host environment access.

### Joinery.Dovetail.Joints

- [HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostApplication part.
- [HostApplicationHostBuilderExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostApplication parts to a `Dovetail.DovetailContextBuilder`.
- [HostApplicationJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1.md) -- `delegate` -- Delegate for the HostApplication part.
- [HostApplicationJointExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying HostApplication parts.
- [HostAsyncJointDelegate<THost>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the Host part.
- [HostHostBuilderExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hosthostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Host parts to a `Dovetail.DovetailContextBuilder`.
- [HostJointDelegate<THost>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostjointdelegate-1.md) -- `delegate` -- Delegate for the Host part.
- [HostJointExtensions](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/hostjointextensions.md) -- `class` `static` -- Extension methods for applying Host parts.
- [IHostApplicationAsyncJoint<TBuilder>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1.md) -- `interface` -- Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostApplicationJoint<TBuilder>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/ihostapplicationjoint-1.md) -- `interface` -- Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostAsyncJoint<THost>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/ihostasyncjoint-1.md) -- `interface` -- Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`
- [IHostJoint<THost>](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting/ihostjoint-1.md) -- `interface` -- Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`
