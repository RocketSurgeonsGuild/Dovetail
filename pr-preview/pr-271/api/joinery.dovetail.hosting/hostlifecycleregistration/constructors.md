# HostLifecycleRegistration Constructors

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting.md)
- Type: [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md)
- Kind: `Constructors`
- Members: `1`

A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

<a id="constructor"></a>
<a id="constructor-hostlifecyclemoment-func-iserviceprovider-cancellationtoken-task"></a>

## HostLifecycleRegistration(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)

- Name: `Constructor(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)`

A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

```csharp
public record HostLifecycleRegistration
{
    public HostLifecycleRegistration(
        HostLifecycleMoment Moment,
        Func<IServiceProvider, CancellationToken, Task> Handler)
    {
        // ...
    }
}
```

## Parameters

- `Moment` ([HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md))
  The moment the `Handler` runs at.
- `Handler` (`Func<IServiceProvider, CancellationToken, Task>`)
  The handler. It receives the host's service provider and the cancellation token that the host passes to the lifecycle method.

## Remarks

Applications don't create these. They are public because every assembly that references the generic host gets its own copy of the (internal, embedded) extension methods and of the hosted service that runs the handlers, and the registrations are the one thing those copies must share: a library's `OnHostStarted` call has to reach the hosted service that the application's copy of the file registered.

The handlers of one moment are started together and awaited together, not one after the other.
