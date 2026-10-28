# Dovetail.Hosting

- Package: `Dovetail.Hosting`
- Version: `0.2.2-beta.2`
- Target framework: `net10.0`
- NuGet: [Dovetail.Hosting](https://www.nuget.org/packages/Dovetail.Hosting)

## Namespaces

### Dovetail

- [DovetailHostApplicationHelpers](/Dovetail/api/dovetail.hosting/dovetailhostapplicationhelpers.md) -- `class` `static` -- Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an `Hosting.IHostApplicationBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.
- [DovetailHostingContextExtensions](/Dovetail/api/dovetail.hosting/dovetailhostingcontextextensions.md) -- `class` `static` -- Extension members for `IDovetailContext` host environment access.

### Dovetail.Joints

- [HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/api/dovetail.hosting/hostapplicationasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostApplication part.
- [HostApplicationHostBuilderExtensions](/Dovetail/api/dovetail.hosting/hostapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostApplication parts to a `DovetailContextBuilder`.
- [HostApplicationJointDelegate<TBuilder>](/Dovetail/api/dovetail.hosting/hostapplicationjointdelegate-1.md) -- `delegate` -- Delegate for the HostApplication part.
- [HostApplicationJointExtensions](/Dovetail/api/dovetail.hosting/hostapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying HostApplication parts.
- [HostAsyncJointDelegate<THost>](/Dovetail/api/dovetail.hosting/hostasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the Host part.
- [HostHostBuilderExtensions](/Dovetail/api/dovetail.hosting/hosthostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Host parts to a `DovetailContextBuilder`.
- [HostJointDelegate<THost>](/Dovetail/api/dovetail.hosting/hostjointdelegate-1.md) -- `delegate` -- Delegate for the Host part.
- [HostJointExtensions](/Dovetail/api/dovetail.hosting/hostjointextensions.md) -- `class` `static` -- Extension methods for applying Host parts.
- [IHostApplicationAsyncJoint<TBuilder>](/Dovetail/api/dovetail.hosting/ihostapplicationasyncjoint-1.md) -- `interface` -- Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostApplicationJoint<TBuilder>](/Dovetail/api/dovetail.hosting/ihostapplicationjoint-1.md) -- `interface` -- Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostAsyncJoint<THost>](/Dovetail/api/dovetail.hosting/ihostasyncjoint-1.md) -- `interface` -- Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`
- [IHostJoint<THost>](/Dovetail/api/dovetail.hosting/ihostjoint-1.md) -- `interface` -- Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`
