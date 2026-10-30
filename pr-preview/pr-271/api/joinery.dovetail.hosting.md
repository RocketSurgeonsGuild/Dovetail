# Joinery.Dovetail.Hosting

- Package: `Joinery.Dovetail.Hosting`
- Version: `0.3.1-pr.5`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail.Hosting](https://www.nuget.org/packages/Joinery.Dovetail.Hosting)

## Namespaces

### Joinery.Dovetail.Hosting

- [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md) -- `enum` -- The moments in a host's life at which Dovetail runs the handlers an application registered with `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped`. They are the four phases of `Microsoft.Extensions.Hosting.IHostedLifecycleService`, for the hosts that run it (the generic host and ASP.NET Core).
- [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md) -- `record` `sealed` -- A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

### Joinery.Dovetail.Joints

- [HostApplicationAsyncJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostapplicationasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostApplication part.
- [HostApplicationHostBuilderExtensions](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostapplicationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostApplication parts to a `Dovetail.DovetailContextBuilder`.
- [HostApplicationJointDelegate<TBuilder>](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1.md) -- `delegate` -- Delegate for the HostApplication part.
- [HostApplicationJointExtensions](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostapplicationjointextensions.md) -- `class` `static` -- Extension methods for applying HostApplication parts.
- [IHostApplicationAsyncJoint<TBuilder>](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1.md) -- `interface` -- Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
- [IHostApplicationJoint<TBuilder>](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/ihostapplicationjoint-1.md) -- `interface` -- Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`
