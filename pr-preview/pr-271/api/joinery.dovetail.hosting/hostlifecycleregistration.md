# HostLifecycleRegistration

- Kind: `record`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting.md)
- Version: `0.3.1-pr.5`
- Namespace: `Joinery.Dovetail.Hosting`
- Target framework: `net10.0`
- Implements: `IEquatable<HostLifecycleRegistration>`

A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

## Definition

```csharp
namespace Joinery.Dovetail.Hosting;

public sealed record HostLifecycleRegistration
    : System.IEquatable<Joinery.Dovetail.Hosting.HostLifecycleRegistration>
{
    // ...
}
```

## Remarks

Applications don't create these. They are public because every assembly that references the generic host gets its own copy of the (internal, embedded) extension methods and of the hosted service that runs the handlers, and the registrations are the one thing those copies must share: a library's `OnHostStarted` call has to reach the hosted service that the application's copy of the file registered.

The handlers of one moment are started together and awaited together, not one after the other.

## Constructors

- [HostLifecycleRegistration(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/constructors.md#constructor-hostlifecyclemoment-func-iserviceprovider-cancellationtoken-task) -- A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

## Properties

- [Handler](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/properties.md#handler) : `Func<IServiceProvider, CancellationToken, Task>` `get; init` -- The handler. It receives the host's service provider and the cancellation token that the host passes to the lifecycle method.
- [Moment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/properties.md#moment) : [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md) `get; init` -- The moment the `Handler` runs at.

## Methods

- [<Clone>$](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#clone) : [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md)
- [Deconstruct(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#deconstruct-hostlifecyclemoment-func-iserviceprovider-cancellationtoken-task)
- [Equals(object?)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [Equals(HostLifecycleRegistration?)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#equals-hostlifecycleregistration) : `bool` -- Indicates whether the current object is equal to another object of the same type.
- [GetHashCode](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Equality(HostLifecycleRegistration?, HostLifecycleRegistration?)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#op-equality-hostlifecycleregistration-hostlifecycleregistration) : `bool` `static`
- [op_Inequality(HostLifecycleRegistration?, HostLifecycleRegistration?)](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#op-inequality-hostlifecycleregistration-hostlifecycleregistration) : `bool` `static`
- [ToString](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration/methods.md#tostring) : `string` -- Returns a string that represents the current object.
