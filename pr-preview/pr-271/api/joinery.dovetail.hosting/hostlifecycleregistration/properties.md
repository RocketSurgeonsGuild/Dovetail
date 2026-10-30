# HostLifecycleRegistration Properties

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting.md)
- Type: [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md)
- Kind: `Properties`
- Members: `2`

A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

<a id="handler"></a>

## Handler

- Name: `Handler`
- Modifiers: `get; init`
- Returns: `Func<IServiceProvider, CancellationToken, Task>`

The handler. It receives the host's service provider and the cancellation token that the host passes to the lifecycle method.

```csharp
public Func<IServiceProvider, CancellationToken, Task> Handler { get; init; }
```

<a id="moment"></a>

## Moment

- Name: `Moment`
- Modifiers: `get; init`
- Returns: [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md)

The moment the `Handler` runs at.

```csharp
public HostLifecycleMoment Moment { get; init; }
```
