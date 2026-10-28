# Dovetail.Hosting

- Package: `Dovetail.Hosting`
- Version: `0.2.2`
- Target framework: `net10.0`
- NuGet: [Dovetail.Hosting](https://www.nuget.org/packages/Dovetail.Hosting)

## Namespaces

### Dovetail

- [DovetailHostApplicationHelpers](/Dovetail/v0.2.2/api/dovetail.hosting/dovetailhostapplicationhelpers.md) -- `class` `static` -- Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an `Hosting.IHostApplicationBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.
- [DovetailHostingContextExtensions](/Dovetail/v0.2.2/api/dovetail.hosting/dovetailhostingcontextextensions.md) -- `class` `static` -- Extension members for `IDovetailContext` host environment access.

### Dovetail.Joints

- [HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostApplication part.
- [HostApplicationHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostApplication parts to a `DovetailContextBuilder`.
- [HostApplicationJointDelegate<TBuilder>](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationjointdelegate-1.md) -- `delegate` -- Delegate for the HostApplication part.
- [HostApplicationJointExtensions](/Dovetail/v0.2.2/api/dovetail.hosting/hostapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying HostApplication parts.
- [HostAsyncJointDelegate<THost>](/Dovetail/v0.2.2/api/dovetail.hosting/hostasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the Host part.
- [HostHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail.hosting/hosthostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Host parts to a `DovetailContextBuilder`.
- [HostJointDelegate<THost>](/Dovetail/v0.2.2/api/dovetail.hosting/hostjointdelegate-1.md) -- `delegate` -- Delegate for the Host part.
- [HostJointExtensions](/Dovetail/v0.2.2/api/dovetail.hosting/hostjointextensions.md) -- `class` `static` -- Extension methods for applying Host parts.
- [IHostApplicationAsyncJoint<TBuilder>](/Dovetail/v0.2.2/api/dovetail.hosting/ihostapplicationasyncjoint-1.md) -- `interface` -- Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostApplicationJoint<TBuilder>](/Dovetail/v0.2.2/api/dovetail.hosting/ihostapplicationjoint-1.md) -- `interface` -- Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostAsyncJoint<THost>](/Dovetail/v0.2.2/api/dovetail.hosting/ihostasyncjoint-1.md) -- `interface` -- Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`
- [IHostJoint<THost>](/Dovetail/v0.2.2/api/dovetail.hosting/ihostjoint-1.md) -- `interface` -- Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`
